using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Tests.Sources;
using Xunit;

namespace AzureStorageWrapper.Tests.Should
{
    public class DownloadFileUriVariations : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;

        public DownloadFileUriVariations(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }


        [Fact]
        public async Task DownloadFileWithEncodedFileName_ShouldDownloadFileReference()
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
        public async Task DownloadFileWithBlanksFileName_ShouldDownloadFileReference()
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