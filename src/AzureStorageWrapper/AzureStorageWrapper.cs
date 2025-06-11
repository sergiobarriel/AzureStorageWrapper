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
using AzureStorageWrapper.Extensions;
using AzureStorageWrapper.Queries;
using AzureStorageWrapper.Responses;
using EnsureThat;

namespace AzureStorageWrapper
{
    /// <summary>
    /// Provides methods to interact with Azure Storage.
    /// </summary>
    public class AzureStorageWrapper : AzureStorageWrapperBase, IAzureStorageWrapper
    {
        private readonly AzureStorageWrapperOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageWrapper"/> class.
        /// </summary>
        /// <param name="options">The options for configuring the Azure Storage Wrapper.</param>
        public AzureStorageWrapper(AzureStorageWrapperOptions options)
            => _options = options;

        #region UploadBlobs

        /// <inheritdoc/>
        public async Task<BlobReference> UploadBlobAsync(string file, Stream content, string container = null)
            => await UploadBlobImplAsync(file, new UploadStream { Stream = content, Container = container });

        /// <inheritdoc/>
        public async Task<BlobReference> UploadBlobAsync(string file, byte[] content, string container = null)
            => await UploadBlobImplAsync(file, new UploadBytes { Bytes = content, Container = container });

        /// <inheritdoc/>
        public async Task<BlobReference> UploadBlobAsync(string file, string contentBase64, string container = null)
            => await UploadBlobImplAsync(file, new UploadBase64 { Base64 = contentBase64, Container = container });

        /// <summary>
        /// Implements the logic for uploading a blob.
        /// </summary>
        /// <typeparam name="T">The type of the upload command.</typeparam>
        /// <param name="file">The name of the file.</param>
        /// <param name="command">The upload command.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the blob reference.</returns>
        private async Task<BlobReference> UploadBlobImplAsync<T>(string file, T command) where T : UploadBlob
        {
            if (string.IsNullOrEmpty(command.Container))
            {
                Ensure.String.IsNotNullOrEmptySW(_options.DefaultContainer);
                command.Container = _options.DefaultContainer;
            }

            command.Name = Path.GetFileNameWithoutExtension(file);
            command.Extension = Path.GetExtension(file);
            command.UseVirtualFolder = false;

            return await UploadBlobAsync(command);
        }

        /// <inheritdoc/>
        public async Task<BlobReference> UploadBlobAsync(UploadBlob command)
        {
            command.Validate();

            var container = new BlobContainerClient(_options.ConnectionString, command.Container);

            if (!await container.ExistsAsync())
            {
                Ensure.Bool.IsNotExistContainer(_options.CreateContainerIfNotExists, command.Container);
                await container.CreateIfNotExistsAsync();
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
                ExpiresIn = _options.DefaultSasUriExpiration,
            });

            var blobReference = new BlobReference()
            {
                Container = command.Container,
                Name = command.Name,
                Extension = command.Extension,
                Uri = blob.Uri.AbsoluteUri,
                SasUri = sasUri,
                Metadata = sanitizedDictionary,
                SasExpires = DateTime.UtcNow.AddSeconds(_options.DefaultSasUriExpiration)
            };

            return blobReference;
        }
        #endregion

        #region DownloadBlobs

        /// <inheritdoc/>
        public async Task<BlobReference> DownloadBlobReferenceAsync(string uri)
            => await DownloadBlobReferenceAsync(new DownloadBlobReference { Uri = uri });

