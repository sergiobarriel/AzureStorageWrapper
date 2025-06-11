using System.Drawing;
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
            await EnumerateWithoutPaginationByCommandAsync();
            await EnumerateWithPaginationWithoutContinationTokenAsync();
            await EnumerateWithPaginationWithoutContinationTokenByCommandAsync();
            await EnumerateWithPaginationWithContinationTokenByCommandAsync();
        }

        public async Task EnumerateWithoutPaginationAsync()
        {
            ConsoleHelper.Start("Enumerate Blobs without Pagination");
            var container = "files";

            //var response = await _azureStorageWrapper.EnumerateBlobsAsync(); //You will need to set the DefaultContainer option.
            var response = await _azureStorageWrapper.EnumerateBlobsAsync(container);

            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Enumerate Blobs without Pagination");
        }
        public async Task EnumerateWithoutPaginationByCommandAsync()
        {
            ConsoleHelper.Start("Enumerate Blobs without Pagination by Command");
            var query = new EnumerateBlobs()
            {
                Container = "files",
                Paginate = false
            };

            var response = await _azureStorageWrapper.EnumerateBlobsAsync(query);
            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Enumerate Blobs without Pagination by Command");
        }

        public async Task EnumerateWithPaginationWithoutContinationTokenAsync()
        {
            ConsoleHelper.Start("Enumerate Blobs with Pagination and without ContinationToken");
            var container = "files";
            var size = 10;

            //var response = await _azureStorageWrapper.EnumerateBlobsAsync(size); //You will need to set the DefaultContainer option.
            var response = await _azureStorageWrapper.EnumerateBlobsAsync(size, container);

            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Enumerate Blobs with Pagination and without ContinationToken");
        }
        public async Task EnumerateWithPaginationWithoutContinationTokenByCommandAsync()
        {
            ConsoleHelper.Start("Enumerate Blobs with Pagination and without ContinationToken by Command");
            var query = new EnumerateBlobs()
            {
                Container = "files",
                Paginate = true,
                Size = 10,
            };

            var response = await _azureStorageWrapper.EnumerateBlobsAsync(query);
            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Enumerate Blobs with Pagination and without ContinationToken by Command");
        }

        public async Task EnumerateWithPaginationWithContinationTokenByCommandAsync()
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
