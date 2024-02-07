using Xunit;

namespace AzureStorageWrapper.Tests.Should
{
    public class UriShould : BaseShould
    {
        private readonly IUriService _uriService;
        
        private string UriWithVirtualFolder = $"https://accountname.blob.core.windows.net/container/virtualFolder/file.extension";
        private string UriWithMultipleVirtualFolder = $"https://accountname.blob.core.windows.net/container/multiple/virtual/folder/file.extension";
        private string UriWithoutVirtualFolder = $"https://accountname.blob.core.windows.net/container/file.extension";
        
        private string UriWithTwoExtensions = $"https://accountname.blob.core.windows.net/container/file.extension.extension";
        private string UriWithVirtualFolderWithTwoExtensions = $"https://accountname.blob.core.windows.net/container/virtual/folder/file.extension.extension";
        private string UriWithManyExtensions = $"https://accountname.blob.core.windows.net/container/file.extension.extension.extension";
        private string UriWithVirtualFolderWithManyExtensions = $"https://accountname.blob.core.windows.net/container/virtual/folder/file.extension.extension.extension";
        
        public UriShould()
        {
            _uriService = new UriService();
        }


        [Fact]
        public void GetHost_WithVirtualFolder_ShouldReturnHost()
        {
            var host = _uriService.GetHost(UriWithVirtualFolder);
        
            Assert.Equal("https://accountname.blob.core.windows.net", host);
        }
    
        [Fact]
        public void GetHost_WithoutVirtualFolder_ShouldReturnHost()
        {
            var host = _uriService.GetHost(UriWithoutVirtualFolder);
        
            Assert.Equal("https://accountname.blob.core.windows.net", host);
        }
        
        [Fact]
        public void GetHost_WithMultipleVirtualFolder_ShouldReturnHost()
        {
            var host = _uriService.GetHost(UriWithMultipleVirtualFolder);
        
            Assert.Equal("https://accountname.blob.core.windows.net", host);
        }
    
        [Fact]
        public void GetContainer_WithVirtualFolder_ShouldReturnContainer()
        {
            var container = _uriService.GetContainer(UriWithVirtualFolder);
        
            Assert.Equal("container", container);
        }
    
        [Fact]
        public void GetContainer_WithoutVirtualFolder_ShouldReturnContainer()
        {
            var container = _uriService.GetContainer(UriWithoutVirtualFolder);
        
            Assert.Equal("container", container);
        }
    
        [Fact]
        public void GetContainer_WithMultipleVirtualFolder_ShouldReturnContainer()
        {
            var container = _uriService.GetContainer(UriWithMultipleVirtualFolder);
        
            Assert.Equal("container", container);
        }
        
        [Fact]
        public void GetFileName_WithVirtualFolder_ShouldReturnFileName()
        {
            var fileName = _uriService.GetFileName(UriWithVirtualFolder);
        
            Assert.Equal("virtualFolder/file", fileName);
        }
        
       
        [Fact]
        public void GetFileName_WithoutVirtualFolder_ShouldReturnFileName()
        {
            var fileName = _uriService.GetFileName(UriWithoutVirtualFolder);
        
            Assert.Equal("file", fileName);
        }
    
        [Fact]
        public void GetFileName_WithTwoExtension_ShouldReturnFileName()
        {
            var fileName = _uriService.GetFileName(UriWithTwoExtensions);
        
            Assert.Equal("file.extension", fileName);
        }
        
        [Fact]
        public void GetFileName_WithVirtualFolder_WithTwoExtension_ShouldReturnFileName()
        {
            var fileName = _uriService.GetFileName(UriWithVirtualFolderWithTwoExtensions);
        
            Assert.Equal("virtual/folder/file.extension", fileName);
        }
        
        [Fact]
        public void GetFileName_WithManyExtension_ShouldReturnFileName()
        {
            var fileName = _uriService.GetFileName(UriWithManyExtensions);
        
            Assert.Equal("file.extension.extension", fileName);
        }     
        
        [Fact]
        public void GetFileName_WithVirtualFolder_WithManyExtension_ShouldReturnFileName()
        {
            var fileName = _uriService.GetFileName(UriWithVirtualFolderWithManyExtensions);
        
            Assert.Equal("virtual/folder/file.extension.extension", fileName);
        }        

        [Fact]
        public void GetFileName_WithMultipleVirtualFolder_ShouldReturnFileName()
        {
            var fileName = _uriService.GetFileName(UriWithMultipleVirtualFolder);
        
            Assert.Equal("multiple/virtual/folder/file", fileName);
        }
        
        [Fact]
        public void GetFileExtension_WithVirtualFolder_ShouldReturnFileExtension()
        {
            var fileExtension = _uriService.GetFileExtension(UriWithVirtualFolder);
        
            Assert.Equal("extension", fileExtension);
        }
    
        [Fact]
        public void GetFileExtension_WithoutVirtualFolder_ShouldReturnFileExtension()
        {
            var fileExtension = _uriService.GetFileExtension(UriWithoutVirtualFolder);
        
            Assert.Equal("extension", fileExtension);
        }
        
        [Fact]
        public void GetFileExtension_WithMultipleVirtualFolder_ShouldReturnFileExtension()
        {
            var fileExtension = _uriService.GetFileExtension(UriWithMultipleVirtualFolder);
        
            Assert.Equal("extension", fileExtension);
        }
    }
}