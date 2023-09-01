using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using AzureStorageWrapper.Responses;
using AzureStorageWrapper.Models;

namespace AzureStorageWrapper
{
    public class AzureStorageWrapper : AzureStorageWrapperBase, IAzureStorageWrapper
    {
        private readonly AzureStorageWrapperConfiguration _configuration;

        public AzureStorageWrapper(AzureStorageWrapperConfiguration configuration) : base(configuration) 
        {
            _configuration = configuration;
        }

        public async Task<BlobReference> UploadBlobAsync(UploadBlob command)
        {
            ValidateUploadBlobCommand(command);

            var container = new BlobContainerClient(_configuration.ConnectionString, command.Container);

            await container.CreateIfNotExistsAsync();

            var generatedName = GenerateBlobName();

            var blobPath = command.GetFullName(generatedName);

            var blobClient = container.GetBlobClient(blobPath);

            await blobClient.UploadAsync(command.GetContent(), overwrite: true);

            command.Metadata ??= new Dictionary<string, string>();

            command.Metadata.TryAdd("TIMESTAMP", $"{DateTime.UtcNow}");
            command.Metadata.TryAdd("ORIGINAL_FILE_NAME", command.Name);
            command.Metadata.TryAdd("ORIGINAL_FILE_EXTENSION", command.Extension);

            await blobClient.SetMetadataAsync(SanitizeDictionary(command.Metadata));

            var sasUri = await GetSasUriAsync(new GetSasUri()
            {
                Container = command.Container,
                Name = generatedName,
                Extension = command.Extension,
                ExpiresIn = _configuration.DefaultSasUriExpiration,
                Path = command.Path
            });

            var referenceEntity = new BlobReference()
            {
                Uri = blobClient.Uri.AbsoluteUri,
                SasUri = sasUri,
                Name = generatedName,
                Extension = command.Extension,
                FullName = blobClient.Name,
                Metadata = command.Metadata,
                Container = command.Container,
                SasExpires = DateTime.UtcNow.AddSeconds(_configuration.DefaultSasUriExpiration)
            };

            return referenceEntity;
        }


        public async Task<BlobReference> DownloadBlobReferenceAsync(DownloadBlobReference command)
        {
            ValidateDownloadBlobReferenceCommand(command);

            var containerClient = new BlobContainerClient(_configuration.ConnectionString, command.Container);

            var blobClient = containerClient.GetBlobClient($"{command.Name}.{command.Extension}");

            var blobProperties = await blobClient.GetPropertiesAsync();

            return new BlobReference()
            {
                Name = command.Name,
                Extension = command.Extension,
                FullName = blobClient.Name,
                Container = command.Container,
                Uri = blobClient.Uri.AbsoluteUri,
                SasUri = await GetSasUriAsync(new GetSasUri()
                {
                    Container = command.Container,
                    Name = command.Name,
                    Extension = command.Extension,
                    ExpiresIn = command.ExpiresIn <= 0 
                        ? _configuration.DefaultSasUriExpiration 
                        : command.ExpiresIn,
                }),
                SasExpires = DateTime.MaxValue,
                Metadata = blobProperties.Value.Metadata,
            };

        }

        private async Task<string> GetSasUriAsync(GetSasUri command)
        {
            ValidateGetSasUriCommand(command);

            var container = new BlobContainerClient(_configuration.ConnectionString, command.Container);

            var blobPath = command.GetFilePath();

            var blobClient = container.GetBlobClient(blobPath);

            if (!await blobClient.ExistsAsync()) return null;

            var blobSasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddSeconds(command.ExpiresIn));

            return blobSasUri.AbsoluteUri;
        }

        
    }
}
