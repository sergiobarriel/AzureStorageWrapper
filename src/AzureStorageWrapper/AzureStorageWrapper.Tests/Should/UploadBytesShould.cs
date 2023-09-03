using AzureStorageWrapper;
using AzureStorageWrapper.Commands;
using Xunit;

namespace AzureStorageWrapper.Tests.Should
{
    public class UploadBytesShould : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;

        public UploadBytesShould(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }

        [Fact]
        public async Task UploadEmptyBytes_ShouldThrowException()
        {
            var bytes = Convert.FromBase64String(string.Empty);

            var command = new UploadBytes()
            {
                Bytes = bytes,
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
        public async Task UploadBytesFile_WithWrongFileProperties_ShouldThrowException(string container, string fileName, string fileExtension)
        {
            var bytes = Convert.FromBase64String("SGVsbG8gd29ybGQh");

            var command = new UploadBytes()
            {
                Bytes = bytes,
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
        public async Task UploadBytesFile_WithWrongMetadata_ShouldUploadFile(Dictionary<string, string> properties)
        {
            var bytes = Convert.FromBase64String("SGVsbG8gd29ybGQh");

            var command = new UploadBytes()
            {
                Bytes = bytes,
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
        public async Task UploadBytes_ShouldUploadFile()
        {
            var bytes = Convert.FromBase64String("SGVsbG8gd29ybGQh");

            var response = await _azureStorageWrapper.UploadBlobAsync(new UploadBytes()
            {
                Bytes = bytes,
                Container = "tests",
                Name = "hello",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                    {{"tests", "tests"}}
            });

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));
        }
    }
}