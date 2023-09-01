using AzureStorageWrapper;
using AzureStorageWrapper.Models;
using Xunit;

namespace AzureStorageWrapper.Tests.Should
{
    public class UploadBase64Should : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;

        public UploadBase64Should(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }

        [Fact]
        public async Task UploadEmptyBase64_ShouldThrowException()
        {
            var base64 = string.Empty;

            var command = new UploadBase64()
            {
                Base64 = base64,
                Container = "tests",
                Name = "hello",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                    {{"tests", "tests"}}
            };

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                _ = await _azureStorageWrapper.UploadBlobAsync(command);
            });
        }

        [Theory]
        [MemberData(nameof(WrongFileProperties))]
        public async Task UploadBase64_WithWrongFileProperties_ShouldThrowException(string container, string fileName, string fileExtension)
        {
            var base64 = "SGVsbG8gd29ybGQh";

            var command = new UploadBase64()
            {
                Base64 = base64,
                Container = container,
                Name = fileName,
                Extension = fileExtension,
                Metadata = new Dictionary<string, string>()
                    {{"tests", "tests"}}
            };

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                _ = await _azureStorageWrapper.UploadBlobAsync(command);
            });
        }

        [Theory]
        [MemberData(nameof(WrongMetadata))]
        public async Task UploadBase64File_WithWrongMetadata_ShouldUploadFile(Dictionary<string, string> properties)
        {
            var base64 = "SGVsbG8gd29ybGQh";

            var command = new UploadBase64()
            {
                Base64 = base64,
                Container = "tests",
                Name = "hello",
                Extension = "md",
                Metadata = properties
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(command);

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));
        }


        [Fact]
        public async Task UploadBase64_ShouldUploadFile()
        {
            var base64 = "SGVsbG8gd29ybGQh";

            var command = new UploadBase64()
            {
                Base64 = base64,
                Container = "tests",
                Name = "hello",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                    {{"tests", "tests"}}
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(command);

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));
        }
    }
}