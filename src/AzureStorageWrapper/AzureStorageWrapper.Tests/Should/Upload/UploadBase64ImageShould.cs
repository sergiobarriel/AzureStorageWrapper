using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Tests.Sources;
using Xunit;

namespace AzureStorageWrapper.Tests.Should
{
    public class UploadBase64ImageShould : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;

        public UploadBase64ImageShould(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }

        [Fact]
        public async Task UploadBase64Image_WithoutHeader_Should_UploadBlob()
        {
            var command = new UploadBase64()
            {
                Base64 = Images.ImageWithoutEmbeddedTag,
                Container = "files",
                Name = "icon",
                Extension = "png",
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(command);

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));
        }


        /// <summary>
        /// data:image/png;base64,iVBO....
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UploadBase64_Image_WithHeader_Should_UploadBlob()
        {
            var command = new UploadBase64()
            {
                Base64 = Images.ImageWithEmbeddedTag, 
                Container = "files",
                Name = "icon",
                Extension = "png",
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(command);

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));
        }


    }
}