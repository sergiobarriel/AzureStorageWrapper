using AzureStorageWrapper;
using AzureStorageWrapper.Models;
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
                Container = "tests",
                Name = "011fe8ba50ce4f9",
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
                Container = "tests",
                Name = "011fe8ba50ce4f9",
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
                Container = "tests",
                Name = "011fe8ba50ce4f9",
                Extension = "md",
                ExpiresIn = int.MaxValue,
            };

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                _ = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);
            });
        }
    }
}
