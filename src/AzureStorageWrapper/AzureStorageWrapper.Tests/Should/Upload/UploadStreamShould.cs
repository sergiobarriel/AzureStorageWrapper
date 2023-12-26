using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Exceptions;
using Xunit;

namespace AzureStorageWrapper.Tests.Should.Upload
{
    public class UploadStreamShould : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;

        public UploadStreamShould(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }

        [Fact]
        public async Task UploadEmptyStream_ShouldThrowException()
        {
            var stream = new MemoryStream(Convert.FromBase64String(string.Empty));

            var command = new UploadStream()
            {
                Stream = stream,
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
        [MemberData(nameof(InvalidFilePropertiesCombination))]
        public async Task UploadStreamBlob_WithWrongFileProperties_Should_ThrowException(string container, string fileName, string fileExtension)
        {
            var stream = new MemoryStream(Convert.FromBase64String("SGVsbG8g8J+Zgg=="));

            var command = new UploadStream()
            {
                Stream = stream,
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
        [MemberData(nameof(InvalidMetadata))]
        public async Task UploadStreamBlob_WithWrongMetadata_Should_UploadFile(Dictionary<string, string> properties)
        {
            var stream = new MemoryStream(Convert.FromBase64String("SGVsbG8g8J+Zgg=="));

            var command = new UploadStream()
            {
                Stream = stream,
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
        public async Task UploadStreamBlob_Should_UploadBlob()
        {
            var stream = new MemoryStream(Convert.FromBase64String("SGVsbG8g8J+Zgg=="));

            var response = await _azureStorageWrapper.UploadBlobAsync(new UploadStream()
            {
                Stream = stream,
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