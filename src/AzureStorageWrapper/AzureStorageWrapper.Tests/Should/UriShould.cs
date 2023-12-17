﻿using Xunit;

namespace AzureStorageWrapper.Tests.Should
{
    public class UriShould : BaseShould
    {
        private readonly IUriService _uriService;

        private string UriWithVirtualFolder = $"https://accountname.blob.core.windows.net/container/virtualFolder/file.extension";
        private string UriWithoutVirtualFolder = $"https://accountname.blob.core.windows.net/container/file.extension";
        public UriShould(IUriService uriService)
        {
            _uriService = uriService;
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
        public void GetContainer_WithVirtualFolder_ShouldReturnHost()
        {
            var container = _uriService.GetContainer(UriWithVirtualFolder);
        
            Assert.Equal("container", container);
        }
    
        [Fact]
        public void GetContainer_WithoutVirtualFolder_ShouldReturnHost()
        {
            var container = _uriService.GetContainer(UriWithoutVirtualFolder);
        
            Assert.Equal("container", container);
        }
    
        [Fact]
        public void GetFileName_WithVirtualFolder_ShouldReturnHost()
        {
            var fileName = _uriService.GetFileName(UriWithVirtualFolder);
        
            Assert.Equal("file", fileName);
        }
    
        [Fact]
        public void GetFileName_WithoutVirtualFolder_ShouldReturnHost()
        {
            var fileName = _uriService.GetFileName(UriWithoutVirtualFolder);
        
            Assert.Equal("file", fileName);
        }
    
        [Fact]
        public void GetFileExtension_WithVirtualFolder_ShouldReturnHost()
        {
            var fileExtension = _uriService.GetFileExtension(UriWithVirtualFolder);
        
            Assert.Equal("extension", fileExtension);
        }
    
        [Fact]
        public void GetFileExtension_WithoutVirtualFolder_ShouldReturnHost()
        {
            var fileExtension = _uriService.GetFileExtension(UriWithoutVirtualFolder);
        
            Assert.Equal("extension", fileExtension);
        }
    }
}