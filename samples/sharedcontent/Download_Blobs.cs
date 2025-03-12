using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorageWrapper;
using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Queries;
using AzureStorageWrapper.Responses;
using samples.Helpers;

namespace samples
{
    public class Download_Blobs
    {
        readonly IAzureStorageWrapper _azureStorageWrapper;
        public Download_Blobs(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }
        public async Task RunAllAsync()
        {
            ConsoleHelper.Module("****  DOWNLOAD BLOBS ****");
            await DownloadBlobReferencesByUriAsync();
            await DownloadBlobReferencesByCommandAsync();
            await DownloadBlobByUriAsync();
            await DownloadBlobByCommandAsync();
        }

        public async Task DownloadBlobReferencesByUriAsync()
        {
            var uri = await UploadFileAsync();
            ConsoleHelper.Start("Download Blob References by Uri");

            var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(uri);

            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Download Blob References by Uri");
        }
        public async Task DownloadBlobReferencesByCommandAsync()
        {
            var uri = await UploadFileAsync();
            ConsoleHelper.Start("Download Blob References by Command");
            var query = new DownloadBlobReference()
            {
                Uri = uri,
                ExpiresIn = 60
            };

            var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(query); ConsoleHelper.Result(response);
            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Download Blob References by Command");
        }

        public async Task DownloadBlobByUriAsync()
        {
            var uri = await UploadFileAsync();
            ConsoleHelper.Start("Download Blob by Uri");

            var response = await _azureStorageWrapper.DownloadBlobAsync(uri);

            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Download Blob by Uri");
        }
        public async Task DownloadBlobByCommandAsync()
        {
            var uri = await UploadFileAsync();
            ConsoleHelper.Start("Download Blob by Command");
            var query = new DownloadBlob()
            {
                Uri = uri,
            };

            var response = await _azureStorageWrapper.DownloadBlobAsync(query);

            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Download Blob by Command");
        }


        private async Task<string> UploadFileAsync()
        {
            var base64 = "SGVsbG8g8J+Zgg==";

            var uploadBlobCommand = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = "hello",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                    {{"hello", "world"}}
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(uploadBlobCommand);
            return response.Uri;
        }
    }
}
