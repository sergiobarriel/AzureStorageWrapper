using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Exceptions;
using AzureStorageWrapper.Tests.Sources;
using Xunit;

namespace AzureStorageWrapper.Tests.Should.Download
{
    public class DownloadBlobStreamShould : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;

        public DownloadBlobStreamShould(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }
        
        [Fact]
        public async Task DownloadBlob_Should_ReturnBlob()
        {
            var commandReference = new DownloadBlobReference()
            {
                Uri = Uris.ExistingFile,
                ExpiresIn = 60
            };

            var blobReference = await _azureStorageWrapper.DownloadBlobReferenceAsync(commandReference);
                
            var command = new DownloadBlob()
            {
                Uri = blobReference.SasUri,
            };

            var response = await _azureStorageWrapper.DownloadBlobAsync(command);

            Assert.NotNull(response);

            Assert.NotNull(response.Stream);
            Assert.True(response.Stream.Length > 0);
        }
        
        
        [Fact]
        public async Task DownloadBlob_WithInvalidUri_Should_ReturnBlob()
        {
            var commandReference = new DownloadBlobReference()
            {
                Uri = Uris.ExistingFile,
                ExpiresIn = 60
            };

            var blobReference = await _azureStorageWrapper.DownloadBlobReferenceAsync(commandReference);
                
            var command = new DownloadBlob()
            {
                Uri = blobReference.Uri,
            };
            
            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.DownloadBlobAsync(command);
            });
        }
    }
}