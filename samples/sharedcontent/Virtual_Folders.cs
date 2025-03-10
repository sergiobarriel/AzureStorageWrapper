using System;
using System.Threading.Tasks;
using AzureStorageWrapper;
using AzureStorageWrapper.Commands;
using samples.Helpers;

namespace samples
{
    public class Virtual_Folders
    {
        readonly IAzureStorageWrapper _azureStorageWrapper;
        public Virtual_Folders(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }

        public async Task RunAllAsync()
        {
            ConsoleHelper.Module("**** VIRTUAL FOLDERS ****");
            await UploadWithVirtualFolderASync();
            await UploadWithoutVirtualFolderASync();
        }

        public async Task UploadWithVirtualFolderASync()
        {
            ConsoleHelper.Start("Upload With Virtual Folders");
            var base64 = "SGVsbG8g8J+Zgg==";

            var command = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = $"{Guid.NewGuid()}",
                Extension = "md",
                UseVirtualFolder = true// Default: True;
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(command);
            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Upload With Virtual Folders");
        }
        public async Task UploadWithoutVirtualFolderASync()
        {
            ConsoleHelper.Start("Upload Without Virtual Folders");
            var base64 = "SGVsbG8g8J+Zgg==";

            var command = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = $"{Guid.NewGuid()}",
                Extension = "md",
                UseVirtualFolder = false
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(command);
            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Upload Without Virtual Folders");
        }

    }
}
