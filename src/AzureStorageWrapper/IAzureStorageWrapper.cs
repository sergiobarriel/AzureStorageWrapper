using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Queries;
using AzureStorageWrapper.Responses;

namespace AzureStorageWrapper
{
    public interface IAzureStorageWrapper
    {
        /// <summary>
        /// Upload blob (base64, stream or bytes) to an Azure Storage container
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<BlobReference> UploadBlobAsync(UploadBlob command);

        /// <summary>
        /// Download SAS Uri for a blob 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<BlobReference> DownloadBlobReferenceAsync(DownloadBlobReference command);

        /// <summary>
        /// Download blob
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<Blob> DownloadBlobAsync(DownloadBlob command);
        
        /// <summary>
        /// Delete blob from Azure Storage container
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task DeleteBlobAsync(DeleteBlob command);

        /// <summary>
        /// Enumerate and paginate blobs inside an Azure Storage container
        /// </summary>
        /// <returns></returns>
        Task<BlobReferenceCollection> EnumerateBlobsAsync(EnumerateBlobs command);
    }
}