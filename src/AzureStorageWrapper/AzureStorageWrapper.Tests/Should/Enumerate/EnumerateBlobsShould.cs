using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Queries;
using Xunit;
using Xunit.Abstractions;

namespace AzureStorageWrapper.Tests.Should.Enumerate
{
    public class EnumerateBlobsShould : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;
        private readonly ITestOutputHelper _output;

        public EnumerateBlobsShould(IAzureStorageWrapper azureStorageWrapper, ITestOutputHelper output)
        {
            _azureStorageWrapper = azureStorageWrapper;
            _output = output;
        }
        
        [Fact]
        public async Task EnumerateBlobs_WithoutPagination_Should_ReturnAllBlobsFromContainer()
        {
            var amount = 10;
            var container = "without-pagination";
            
            await UploadFilesAsync(amount, container);
            
            var query = new EnumerateBlobs()
            {
                Container = container,
                Paginate = false
            };
        
            var references = await _azureStorageWrapper.EnumerateBlobsAsync(query);
            
            _output.WriteLine($"Enumerating {references.References.Count()} references");
            
            Assert.True(references.References.Any());
            Assert.True(references.References.Count() >= amount);

            foreach (var reference in references.References)
            {
                await _azureStorageWrapper.DeleteBlobAsync(new DeleteBlob() { Uri = reference.Uri });
            }
        }
        
        [Fact]
        public async Task EnumerateBlobs_WithContinuationToken_ShouldReturnBlobsPageByPage()
        {
            var amount = 20;
            var size = 10;
            var container = "pagination";
            
            await UploadFilesAsync(amount, container);
            
            var firstIterationReferences = await _azureStorageWrapper.EnumerateBlobsAsync(new EnumerateBlobs()
            {
                Container = container,
                Paginate = true,
                Size = size,
            });
            
            _output.WriteLine($"Enumerating {firstIterationReferences.References.Count()} references");
            
            Assert.True(firstIterationReferences.References.Any());
            
            var secondIterationReferences = await _azureStorageWrapper.EnumerateBlobsAsync(new EnumerateBlobs()
            {
                Container = container,
                Paginate = true,
                Size = size,
                ContinuationToken = firstIterationReferences.ContinuationToken
            });

            _output.WriteLine($"Enumerating {secondIterationReferences.References.Count()} references");

            Assert.True(secondIterationReferences.References.Any());
            
            foreach (var reference in firstIterationReferences.References)
            {
                await _azureStorageWrapper.DeleteBlobAsync(new DeleteBlob() { Uri = reference.Uri });
            }
            
            foreach (var reference in secondIterationReferences.References)
            {
                await _azureStorageWrapper.DeleteBlobAsync(new DeleteBlob() { Uri = reference.Uri });
            }
        }

        private async Task UploadFilesAsync(int amount, string container)
        {
            for (var i = 0; i < amount; i++)
            {
                await _azureStorageWrapper.UploadBlobAsync(new UploadBase64()
                {
                    Base64 =  "SGVsbG8g8J+Zgg==",
                    Container = container,
                    Name = "hello",
                    Extension = "md",
                    Metadata = new Dictionary<string, string>()
                        {{"hello", "world"}}
                });
            }
        }
    }
}