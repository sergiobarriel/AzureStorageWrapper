using System.Threading.Tasks;
using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Responses;

namespace AzureStorageWrapper
{
    public interface IAzureStorageWrapper
    {
        Task<BlobReference> UploadBlobAsync(UploadBlob command);
        Task<BlobReference> DownloadBlobReferenceAsync(DownloadBlobReference command);
    }
}