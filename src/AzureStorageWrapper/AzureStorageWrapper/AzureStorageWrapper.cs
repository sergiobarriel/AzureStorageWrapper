using System;
using System.Collections.Generic;
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

            var path = $"{folder}/{command.Name}.{command.Extension}";

            var blobClient = container.GetBlobClient(path);

            await blobClient.UploadAsync(command.GetContent(), overwrite: true);

            command.Metadata ??= new Dictionary<string, string>();

            command.Metadata.TryAdd("ASW_FOLDER", folder);
            command.Metadata.TryAdd("ASW_TIMESTAMP", $"{DateTime.UtcNow}");

            await blobClient.SetMetadataAsync(SanitizeDictionary(command.Metadata));

            var sasUri = await GetSasUriAsync(new GetSasUri()
            {
                Folder = folder,
                Container = command.Container,
                Name = command.Name,
                Extension = command.Extension,
                ExpiresIn = _configuration.DefaultSasUriExpiration,
            });

            var referenceEntity = new BlobReference()
            {
                Folder = folder,
                Name = command.Name,
                Extension = command.Extension,
                FullName = blobClient.Name,
                Uri = blobClient.Uri.AbsoluteUri,
                SasUri = sasUri,
                Metadata = command.Metadata,
                Container = command.Container,
                SasExpires = DateTime.UtcNow.AddSeconds(_configuration.DefaultSasUriExpiration)
            };

            return referenceEntity;
        }


        public async Task<BlobReference> DownloadBlobReferenceAsync(DownloadBlobReference command)
        {
            Validate(command);

            var containerClient = new BlobContainerClient(_configuration.ConnectionString, command.Container);

            var blobClient = containerClient.GetBlobClient($"{command.Folder}/{command.Name}.{command.Extension}");

            var blobProperties = await blobClient.GetPropertiesAsync();

            return new BlobReference()
            {
                Folder = command.Folder,
                Name = command.Name,
                Extension = command.Extension,
                FullName = blobClient.Name,
                Container = command.Container,
                Uri = blobClient.Uri.AbsoluteUri,
                SasUri = await GetSasUriAsync(new GetSasUri()
                {
                    Folder = command.Folder,
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
            Validate(command);

            var container = new BlobContainerClient(_configuration.ConnectionString, command.Container);

            var blobPath = command.GeneratePath();

            var blobClient = container.GetBlobClient(blobPath);

            if (!await blobClient.ExistsAsync()) return null;

            var blobSasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddSeconds(command.ExpiresIn));

            return blobSasUri.AbsoluteUri;
        }

        
    }
}
