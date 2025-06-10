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
        public async Task UploadStreamBlob_Should_UploadBlob()
        {
            // Arrange
            
            var stream = new MemoryStream(Convert.FromBase64String("SGVsbG8g8J+Zgg=="));

            var command = new UploadStream()
            {
                Stream = stream,
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
        public async Task UploadEmptyStream_ShouldThrowException()
        {
            // Arrange
            
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

            // Act & Assert
            
            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.UploadBlobAsync(command);
            });
        }

        [Theory]
        [MemberData(nameof(WrongUploadBlobCommandProperties))]
        public async Task UploadStreamBlob_WithWrongFileProperties_Should_ThrowException(string container, string fileName, string fileExtension)
        {
            // Arrange
            
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

            // Act & Assert
            
            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.UploadBlobAsync(command);
            });
        }
        

 
    }
}