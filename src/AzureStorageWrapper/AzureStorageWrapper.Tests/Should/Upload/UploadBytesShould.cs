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
        public async Task UploadBytesBlob_Should_UploadBlob()
        {
            // Arrange
            
            var bytes = Convert.FromBase64String("SGVsbG8g8J+Zgg==");

            var command = new UploadBytes()
            {
                Bytes = bytes,
                Container = "files",
                Name = "hello",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                    { { "hello", "world" } }
            };
            
            // Act
            
            var response = await _azureStorageWrapper.UploadBlobAsync(command);

            // Assert
            
            Assert.NotNull(response);
            Assert.True(await PingAsync(response.SasUri));
        }
        
        [Fact]
        public async Task UploadEmptyBytes_Should_ThrowException()
        {
            // Arrange
            
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

            // Act & Assert
            
            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.UploadBlobAsync(command);
            });
        }

        [Theory]
        [MemberData(nameof(WrongUploadBlobCommandProperties))]
        public async Task UploadBytesBlob_WithWrongFileProperties_Should_ThrowException(string container, string fileName, string fileExtension)
        {
            // Arrange
            
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

            // Act & Assert
            
            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.UploadBlobAsync(command);
            });
        }

    }
}