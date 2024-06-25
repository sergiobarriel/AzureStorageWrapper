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
        
        // [Fact]
        // public async Task EnumerateBlobs_ShouldReturnAllBlobsFromAContainer()
        // {
        //     var command = new EnumerateAllBlobs()
        //     {
        //         Container = "files"
        //     };
        //
        //     var references = await _azureStorageWrapper.EnumerateAllBlobsAsync(command);
        //     
        //     _output.WriteLine($"Enumerating {references.References.Count()} references");
        //     
        //     Assert.True(references.References.Any());
        //     
        // }
        
        [Fact]
        public async Task EnumerateBlobs_WithContinuationToken_ShouldReturnBlobsPageByPage()
        {
            var firstIterationReferences = await _azureStorageWrapper.EnumerateBlobsAsync(new EnumerateBlobs()
            {
                Container = "files",
                Size = 10,
            });
            
            _output.WriteLine($"Enumerating {firstIterationReferences.References.Count()} references");
            
            Assert.True(firstIterationReferences.References.Any());
            
            var secondIterationReferences = await _azureStorageWrapper.EnumerateBlobsAsync(new EnumerateBlobs()
            {
                Container = "files",
                Size = 10,
                ContinuationToken = firstIterationReferences.ContinuationToken
            });

            _output.WriteLine($"Enumerating {secondIterationReferences.References.Count()} references");

            Assert.True(secondIterationReferences.References.Any());
        }
    }
}