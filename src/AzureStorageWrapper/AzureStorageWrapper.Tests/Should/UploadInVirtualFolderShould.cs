using AzureStorageWrapper.Commands;
using Xunit;

namespace AzureStorageWrapper.Tests.Should
{
    public class UploadInVirtualFolderShould : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;

        public UploadInVirtualFolderShould(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }

        [Fact]
        public async Task UploadFileInContainer_ShouldUploadFile()
        {
            var base64 = "SGVsbG8g8J+Zgg==";
        
            var command = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = "hello",
                Extension = "md",
                UseVirtualFolder = false,
                Metadata = new Dictionary<string, string>()
                {
                    { "UseVirtualFolder", "false" }
                }
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(command);
        
            Assert.True(await PingAsync(response.SasUri));
            Assert.Contains("files/hello.md", response.Uri);
        }
    
        [Fact]
        public async Task UploadFileInVirtualFolder_ShouldUploadFile()
        {
            var base64 = "SGVsbG8g8J+Zgg==";
        
            var command = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = "hello",
                Extension = "md",
                UseVirtualFolder = true,
                Metadata = new Dictionary<string, string>()
                {
                    { "UseVirtualFolder", "true" }
                }
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(command);
        
            Assert.True(await PingAsync(response.SasUri));
            Assert.DoesNotContain("files/hello.md", response.Uri);
        }
    
        [Fact]
        public async Task UploadFile_WithoutSettingUseVirtualFolderProperty_ShouldUploadFile()
        {
            var base64 = "SGVsbG8g8J+Zgg==";
        
            var command = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = "hello",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                {
                    { "UseVirtualFolder", "not set in command" }
                }
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(command);
        
            Assert.True(await PingAsync(response.SasUri));
            Assert.DoesNotContain("files/hello.md", response.Uri);
        }
    }
}