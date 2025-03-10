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
            await DownloadBlobReferencesAsync();
        }

        public async Task DownloadBlobReferencesAsync()
        {
            var uri = await UploadFileAsynt();
            ConsoleHelper.Start("Download Blob References");
            var query = new DownloadBlobReference()
            {
                Uri = uri,
                ExpiresIn = 60
            };

            var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(query); ConsoleHelper.Result(response);
            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Download Blob References");
        }


        private async Task<string> UploadFileAsynt()
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
