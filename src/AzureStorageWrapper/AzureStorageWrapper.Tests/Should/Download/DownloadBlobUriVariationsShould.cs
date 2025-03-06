using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Tests.Sources;
using Xunit;

namespace AzureStorageWrapper.Tests.Should.Download
{
    public class DownloadBlobUriVariationsShould : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;

        public DownloadBlobUriVariationsShould(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }


        // [Fact]
        // public async Task DownloadBlob_WithEncodedFileName_Should_ReturnFileReference()
        // {
        //     // Arrange
        //     
        //     var base64 = "SGVsbG8g8J+Zgg==";
        //     
        //     var uploadBlobCommand = new UploadBase64()
        //     {
        //         Base64 = base64,
        //         Container = "files",
        //         Name = "hello world",
        //         Extension = "md",
        //         Metadata = new Dictionary<string, string>()
        //             {{"hello", "world"}}
        //     };
        //
        //     var uploadBlobResponse = await _azureStorageWrapper.UploadBlobAsync(uploadBlobCommand);
        //         
        //     var downloadBlobReferenceCommand = new DownloadBlobReference()
        //     {
        //         Uri = uploadBlobResponse.Uri,
        //         ExpiresIn = 360,
        //     };
        //
        //     var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);
        //     
        //     Assert.True(await PingAsync(response.SasUri));
        // }

        [Fact]
        public async Task DownloadBlob_WithBlanksFileName_Should_ReturnFileReference()
        {
            // Arrange
            
            var base64 = "SGVsbG8g8J+Zgg==";
            
            var uploadBlobCommand = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = "hello world",
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
            
            var command = new DownloadBlobReference()
            {
                Uri = uploadBlobResponse.Uri,
                ExpiresIn = 180
            };

            // Act
            
            var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);

            // Assert
            
            Assert.True(await PingAsync(response.SasUri));
        }

    }
}