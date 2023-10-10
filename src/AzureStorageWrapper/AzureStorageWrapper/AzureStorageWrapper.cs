using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        public AzureStorageWrapper(AzureStorageWrapperConfiguration configuration) : base(configuration)
        {

        }

        public async Task<BlobReference> UploadBlobAsync(UploadBlob command)
        {
            Validate(command);

            var container = new BlobContainerClient(_configuration.ConnectionString, command.Container);
            
            if (!await container.ExistsAsync())
            {
                if (_configuration.CreateContainerIfNotExists)
                    await container.CreateIfNotExistsAsync();
                else throw new AzureStorageWrapperException($"{command.Container} doesn't exists!");
            }

            var folder = Guid.NewGuid().ToString("N")[..15];

            var sanitizedFileName = SanitizeFileName(command.Name);

            var path = $"{folder}/{sanitizedFileName}.{command.Extension}";

            var blobClient = container.GetBlobClient(path);

            await blobClient.UploadAsync(command.GetContent(), overwrite: true);

            command.Metadata ??= new Dictionary<string, string>();

            command.Metadata.TryAdd("asw_folder", folder);
            command.Metadata.TryAdd("asw_timestamp", $"{DateTime.UtcNow}");

            await blobClient.SetMetadataAsync(SanitizeDictionary(command.Metadata));

            var sasUri = await GetSasUriAsync(new GetSasUri()
            {
                Uri = blobClient.Uri.AbsoluteUri,
                ExpiresIn = _configuration.DefaultSasUriExpiration,
            });

            var referenceEntity = new BlobReference()
            {
                Container = command.Container,
                Name = sanitizedFileName,
                Extension = command.Extension,
                Uri = blobClient.Uri.AbsoluteUri,
                SasUri = sasUri,
                Metadata = command.Metadata,
                SasExpires = DateTime.UtcNow.AddSeconds(_configuration.DefaultSasUriExpiration)
            };

            return referenceEntity;
        }

        public async Task<BlobReference> DownloadBlobReferenceAsync(DownloadBlobReference command)
        {
            Validate(command);

            var container = new BlobContainerClient(_configuration.ConnectionString, GetContainerFromUri(command.Uri));

            if (! await container.ExistsAsync())
                throw new AzureStorageWrapperException($"{GetContainerFromUri(command.Uri)} doesn't exists!");
            
            var blobClient = container.GetBlobClient(GetBlobNameFromUri(command.Uri));

            if (! await blobClient.ExistsAsync())
                throw new AzureStorageWrapperException($"Uri {command.Uri} doesn't exists!");

            var blobProperties = await blobClient.GetPropertiesAsync();

            var fileNameAndExtension = GetFileNameFromUri(command.Uri);

            return new BlobReference()
            {
                Container = blobClient.BlobContainerName,
                Name = fileNameAndExtension.name,
                Extension = fileNameAndExtension.extension,
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
            Validate(command);

            var container = new BlobContainerClient(_configuration.ConnectionString, GetContainerFromUri(command.Uri));

            if (!await container.ExistsAsync())
                throw new AzureStorageWrapperException($"{GetContainerFromUri(command.Uri)} doesn't exists!");

            var blobClient = container.GetBlobClient(GetBlobNameFromUri(command.Uri));

            if (!await blobClient.ExistsAsync())
                throw new AzureStorageWrapperException($"Uri {command.Uri} doesn't exists!");

            await blobClient.DeleteIfExistsAsync();
        }

        private async Task<string> GetSasUriAsync(GetSasUri command)
        {
            Validate(command);

            var container = new BlobContainerClient(_configuration.ConnectionString, GetContainerFromUri(command.Uri));

            var path = GetBlobNameFromUri(command.Uri);

            var blobClient = container.GetBlobClient(path);

            if (!await blobClient.ExistsAsync()) return null;

            var blobSasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddSeconds(command.ExpiresIn));

            return blobSasUri.AbsoluteUri;
        }

        private (string name, string extension) GetFileNameFromUri(string uri)
        {
            var uriObject = new Uri(uri);

            var fileName = string.Join(string.Empty, uriObject.Segments.Last());

            var parts = fileName.Split(".");

            return (name: parts[0], extension: parts[1]);
        }

        private string GetContainerFromUri(string uri)
        {
            var uriObject = new Uri(uri);

            return uriObject.Segments[1].TrimEnd('/');
        }

        private string GetBlobNameFromUri(string uri)
        {
            var uriObject = new Uri(uri);

            return string.Join(string.Empty, uriObject.Segments[2..]);
        }

        private static string SanitizeFileName(string fileName)
        {
            var withoutBlanks = RemoveBlanks(fileName);

            var withoutDiacritics = RemoveDiacritics(withoutBlanks);

            return withoutDiacritics;
        }

        private static string RemoveBlanks(string fileName)
        {
            return Regex.Replace(fileName, @"[^\w\d\-]", "_");
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);

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
