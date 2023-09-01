using System.Threading.Tasks;
using AzureStorageWrapper.Responses;
using AzureStorageWrapper.Models;

namespace AzureStorageWrapper
{
    public interface IAzureStorageWrapper
    {
        Task<BlobReference> UploadBlobAsync(UploadBlob command);
        Task<BlobReference> DownloadBlobReferenceAsync(DownloadBlobReference command);
    }
}