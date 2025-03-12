using System.IO;
using System.Threading.Tasks;
using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Queries;
using AzureStorageWrapper.Responses;

namespace AzureStorageWrapper
{
    /// <summary>
    /// Interface for Azure Storage Wrapper.
    /// </summary>
    public interface IAzureStorageWrapper
    {
        /// <summary>
        /// Uploads a blob to an Azure Storage container.
        /// </summary>
        /// <param name="file">The name of the file.</param>
        /// <param name="content">The content of the file as a stream.</param>
        /// <param name="container">The name of the container. Optional.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the blob reference.</returns>
        Task<BlobReference> UploadBlobAsync(string file, Stream content, string container = null);

        /// <summary>
        /// Uploads a blob to an Azure Storage container.
        /// </summary>
        /// <param name="file">The name of the file.</param>
        /// <param name="content">The content of the file as a byte array.</param>
        /// <param name="container">The name of the container. Optional.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the blob reference.</returns>
        Task<BlobReference> UploadBlobAsync(string file, byte[] content, string container = null);

        /// <summary>
        /// Uploads a blob to an Azure Storage container.
        /// </summary>
        /// <param name="file">The name of the file.</param>
        /// <param name="contentBase64">The content of the file as a Base64 string.</param>
        /// <param name="container">The name of the container. Optional.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the blob reference.</returns>
        Task<BlobReference> UploadBlobAsync(string file, string contentBase64, string container = null);

        /// <summary>
        /// Uploads a blob (base64, stream, or bytes) to an Azure Storage container.
        /// </summary>
        /// <param name="command">The upload command.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the blob reference.</returns>
        Task<BlobReference> UploadBlobAsync(UploadBlob command);

        /// <summary>
        /// Downloads a blob reference from an Azure Storage container.
        /// </summary>
        /// <param name="uri">The URI of the blob.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the blob reference.</returns>
        Task<BlobReference> DownloadBlobReferenceAsync(string uri);

        /// <summary>
        /// Downloads a SAS URI for a blob.
        /// </summary>
        /// <param name="command">The download command.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the blob reference.</returns>
        Task<BlobReference> DownloadBlobReferenceAsync(DownloadBlobReference command);

        /// <summary>
        /// Downloads a blob from an Azure Storage container.
        /// </summary>
        /// <param name="uri">The URI of the blob.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the blob.</returns>
        Task<Blob> DownloadBlobAsync(string uri);

        /// <summary>
        /// Downloads a blob from an Azure Storage container.
        /// </summary>
        /// <param name="command">The download command.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the blob.</returns>
        Task<Blob> DownloadBlobAsync(DownloadBlob command);

        /// <summary>
        /// Deletes a blob from an Azure Storage container.
        /// </summary>
        /// <param name="uri">The URI of the blob.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteBlobAsync(string uri);

        /// <summary>
        /// Deletes a blob from an Azure Storage container.
        /// </summary>
        /// <param name="command">The delete command.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteBlobAsync(DeleteBlob command);

        /// <summary>
        /// Enumerates blobs inside an Azure Storage container.
        /// </summary>
        /// <param name="paginateSize">The size of the pagination.</param>
        /// <param name="container">The name of the container. Optional.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the blob reference collection.</returns>
        Task<BlobReferenceCollection> EnumerateBlobsAsync(int paginateSize, string container = null);

        /// <summary>
        /// Enumerates blobs inside an Azure Storage container.
        /// </summary>
        /// <param name="container">The name of the container. Optional.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the blob reference collection.</returns>
        Task<BlobReferenceCollection> EnumerateBlobsAsync(string container = null);

        /// <summary>
        /// Enumerates and paginates blobs inside an Azure Storage container.
        /// </summary>
        /// <param name="command">The enumerate command.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the blob reference collection.</returns>
        Task<BlobReferenceCollection> EnumerateBlobsAsync(EnumerateBlobs command);
    }
}
