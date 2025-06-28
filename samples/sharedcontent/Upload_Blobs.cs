using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AzureStorageWrapper;
using AzureStorageWrapper.Commands;
using samples.Helpers;

namespace samples
{
    public class Upload_Blobs
    {
        readonly IAzureStorageWrapper _azureStorageWrapper;
        public Upload_Blobs(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }

        public async Task RunAllAsync()
        {
            ConsoleHelper.Module("****  UPLOAD BLOBS ****");
            await UploadInBase64Async();
            await UploadInBytesAsync();
            await UploadInStreamAsync();
        }

        public async Task UploadInBase64Async()
        {
            ConsoleHelper.Start("Upload in Base64");
            var base64 = "SGVsbG8g8J+Zgg==";

            var command = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = "hello",
                Extension = "md",
                Metadata = new Dictionary<string, string>() { { "key", "value" } }
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(command);
            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Upload in Base64");
        }

        public async Task UploadInBytesAsync()
        {
            ConsoleHelper.Start("Upload in Bytes");
            var bytes = Convert.FromBase64String("SGVsbG8g8J+Zgg==");

            var command = new UploadBytes()
            {
                Bytes = bytes,
                Container = "files",
                Name = "hello",
                Extension = "md",
                Metadata = new Dictionary<string, string>() { { "key", "value" } }
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(command);
            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Upload in Base64");
        }

        public async Task UploadInStreamAsync()
        {
            ConsoleHelper.Start("Upload in Bytes");
            var stream = new MemoryStream(Convert.FromBase64String("SGVsbG8g8J+Zgg=="));

            var command = new UploadStream()
            {
                Stream = stream,
                Container = "files",
                Name = "hello",
                Extension = "md",
                Metadata = new Dictionary<string, string>() { { "key", "value" } }
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(command);
            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Upload in Base64");
        }
    }
}
