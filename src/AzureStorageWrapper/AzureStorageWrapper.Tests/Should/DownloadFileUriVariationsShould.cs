using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Tests.Sources;
using Xunit;

namespace AzureStorageWrapper.Tests.Should
{
    public class DownloadFileUriVariationsShould : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;

        public DownloadFileUriVariationsShould(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }


        [Fact]
        public async Task DownloadFile_WithEncodedFileName_ShouldDownloadFileReference()
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
        public async Task DownloadFile_WithBlanksFileName_ShouldDownloadFileReference()
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