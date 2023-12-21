using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Exceptions;
using Xunit;

namespace AzureStorageWrapper.Tests.Should.Upload
{
    public class UploadBytesShould : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;

        public UploadBytesShould(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }

        [Fact]
        public async Task UploadEmptyBytes_Should_ThrowException()
        {
            var bytes = Convert.FromBase64String(string.Empty);

            var command = new UploadBytes()
            {
                Bytes = bytes,
                Container = "files",
                Name = "hello",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                    {{"hello", "world"}}
            };

            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.UploadBlobAsync(command);
            });
        }

        [Theory]
        [MemberData(nameof(WrongFileProperties))]
        public async Task UploadBytesBlob_WithWrongFileProperties_Should_ThrowException(string container, string fileName, string fileExtension)
        {
            var bytes = Convert.FromBase64String("SGVsbG8g8J+Zgg==");

            var command = new UploadBytes()
            {
                Bytes = bytes,
                Container = container,
                Name = fileName,
                Extension = fileExtension,
                Metadata = new Dictionary<string, string>()
                    {{"hello", "world"}}
            };

            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.UploadBlobAsync(command);
            });
        }

        [Theory]
        [MemberData(nameof(WrongMetadata))]
        public async Task UploadBytesBlob_WithWrongMetadata_Should_UploadBlob(Dictionary<string, string> properties)
        {
            var bytes = Convert.FromBase64String("SGVsbG8g8J+Zgg==");

            var command = new UploadBytes()
            {
                Bytes = bytes,
                Container = "files",
                Name = "hello",
                Extension = "md",
                Metadata = properties
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(command);

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));
        }

        [Fact]
        public async Task UploadBytesBlob_Should_UploadBlob()
        {
            var bytes = Convert.FromBase64String("SGVsbG8g8J+Zgg==");

            var response = await _azureStorageWrapper.UploadBlobAsync(new UploadBytes()
            {
                Bytes = bytes,
                Container = "files",
                Name = "hello",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                    {{"hello", "world"}}
            });

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));
        }
    }
}