        /// <inheritdoc/>
        public async Task<BlobReference> DownloadBlobReferenceAsync(DownloadBlobReference command)
        {
            command.Validate(_options);

            var blob = new BlobClient(new Uri(command.Uri));

            var container = new BlobContainerClient(_options.ConnectionString, blob.BlobContainerName);

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
                        ? _options.DefaultSasUriExpiration
                        : command.ExpiresIn,
                }),
                SasExpires = DateTime.MaxValue,
                Metadata = blobProperties.Value.Metadata,
            };
        }

        /// <inheritdoc/>
        public async Task<Blob> DownloadBlobAsync(string uri)
            => await DownloadBlobAsync(new DownloadBlob { Uri = uri });

        /// <inheritdoc/>
        public async Task<Blob> DownloadBlobAsync(DownloadBlob command)
        {
            command.Validate();

            var sasUri = await GetSasUriAsync(new GetSasUri()
            {
                Uri = command.Uri,
                ExpiresIn = _options.DefaultSasUriExpiration,
            });

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(sasUri);

                Ensure.Bool.IsTrue(response.IsSuccessStatusCode, $"something went wrong when downloading blob {command.Uri}");

                var stream = await response.Content.ReadAsStreamAsync();

                return new Blob()
                {
                    Stream = stream
                };
            }
        }
        #endregion

        #region DeleteBlobs

        /// <inheritdoc/>
        public async Task DeleteBlobAsync(string uri)
            => await DeleteBlobAsync(new DeleteBlob { Uri = uri });

        /// <inheritdoc/>
        public async Task DeleteBlobAsync(DeleteBlob command)
        {
            command.Validate();

            var blob = new BlobClient(new Uri(command.Uri));

            var container = new BlobContainerClient(_options.ConnectionString, blob.BlobContainerName);

            var blobClient = container.GetBlobClient(blob.Name);

            await blobClient.DeleteIfExistsAsync();
        }
        #endregion

        #region EnumerateBlobs

        /// <inheritdoc/>
        public async Task<BlobReferenceCollection> EnumerateBlobsAsync(int paginateSize, string container = null)
            => await EnumerateImplBlobsAsync(new EnumerateBlobs { Container = container, Paginate = true, Size = paginateSize });

        /// <inheritdoc/>
        public async Task<BlobReferenceCollection> EnumerateBlobsAsync(string container = null)
            => await EnumerateImplBlobsAsync(new EnumerateBlobs { Container = container, Paginate = false });

        private async Task<BlobReferenceCollection> EnumerateImplBlobsAsync(EnumerateBlobs command)
        {
            if (string.IsNullOrEmpty(command.Container))
            {
                Ensure.String.IsNotNullOrEmptySW(_options.DefaultContainer);
                command.Container = _options.DefaultContainer;
            }

            return await EnumerateBlobsAsync(command);
        }

        /// <inheritdoc/>
        public async Task<BlobReferenceCollection> EnumerateBlobsAsync(EnumerateBlobs command)
        {
            command.Validate();

            var container = new BlobContainerClient(_options.ConnectionString, command.Container);

            var segment = container
                .GetBlobsAsync()
                .AsPages(command.Paginate ? command.ContinuationToken : null, command.Paginate ? command.Size : (int?)null);

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
                        ExpiresIn = _options.DefaultSasUriExpiration
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
        #endregion

        /// <summary>
        /// Generates a SAS URI for a blob.
        /// </summary>
        /// <param name="command">The SAS URI command.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the SAS URI.</returns>
        private async Task<string> GetSasUriAsync(GetSasUri command)
        {
            command.Validate(_options);

            var blob = new BlobClient(new Uri(command.Uri));

            var container = new BlobContainerClient(_options.ConnectionString, blob.BlobContainerName);

            var blobClient = container.GetBlobClient(blob.Name);

            if (!await blobClient.ExistsAsync()) return null;

            var blobSasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddSeconds(command.ExpiresIn));

            return blobSasUri.AbsoluteUri;
        }

        /// <summary>
        /// Sanitizes a dictionary by removing diacritics and replacing invalid characters.
        /// </summary>
        /// <param name="metadata">The metadata dictionary to sanitize.</param>
        /// <returns>The sanitized dictionary.</returns>
        private static Dictionary<string, string> SanitizeDictionary(Dictionary<string, string> metadata)
        {
            return metadata.ToDictionary(item => SanitizeKey(item.Key), item => SanitizeValue(item.Value));

            string SanitizeKey(string key)
            {
                key = RemoveDiacritics(key);

                key = Regex.Replace(key, @"[^a-zA-Z0-9]+", "_");

                return key;
            }

            string SanitizeValue(string value)
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
