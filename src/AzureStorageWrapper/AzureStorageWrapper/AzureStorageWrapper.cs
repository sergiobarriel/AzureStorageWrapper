using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Exceptions;
using AzureStorageWrapper.Responses;

namespace AzureStorageWrapper
{
    public class AzureStorageWrapper : IAzureStorageWrapper
    {
        private readonly AzureStorageWrapperConfiguration _configuration;
        private readonly IUriService _uriService = new UriService();

        public AzureStorageWrapper(AzureStorageWrapperConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BlobReference> UploadBlobAsync(UploadBlob command)
        {
            command.Validate();

            await CreateContainerIfNotExists(command.Container);
            
            var container = GetContainer(command.Container);
            
            var blobClient = command.UseVirtualFolder
                ? container.GetBlobClient($"{GetRandomId()}/{command.Name}.{command.Extension}")
                : container.GetBlobClient($"{command.Name}.{command.Extension}");

            await blobClient.UploadAsync(command.GetContent(), overwrite: true);

            command.Metadata ??= new Dictionary<string, string>();
            command.Metadata.TryAdd("_timestamp", $"{DateTime.UtcNow}");

            var sanitizedDictionary = SanitizeDictionary(command.Metadata);
            
            await blobClient.SetMetadataAsync(sanitizedDictionary);

            var sasUri = await GetSasUriAsync(new GetSasUri()
            {
                Uri = blobClient.Uri.AbsoluteUri,
                ExpiresIn = _configuration.DefaultSasUriExpiration,
            });

            var blobReference = new BlobReference()
            {
                Container = command.Container,
                Name = command.Name,
                Extension = command.Extension,
                Uri = blobClient.Uri.AbsoluteUri,
                SasUri = sasUri,
                Metadata = sanitizedDictionary,
                SasExpires = DateTime.UtcNow.AddSeconds(_configuration.DefaultSasUriExpiration)
            };

            return blobReference;
        }
        
        public async Task<BlobReference> DownloadBlobReferenceAsync(DownloadBlobReference command)
        {
            command.Validate(_configuration);

            var containerName = _uriService.GetContainer(command.Uri);
            var fileName = GetFileName(command.Uri);

            var container = GetContainer(containerName);
            
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
            command.Validate();

            var containerName = _uriService.GetContainer(command.Uri);
            var fileName = GetFileName(command.Uri);

            var container = GetContainer(containerName);

            if (!await container.ExistsAsync())
                throw new AzureStorageWrapperException($"container {containerName} doesn't exists!");

            var blobClient = container.GetBlobClient($"{fileName.name}.{fileName.extension}");

            if (!await blobClient.ExistsAsync())
                throw new AzureStorageWrapperException($"blob {fileName.name}.{fileName.extension} doesn't exists!");

            await blobClient.DeleteIfExistsAsync();
        }

        private BlobContainerClient GetContainer(string containerName)
        {
            return new BlobContainerClient(_configuration.ConnectionString, containerName);
        }

        private async Task CreateContainerIfNotExists(string containerName)
        {
            var container = GetContainer(containerName);
            
            if (!await container.ExistsAsync())
            {
                if (_configuration.CreateContainerIfNotExists)
                    await container.CreateIfNotExistsAsync();
                
                else throw new AzureStorageWrapperException($"container {containerName} doesn't exists!");
            }
        }
        
        public async Task<Blob> DownloadBlobAsync(DownloadBlob command)
        {
            command.Validate();
            
            using var httpClient = new HttpClient();
            
            var response = await httpClient.GetAsync(command.Uri);

            if (!response.IsSuccessStatusCode)
            {
                var fileName = _uriService.GetFileName(command.Uri);
                var fileExtension = _uriService.GetFileExtension(command.Uri);
                
                throw new AzureStorageWrapperException($"something went wrong when downloading blob {fileName}.{fileExtension}: {response.ReasonPhrase}");
            }
            
            var stream = await response.Content.ReadAsStreamAsync();

            return new Blob()
            {
                Stream = stream
            };
        }

        private async Task<string> GetSasUriAsync(GetSasUri command)
        {
            command.Validate(_configuration);

            var fileName = GetFileName(command.Uri);

            var container = GetContainer(_uriService.GetContainer(command.Uri));

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
        
        private static Dictionary<string, string> SanitizeDictionary(Dictionary<string, string> metadata)
        {
            return metadata.ToDictionary(item => SanitizeKey(item.Key), item => SanitizeValue(item.Value));

            static string SanitizeKey(string key)
            {
                key = RemoveDiacritics(key);
                
                key = Regex.Replace(key, @"[^a-zA-Z0-9]+", "_");

                return key;
            }
            
            static string SanitizeValue(string @value)
            {
                value = RemoveDiacritics(value);

                return value;
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
        
        /// <summary>
        /// ~2 centuries of work are needed in order to have a 1% probability of at least one collision: https://alex7kom.github.io/nano-nanoid-cc
        /// </summary>
        private static string GetRandomId()
        {
            var guid = Guid.NewGuid().ToString("N");

            return guid.Substring(0, 15);
        }
    }
}
