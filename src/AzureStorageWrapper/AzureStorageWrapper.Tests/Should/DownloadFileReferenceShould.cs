using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Tests.Sources;
using Xunit;

namespace AzureStorageWrapper.Tests.Should
{
    public class DownloadFileReferenceShould : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;

        public DownloadFileReferenceShould(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }

        [Fact]
        public async Task DownloadBlobReferenceWithManyDots_ShouldReturnReference()
        {
            var command = new DownloadBlobReference()
            {
                Uri = Uris.ExistingFileWithManyDots,
                ExpiresIn = 360,
            };

            var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));
        }

        [Fact]
        public async Task DownloadBlobReferenceWithExtensions_ShouldReturnReference()
        {
            var command = new DownloadBlobReference()
            {
                Uri = Uris.ExistingFileWithManyExtensions,
                ExpiresIn = 360,
            };

            var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));
        }

        [Fact]
        public async Task DownloadBlobReference_ShouldReturnReference()
        {
            var command = new DownloadBlobReference()
            {
                Uri = Uris.ExistingFile,
                ExpiresIn = 360,
            };

            var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));
        }

        [Fact]
        public async Task DownloadBlobReference_WithWrongUri_ShouldThrowException()
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
        public async Task DownloadBlobReference_WithUnExistingUri_ShouldThrowException()
        {
            var command = new DownloadBlobReference()
            {
                Uri = Uris.UnExistingFile,
                ExpiresIn = 360,
            };


            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);
            });
        }


        [Theory]
        [MemberData(nameof(WrongExpiresIn))]
        public async Task DownloadBlobReference_WithWrongExpiration_ShouldReturnReference(int expiresIn)
        {
            var command = new DownloadBlobReference()
            {
                Uri = Uris.ExistingFile,
                ExpiresIn = expiresIn,
            };

            var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));
        }

        [Fact]
        public async Task DownloadBlobReference_WithHighExpiration_ShouldThrowException()
        {
            var command = new DownloadBlobReference()
            {
                Uri = Uris.ExistingFile,
                ExpiresIn = int.MaxValue,
            };

            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);
            });
        }
        
    }
}
