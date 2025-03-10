using AzureStorageWrapper.Commands;
using AzureStorageWrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using samples.Helpers;

namespace samples
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
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

            return new OkObjectResult("FIN");
        }
    }
}
