using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Responses;

namespace AzureStorageWrapper
{
    public class AzureStorageWrapper : AzureStorageWrapperBase, IAzureStorageWrapper
    {
        private readonly IUriService _uriService;

        public AzureStorageWrapper(AzureStorageWrapperConfiguration configuration, IUriService uriService) : base(configuration)
        {
            _uriService = uriService;
        }

        public async Task<BlobReference> UploadBlobAsync(UploadBlob command)
        {
            ValidateCommand(command);

            await CreateContainerIfNotExists(command.Container);
            
            var container = await GetContainer(command.Container);
            
            var fileName = Sanitize(command.Name);

            var blobClient = command.UseVirtualFolder
                ? container.GetBlobClient($"{RandomString()}/{fileName}.{command.Extension}")
                : container.GetBlobClient($"{fileName}.{command.Extension}");

            await blobClient.UploadAsync(command.GetContent(), overwrite: true);

            command.Metadata ??= new Dictionary<string, string>();
            command.Metadata.TryAdd("_timestamp", $"{DateTime.UtcNow}");

            await blobClient.SetMetadataAsync(SanitizeDictionary(command.Metadata));

            var sasUri = await GetSasUriAsync(new GetSasUri()
            {
                Uri = blobClient.Uri.AbsoluteUri,
                ExpiresIn = _configuration.DefaultSasUriExpiration,
            });

            var blobReference = new BlobReference()
            {
                Container = command.Container,
                Name = fileName,
                Extension = command.Extension,
                Uri = blobClient.Uri.AbsoluteUri,
                SasUri = sasUri,
                Metadata = command.Metadata,
                SasExpires = DateTime.UtcNow.AddSeconds(_configuration.DefaultSasUriExpiration)
            };

            return blobReference;
        }
        
        public async Task<BlobReference> DownloadBlobReferenceAsync(DownloadBlobReference command)
        {
            ValidateCommand(command);

            var containerName = _uriService.GetContainer(command.Uri);
            var fileName = GetFileName(command.Uri);

            var container = new BlobContainerClient(_configuration.ConnectionString, containerName);
            
            if (! await container.ExistsAsync())
                throw new AzureStorageWrapperException($"container {containerName} doesn't exists!");
            
            var blobClient = container.GetBlobClient($"{fileName.name}.{fileName.extension}");

            if (! await blobClient.ExistsAsync())
                throw new AzureStorageWrapperException($"blob {fileName.name}.{fileName.extension} doesn't exists!");

            var blobProperties = await blobClient.GetPropertiesAsync();

            return new BlobReference()
            {
                Container = blobClient.BlobContainerName,
                Name = fileName.name,
                Extension = fileName.extension,
                Uri = blobClient.Uri.AbsoluteUri,
                SasUri = await GetSasUriAsync(new GetSasUri()
                {
                    Uri = command.Uri,
                    ExpiresIn = command.ExpiresIn <= 0
                        ? _configuration.DefaultSasUriExpiration
                        : command.ExpiresIn,
                }),
                SasExpires = DateTime.MaxValue,
                Metadata = blobProperties.Value.Metadata,
            };
        }

        public async Task DeleteBlobAsync(DeleteBlob command)
        {
            ValidateCommand(command);

            var containerName = _uriService.GetContainer(command.Uri);
            var fileName = GetFileName(command.Uri);

            var container = await GetContainer(containerName);

            if (!await container.ExistsAsync())
                throw new AzureStorageWrapperException($"container {containerName} doesn't exists!");

            var blobClient = container.GetBlobClient($"{fileName.name}.{fileName.extension}");

            if (!await blobClient.ExistsAsync())
                throw new AzureStorageWrapperException($"blob {fileName.name}.{fileName.extension} doesn't exists!");

            await blobClient.DeleteIfExistsAsync();
        }

        private async Task<BlobContainerClient> GetContainer(string containerName)
        {
            return new BlobContainerClient(_configuration.ConnectionString, containerName);
        }

        private async Task CreateContainerIfNotExists(string containerName)
        {
            var container = new BlobContainerClient(_configuration.ConnectionString, containerName);
            
            if (!await container.ExistsAsync())
            {
                if (_configuration.CreateContainerIfNotExists)
                    await container.CreateIfNotExistsAsync();
                else throw new AzureStorageWrapperException($"container {containerName} doesn't exists!");
            }
        }

        private async Task<string> GetSasUriAsync(GetSasUri command)
        {
            ValidateCommand(command);

            var containerName = _uriService.GetContainer(command.Uri);
            var fileName = GetFileName(command.Uri);

            var container = new BlobContainerClient(_configuration.ConnectionString, containerName);

            var blobClient = container.GetBlobClient($"{fileName.name}.{fileName.extension}");

            if (!await blobClient.ExistsAsync()) return null;

            var blobSasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddSeconds(command.ExpiresIn));

            return blobSasUri.AbsoluteUri;
        }
        
        private (string name, string extension) GetFileName(string uri)
        {
            var unescapedUriRepresentation = Uri.UnescapeDataString(uri);

            var container = _uriService.GetContainer(unescapedUriRepresentation);
            var host = _uriService.GetHost(unescapedUriRepresentation);
            var extension = _uriService.GetFileExtension(unescapedUriRepresentation);

            var temp = unescapedUriRepresentation
                .Replace($"{host}/", string.Empty)
                .Replace($"{container}/", string.Empty)
                .Replace($".{extension}", string.Empty);
            
            return (name: temp, extension: extension);
        }
        
        private static string Sanitize(string fileName)
        {
            var withoutBlanks = RemoveBlanks(fileName);

            var withoutDiacritics = RemoveDiacritics(withoutBlanks);

            return withoutDiacritics;

            static string RemoveBlanks(string fileName)
            {
                return Regex.Replace(fileName, @"[^\w\d\-]", "_");
            }

            static string RemoveDiacritics(string fileName)
            {
                var normalizedString = fileName.Normalize(NormalizationForm.FormD);

                var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

                foreach (var @char in normalizedString)
                {
                    var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(@char);

                    if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(@char);
                    }
                }

                return stringBuilder
                    .ToString()
                    .Normalize(NormalizationForm.FormC);
            }
        }
    }
}
