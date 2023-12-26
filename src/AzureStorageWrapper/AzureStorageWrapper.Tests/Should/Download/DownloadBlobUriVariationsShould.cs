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


        [Fact]
        public async Task DownloadBlob_WithEncodedFileName_Should_ReturnFileReference()
        {
            var command = new DownloadBlobReference()
            {
                Uri = Uris.ExistingFileWithUriEncoded,
                ExpiresIn = 180
            };

            var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);
            
            Assert.True(await PingAsync(response.SasUri));
        }

        [Fact]
        public async Task DownloadBlob_WithBlanksFileName_Should_ReturnFileReference()
        {
            var command = new DownloadBlobReference()
            {
                Uri = Uris.ExistingFileWithBlanks,
                ExpiresIn = 180
            };

            var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);

            Assert.True(await PingAsync(response.SasUri));
        }

    }
}