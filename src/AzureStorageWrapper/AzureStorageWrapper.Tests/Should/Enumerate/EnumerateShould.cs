using AzureStorageWrapper.Commands;
using Xunit;
using Xunit.Abstractions;

namespace AzureStorageWrapper.Tests.Should.Enumerate
{
    public class EnumerateShould : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;
        private readonly ITestOutputHelper _output;

        public EnumerateShould(IAzureStorageWrapper azureStorageWrapper, ITestOutputHelper output)
        {
            _azureStorageWrapper = azureStorageWrapper;
            _output = output;
        }
        
        [Fact]
        public async Task EnumerateBlobs_ShouldReturnAllBlobsFromAContainer()
        {
            var amount = 10;
            
            await UploadFilesAsync(amount);
            
            var command = new EnumerateAllBlobs()
            {
                Container = ContainerWhereUploadFilesAndEnumerateThem
            };
        
            var references = await _azureStorageWrapper.EnumerateAllBlobsAsync(command);
            
            _output.WriteLine($"Enumerating {references.References.Count()} references");
            
            Assert.True(references.References.Any());
            Assert.True(references.References.Count() == amount);

            await RemoveFilesAsync(references.References.Select(reference => reference.Uri).ToArray());
        }
        
        [Fact]
        public async Task EnumerateBlobs_WithContinuationToken_ShouldReturnBlobsPageByPage()
        {
            var amount = 20;
            var size = 10;
            
            await UploadFilesAsync(amount);
            
            var firstIterationReferences = await _azureStorageWrapper.EnumerateBlobsAsync(new EnumerateBlobs()
            {
                Container = ContainerWhereUploadFilesAndEnumerateThem,
                Size = size,
            });
            
            _output.WriteLine($"Enumerating {firstIterationReferences.References.Count()} references");
            
            Assert.True(firstIterationReferences.References.Any());
            
            var secondIterationReferences = await _azureStorageWrapper.EnumerateBlobsAsync(new EnumerateBlobs()
            {
                Container = ContainerWhereUploadFilesAndEnumerateThem,
                Size = size,
                ContinuationToken = firstIterationReferences.ContinuationToken
            });

            _output.WriteLine($"Enumerating {secondIterationReferences.References.Count()} references");

            Assert.True(secondIterationReferences.References.Any());
            
            await RemoveFilesAsync(firstIterationReferences.References.Select(reference => reference.Uri).ToArray());
            await RemoveFilesAsync(secondIterationReferences.References.Select(reference => reference.Uri).ToArray());
        }

        private async Task UploadFilesAsync(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                await _azureStorageWrapper.UploadBlobAsync(new UploadBase64()
                {
                    Base64 =  "SGVsbG8g8J+Zgg==",
                    Container = ContainerWhereUploadFilesAndEnumerateThem,
                    Name = "hello",
                    Extension = "md",
                    Metadata = new Dictionary<string, string>()
                        {{"hello", "world"}}
                });
            }
        }

        private async Task RemoveFilesAsync(string[] uris)
        {
            foreach (var uri in uris)
            {
                await _azureStorageWrapper.DeleteBlobAsync(new DeleteBlob()
                {
                    Uri = uri
                });
            }
        }
    }
}