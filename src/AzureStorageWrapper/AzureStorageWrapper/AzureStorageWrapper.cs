using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
    public class AzureStorageWrapper : AzureStorageWrapperBase, IAzureStorageWrapper
    {
        private readonly AzureStorageWrapperConfiguration _configuration;

        public AzureStorageWrapper(AzureStorageWrapperConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BlobReference> UploadBlobAsync(UploadBlob command)
        {
            command.Validate();
           
            var container = new BlobContainerClient(_configuration.ConnectionString, command.Container);
            
            if (!await container.ExistsAsync())
            {
                if (_configuration.CreateContainerIfNotExists) await container.CreateIfNotExistsAsync();
                
                else throw new AzureStorageWrapperException($"container {command.Container} doesn't exists!");
            }

            var blobName = command.UseVirtualFolder
                ? $"{GetRandomId()}/{command.Name}.{command.Extension}"
                : $"{command.Name}.{command.Extension}";
            
            var blob = container.GetBlobClient(blobName);
            
            await blob.UploadAsync(command.GetContent(), overwrite: true);

            var sanitizedDictionary = SanitizeDictionary(command.Metadata);
            
            await blob.SetMetadataAsync(sanitizedDictionary);

            var sasUri = await GetSasUriAsync(new GetSasUri()
            {
                Uri = blob.Uri.AbsoluteUri,
                ExpiresIn = _configuration.DefaultSasUriExpiration,
            });

            var blobReference = new BlobReference()
            {
                Container = command.Container,
                Name = command.Name,
                Extension = command.Extension,
                Uri = blob.Uri.AbsoluteUri,
                SasUri = sasUri,
                Metadata = sanitizedDictionary,
                SasExpires = DateTime.UtcNow.AddSeconds(_configuration.DefaultSasUriExpiration)
            };

            return blobReference;

        }
        
        public async Task<BlobReference> DownloadBlobReferenceAsync(DownloadBlobReference command)
        {
            command.Validate(_configuration);

            var blob = new BlobClient(new Uri(command.Uri));
            
            var container = new BlobContainerClient(_configuration.ConnectionString, blob.BlobContainerName);
            
            var blobClient = container.GetBlobClient(blob.Name);

            var blobProperties = await blobClient.GetPropertiesAsync();
            
            return new BlobReference()
            {
                Container = blobClient.BlobContainerName,
                Name = blobClient.Name,
                Extension = Path.GetExtension(blobClient.Name),
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
        
        public async Task<Blob> DownloadBlobAsync(DownloadBlob command)
        {
            command.Validate();

            var sasUri = await GetSasUriAsync(new GetSasUri()
            {
                Uri = command.Uri,
                ExpiresIn = _configuration.DefaultSasUriExpiration,
            });
            
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(sasUri);

                if (!response.IsSuccessStatusCode)
                {
                    throw new AzureStorageWrapperException($"something went wrong when downloading blob {command.Uri}");
                }

                var stream = await response.Content.ReadAsStreamAsync();
                
                return new Blob()
                {
                    Stream = stream
                };
            }
        }
        
        public async Task DeleteBlobAsync(DeleteBlob command)
        {
            command.Validate();

            var blob = new BlobClient(new Uri(command.Uri));
            
            var container = new BlobContainerClient(_configuration.ConnectionString, blob.BlobContainerName);
            
            var blobClient = container.GetBlobClient(blob.Name);

            await blobClient.DeleteIfExistsAsync();
        }
        
        public async Task<BlobReferenceCollection> EnumerateBlobsAsync(EnumerateBlobs command)
        {
            command.Validate();
            
            var container = new BlobContainerClient(_configuration.ConnectionString, command.Container);

            var segment = container
                .GetBlobsAsync()
                .AsPages(command.ContinuationToken, command.Size);
            
            var enumerator = segment.GetAsyncEnumerator();
            
            var references = new List<BlobReference>();
            
            while (await enumerator.MoveNextAsync())
            {
                var page = enumerator.Current;

                foreach (var item in page.Values)
                {
                    var blobReference = await DownloadBlobReferenceAsync(new DownloadBlobReference()
                    {
                        Uri = $"{container.Uri}/{item.Name}",
                        ExpiresIn = _configuration.DefaultSasUriExpiration
                    });

                    references.Add(blobReference);
                }
                
                await enumerator.DisposeAsync();

                return new BlobReferenceCollection()
                {
                    References = references,
                    ContinuationToken = page.ContinuationToken
                };
            }

            return new BlobReferenceCollection();
        }
        
        public async Task<BlobReferenceCollection> EnumerateAllBlobsAsync(EnumerateAllBlobs command)
        {
            command.Validate();
            
            var container = new BlobContainerClient(_configuration.ConnectionString, command.Container);

            var segment = container
                .GetBlobsAsync()
                .AsPages(null, 10);
            
            var enumerator = segment.GetAsyncEnumerator();
            
            var references = new List<BlobReference>();
            
            while (await enumerator.MoveNextAsync())
            {
                var page = enumerator.Current;

                foreach (var item in page.Values)
                {
                    var blobReference = await DownloadBlobReferenceAsync(new DownloadBlobReference()
                    {
                        Uri = $"{container.Uri}/{item.Name}",
                        ExpiresIn = _configuration.DefaultSasUriExpiration
                    });

                    references.Add(blobReference);
                }
                
                await enumerator.DisposeAsync();

                return new BlobReferenceCollection()
                {
                    References = references,
                    ContinuationToken = page.ContinuationToken
                };
            }

            return new BlobReferenceCollection();
        }
        
        private async Task<string> GetSasUriAsync(GetSasUri command)
        {
            command.Validate(_configuration);

            var blob = new BlobClient(new Uri(command.Uri));
            
            var container = new BlobContainerClient(_configuration.ConnectionString, blob.BlobContainerName);
            
            var blobClient = container.GetBlobClient(blob.Name);

            if (!await blobClient.ExistsAsync()) return null;

            var blobSasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddSeconds(command.ExpiresIn));

            return blobSasUri.AbsoluteUri;
        }
        
        private static Dictionary<string, string> SanitizeDictionary(Dictionary<string, string> metadata)
        {
            return metadata.ToDictionary(item => SanitizeKey(item.Key), item => SanitizeValue(item.Value));

            string SanitizeKey(string key)
            {
                key = RemoveDiacritics(key);
                
                key = Regex.Replace(key, @"[^a-zA-Z0-9]+", "_");

                return key;
            }
            
            string SanitizeValue(string @value)
            {
                value = RemoveDiacritics(value);

                return value;
            }
            
            string RemoveDiacritics(string fileName)
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
