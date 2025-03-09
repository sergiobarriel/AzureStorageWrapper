using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Exceptions;
using Xunit;

namespace AzureStorageWrapper.Tests.Should.Upload
{
    public class UploadBase64Should : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;

        public UploadBase64Should(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }
        
        [Fact]
        public async Task UploadBase64_Should_UploadBlob()
        {
            // Arrange
            
            var base64 = "SGVsbG8g8J+Zgg==";

            var command = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = "hello",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                    {{"hello", "world"}}
            };

            // Act
            
            var response = await _azureStorageWrapper.UploadBlobAsync(command);

            // Assert
            
            Assert.NotNull(response);
            Assert.True(await PingAsync(response.SasUri));
        }

        
        [Fact]
        public async Task UploadEmptyBase64_Should_ThrowException()
        {
            // Arrange
            
            var base64 = string.Empty;

            var command = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = "hello",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                    {{"hello", "world"}}
            };

            // Act and Assert
            
            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.UploadBlobAsync(command);
            });
        }

        [Theory]
        [MemberData(nameof(WrongUploadBlobCommandProperties))]
        public async Task UploadBase64Blob_WithWrongFileProperties_Should_ThrowException(string container, string fileName, string fileExtension)
        {
            // Arrange
            
            var base64 = "SGVsbG8g8J+Zgg==";

            var command = new UploadBase64()
            {
                Base64 = base64,
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

        [Fact]
        public async Task UploadBase64_WithMultipleDotsInName_Should_UploadBlob()
        {
            // Arrange
            
            var base64 = "SGVsbG8g8J+Zgg==";

            var command = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = "hello.hello.hello.hello",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                    {{"hello", "world"}}
            };

            // Act
            
            var response = await _azureStorageWrapper.UploadBlobAsync(command);

            // Assert
            
            Assert.NotNull(response);
            Assert.True(await PingAsync(response.SasUri));
        }
        
        [Fact]
        public async Task UploadBlob_WithBlankSpacesInName_Should_UploadBlob()
        {
            // Arrange
            
            var base64 = "SGVsbG8g8J+Zgg==";

            var command = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = "hello world",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                    {{"hello", "world"}}
            };

            // Act
            
            var response = await _azureStorageWrapper.UploadBlobAsync(command);

            // Assert
            
            Assert.NotNull(response);
            Assert.True(await PingAsync(response.SasUri));
        }

    }
}