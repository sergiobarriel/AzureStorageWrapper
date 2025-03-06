using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Exceptions;
using AzureStorageWrapper.Tests.Sources;
using Xunit;
 
namespace AzureStorageWrapper.Tests.Should.Download
{
    public class DownloadBlobReferenceShould : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;

        public DownloadBlobReferenceShould(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }

        [Fact]
        public async Task DownloadBlobReference_WithManyDots_Should_ReturnReference()
        {
            var base64 = "SGVsbG8g8J+Zgg==";
            
            var uploadBlobCommand = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = "hello.world.hello.world",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                    {{"hello", "world"}}
            };

            var uploadBlobResponse = await _azureStorageWrapper.UploadBlobAsync(uploadBlobCommand);
                
            var downloadBlobReferenceCommand = new DownloadBlobReference()
            {
                Uri = uploadBlobResponse.Uri,
                ExpiresIn = 360,
            };

            // Act
            var blobReference = await _azureStorageWrapper.DownloadBlobReferenceAsync(downloadBlobReferenceCommand);

            // Assert
            Assert.NotNull(blobReference);
            Assert.True(await PingAsync(blobReference.SasUri));
        }

        [Fact]
        public async Task DownloadBlobReference_WithExtensions_Should_ReturnReference()
        {
            var base64 = "SGVsbG8g8J+Zgg==";
            
            var uploadBlobCommand = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = "hello",
                Extension = "md.md.md",
                Metadata = new Dictionary<string, string>()
                    {{"hello", "world"}}
            };

            var uploadBlobResponse = await _azureStorageWrapper.UploadBlobAsync(uploadBlobCommand);
                
            var downloadBlobReferenceCommand = new DownloadBlobReference()
            {
                Uri = uploadBlobResponse.Uri,
                ExpiresIn = 360,
            };

            // Act
            var blobReference = await _azureStorageWrapper.DownloadBlobReferenceAsync(downloadBlobReferenceCommand);

            // Assert
            Assert.NotNull(blobReference);
            Assert.True(await PingAsync(blobReference.SasUri));
        }

        [Fact]
        public async Task DownloadBlobReference_Should_ReturnReference()
        {
            // Arrange
            
            var base64 = "SGVsbG8g8J+Zgg==";
            
            var uploadBlobCommand = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = "hello",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                    {{"hello", "world"}}
            };

            var uploadBlobResponse = await _azureStorageWrapper.UploadBlobAsync(uploadBlobCommand);
                
            var downloadBlobReferenceCommand = new DownloadBlobReference()
            {
                Uri = uploadBlobResponse.Uri,
                ExpiresIn = 360,
            };

            // Act
            var blobReference = await _azureStorageWrapper.DownloadBlobReferenceAsync(downloadBlobReferenceCommand);

            // Assert
            Assert.NotNull(blobReference);
            Assert.True(await PingAsync(blobReference.SasUri));
        }

        [Fact]
        public async Task DownloadBlobReference_WithWrongUri_Should_ThrowException()
        {
            var command = new DownloadBlobReference()
            {
                Uri = string.Empty,
                ExpiresIn = 360,
            };


            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);
            });
        }


        [Fact]
        public async Task DownloadBlobReference_WithUnExistingUri_Should_ThrowException()
        {
            var command = new DownloadBlobReference()
            {
                Uri = "",
                ExpiresIn = 360,
            };


            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);
            });
        }
        
        
    }
}