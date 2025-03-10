using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorageWrapper;
using AzureStorageWrapper.Commands;
using samples.Helpers;

namespace samples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AzuriteHelper.Start();// Starts the Azurite process by executing the run-azurite.bat file
            var options = new AzureStorageWrapperOptions
            {
                ConnectionString = "UseDevelopmentStorage=true",
                MaxSasUriExpiration = 600,
                DefaultSasUriExpiration = 300,
                CreateContainerIfNotExists = true,
            };
            var azureStorageWrapper = new AzureStorageWrapper.AzureStorageWrapper(options);


            #region Upload Blobs In Bytes
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

            var response = await azureStorageWrapper.UploadBlobAsync(command);
            ConsoleHelper.Result(response);
            ConsoleHelper.Finalized("Upload in Base64");
            #endregion

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
