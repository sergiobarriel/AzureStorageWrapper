using System.Threading.Tasks;
using AzureStorageWrapper;
using AzureStorageWrapper.Commands;
using samples.Helpers;

namespace samples
{
    public class Delete_Blobs
    {
        readonly IAzureStorageWrapper _azureStorageWrapper;
        public Delete_Blobs(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }
        public async Task RunAllAsync()
        {
            ConsoleHelper.Module("****  DELETE BLOBS ****");
            await DeleteByUriAsync();
            await DeleteByCommandAsync();
        }

        public async Task DeleteByUriAsync()
        {
            ConsoleHelper.Start("Delete Blobs by Uri");
            var uri = "https://accountName.blob.core.windows.net/files/5a19306fc5014a4/hello.md";

            await _azureStorageWrapper.DeleteBlobAsync(uri);

            ConsoleHelper.Finalized("Download in Base64");
            ConsoleHelper.Finalized("Delete Blobs by Uri");
        }

        public async Task DeleteByCommandAsync()
        {
            ConsoleHelper.Start("Delete Blobs by Command");
            var command = new DeleteBlob()
            {
                Uri = "https://accountName.blob.core.windows.net/files/5a19306fc5014a4/hello.md"
            };

            await _azureStorageWrapper.DeleteBlobAsync(command);

            ConsoleHelper.Finalized("Download in Base64");
            ConsoleHelper.Finalized("Delete Blobs by Command");
        }
    }
}
