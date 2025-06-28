using System.Threading.Tasks;
using AzureStorageWrapper;
using AzureStorageWrapper.Queries;
using samples.Helpers;

namespace samples
{
    public class Enumerate_Blobs
    {
        readonly IAzureStorageWrapper _azureStorageWrapper;
        public Enumerate_Blobs(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }
        public async Task RunAllAsync()
        {
            ConsoleHelper.Module("****  ENUMERATE BLOBS ****");
            await EnumerateWithoutPaginationAsync();
            await EnumerateWithPaginationWithoutContinationTokenAsync();
            await EnumerateWithPaginationWithContinationTokenAsync();
        }

        public async Task EnumerateWithoutPaginationAsync()
        {
            ConsoleHelper.Start("Enumerate Blobs without Pagination");
            var query = new EnumerateBlobs()
            {
                Container = "files",
                Paginate = false
            };

            var response = await _azureStorageWrapper.EnumerateBlobsAsync(query);
            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Enumerate Blobs without Pagination");
        }

        public async Task EnumerateWithPaginationWithoutContinationTokenAsync()
        {
            ConsoleHelper.Start("Enumerate Blobs with Pagination and without ContinationToken");
            var query = new EnumerateBlobs()
            {
                Container = "files",
                Paginate = true,
                Size = 10,
            };

            var response = await _azureStorageWrapper.EnumerateBlobsAsync(query);
            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Enumerate Blobs with Pagination and without ContinationToken");
        }

        public async Task EnumerateWithPaginationWithContinationTokenAsync()
        {
            ConsoleHelper.Start("Enumerate Blobs with Pagination and with ContinationToken");
            var firstQuery = new EnumerateBlobs()
            {
                Container = "files",
                Paginate = true,
                Size = 10,
            };

            var firstResponse = await _azureStorageWrapper.EnumerateBlobsAsync(firstQuery);
            ConsoleHelper.Result(firstResponse);

            var secondQuery = new EnumerateBlobs()
            {
                Container = "files",
                Paginate = true,
                Size = 10,
                ContinuationToken = firstResponse.ContinuationToken
            };

            var secondResponse = await _azureStorageWrapper.EnumerateBlobsAsync(secondQuery);
            ConsoleHelper.Result(secondResponse);
            ConsoleHelper.Finalized("Enumerate Blobs with Pagination and with ContinationToken");
        }
    }
}
