﻿using AzureStorageWrapper.Commands;
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
        public async Task DownloadBlobReference_WithExtensions_Should_ReturnReference()
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
        public async Task DownloadBlobReference_Should_ReturnReference()
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
                Uri = Uris.UnExistingFile,
                ExpiresIn = 360,
            };


            await Assert.ThrowsAsync<AzureStorageWrapperException>(async () =>
            {
                _ = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);
            });
        }


        [Theory]
        [MemberData(nameof(InvalidExpiresIn))]
        public async Task DownloadBlobReference_WithWrongExpiration_Should_ReturnReference(int expiresIn)
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
        public async Task DownloadBlobReference_WithHighExpiration_Should_ThrowException()
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