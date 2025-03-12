using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using AzureStorageWrapper;
using AzureStorageWrapper.Commands;
using samples.Helpers;
using static System.Net.WebRequestMethods;

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
            await UploadByFileInBase64Async();
            await UploadByCommandInBase64Async();
            await UploadByFileInBytesAsync();
            await UploadByCommandInBytesAsync();
            await UploadByFileInStreamAsync();
            await UploadByCommandInStreamAsync();
        }

        public async Task UploadByFileInBase64Async()
        {
            ConsoleHelper.Start("Upload by file and content in Base64");
            var base64 = "SGVsbG8g8J+Zgg==";
            var container = "files";

            //var response = await _azureStorageWrapper.UploadBlobAsync("files.md", base64); //You will need to set the DefaultContainer option.
            var response = await _azureStorageWrapper.UploadBlobAsync("files.md", base64, container);

            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Upload by file and content in Base64");
        }

        public async Task UploadByCommandInBase64Async()
        {
            ConsoleHelper.Start("Upload by Command in Base64");
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

        public async Task UploadByFileInBytesAsync()
        {
            ConsoleHelper.Start("Upload by file and content in Bytes");
            var bytes = Convert.FromBase64String("SGVsbG8g8J+Zgg==");
            var container = "files";

            //var response = await _azureStorageWrapper.UploadBlobAsync("files.md", bytes); //You will need to set the DefaultContainer option.
            var response = await _azureStorageWrapper.UploadBlobAsync("files.md", bytes, container);

            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Upload by file and content in Bytes");
        }

        public async Task UploadByCommandInBytesAsync()
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
            ConsoleHelper.Finalized("Upload in Bytes");
        }

        public async Task UploadByFileInStreamAsync()
        {
            ConsoleHelper.Start("Upload by file and content in Stream");
            var stream = new MemoryStream(Convert.FromBase64String("SGVsbG8g8J+Zgg=="));
            var container = "files";

            //var response = await _azureStorageWrapper.UploadBlobAsync("files.md", bytes); //You will need to set the DefaultContainer option.
            var response = await _azureStorageWrapper.UploadBlobAsync("files.md", stream, container);

            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Upload by file and content in Stream");
        }

        public async Task UploadByCommandInStreamAsync()
        {
            ConsoleHelper.Start("Upload by Command in Stream");
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
            ConsoleHelper.Finalized("Upload by Command in Stream");
        }
    }
}
