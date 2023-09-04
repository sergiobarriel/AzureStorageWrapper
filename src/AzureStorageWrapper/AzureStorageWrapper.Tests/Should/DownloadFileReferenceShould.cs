using AzureStorageWrapper.Commands;
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
        public async Task DownloadBlobReference_ShouldReturnReference()
        {
            var command = new DownloadBlobReference()
            {
                Container = "greetings",
                Folder = "23ca3bafd024499",
                Name = "greeting",
                Extension = "md",
                ExpiresIn = 360,
            };

            var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));
        }


        [Theory]
        [MemberData(nameof(WrongExpiresIn))]
        public async Task DownloadBlobReference_WithWrongExpiration_ShouldReturnReference(int expiresIn)
        {
            var command = new DownloadBlobReference()
            {
                Container = "greetings",
                Folder = "23ca3bafd024499",
                Name = "greeting",
                Extension = "md",
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
                Container = "greetings",
                Folder = "23ca3bafd024499",
                Name = "greeting",
                Extension = "md",
                ExpiresIn = int.MaxValue,
            };

            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);
            });
        }
        
        [Fact]
        public async Task DownloadBlobReference_WithoutContainer_ShouldThrowException()
        {
            var command = new DownloadBlobReference()
            {
                Folder = "23ca3bafd024499",
                Name = "greeting",
                Extension = "md",
                ExpiresIn = 360,
            };

            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);
            });
        }

        [Fact]
        public async Task DownloadBlobReference_WithoutName_ShouldThrowException()
        {
            var command = new DownloadBlobReference()
            {
                Container = "greetings",
                Folder = "23ca3bafd024499",
                Extension = "md",
                ExpiresIn = 360,
            };

            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);
            });
        }

        [Fact]
        public async Task DownloadBlobReference_WithoutExtension_ShouldThrowException()
        {
            var command = new DownloadBlobReference()
            {
                Container = "greetings",
                Folder = "23ca3bafd024499",
                Name = "greeting",
                ExpiresIn = 360,
            };

            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);
            });
        }
    }
}
