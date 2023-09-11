using AzureStorageWrapper.Commands;
using Xunit;

namespace AzureStorageWrapper.Tests.Should
{
    public class DeleteFileShould : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;

        public DeleteFileShould(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }

        [Fact]
        public async Task DeleteFile_ShouldDeleteFile()
        {
            var base64 = "SGVsbG8g8J+Zgg==";

            var uploadCommand = new UploadBase64()
            {
                Base64 = base64,
                Container = "greetings",
                Name = "greeting",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                    {{"GREETING_PLACE", "Office"}}
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(uploadCommand);

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));

            var deleteCommand = new DeleteBlob()
            {
                Uri = response.Uri
            };

            await _azureStorageWrapper.DeleteBlobAsync(deleteCommand);

            Assert.False(await PingAsync(response.SasUri));

        }
    }
}