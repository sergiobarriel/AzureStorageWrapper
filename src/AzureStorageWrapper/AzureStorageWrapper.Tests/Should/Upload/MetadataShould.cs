using AzureStorageWrapper.Commands;
using Xunit;

namespace AzureStorageWrapper.Tests.Should.Upload;

public class MetadataShould : BaseShould
{
    private readonly IAzureStorageWrapper _azureStorageWrapper;

    public MetadataShould(IAzureStorageWrapper azureStorageWrapper)
    {
        _azureStorageWrapper = azureStorageWrapper;
    }
        
    [Fact]
    public async Task UploadBlob_WithMetadataDiacriticsKey_Should_UploadBlob()
    {
        var base64 = "SGVsbG8g8J+Zgg==";
        var diacriticsOne = "áéíóú";
        var diacriticsTwo = "áéíóúäëïöüàèìòù";
            
        var command = new UploadBase64()
        {
            Base64 = base64,
            Container = "files",
            Name = "hello",
            Extension = "md",
            Metadata = new Dictionary<string, string>()
            {
                { diacriticsOne, "diacritics_one" }, 
                { diacriticsTwo, "diacritics_two"  }, 
            },
        };

        var response = await _azureStorageWrapper.UploadBlobAsync(command);

        Assert.NotNull(response);

        Assert.True(response.Metadata.TryGetValue("aeiou", out var _));
        Assert.True(response.Metadata.TryGetValue("aeiouaeiouaeiou", out var _));
        
        Assert.True(await PingAsync(response.SasUri));
    }
        
    [Fact]
    public async Task UploadBlob_WithDiacriticsMetadataValue_Should_UploadBlob()
    {
        var base64 = "SGVsbG8g8J+Zgg==";
        var diacriticsOne = "áéíóú";
        var diacriticsTwo = "áéíóúäëïöüàèìòù";
            
        var command = new UploadBase64()
        {
            Base64 = base64,
            Container = "files",
            Name = "hello",
            Extension = "md",
            Metadata = new Dictionary<string, string>()
            {
                { "diacritics_one", diacriticsOne }, 
                { "diacritics_two", diacriticsTwo }, 
            },
        };

        var response = await _azureStorageWrapper.UploadBlobAsync(command);

        Assert.NotNull(response);
        
        Assert.Equal("aeiou", response.Metadata["diacritics_one"]);
        Assert.Equal("aeiouaeiouaeiou", response.Metadata["diacritics_two"]);

        Assert.True(await PingAsync(response.SasUri));
    }
    
    
    [Fact]
    public async Task UploadBlob_WithBlankSpaceMetadataKey_Should_UploadBlob()
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
                { "xxx xxx", "xxx" }, 
                { "zzz zzz", "zzz" }, 
            },
        };

        var response = await _azureStorageWrapper.UploadBlobAsync(command);

        Assert.NotNull(response);
        
        Assert.True(response.Metadata.TryGetValue("xxx_xxx", out var _));
        Assert.True(response.Metadata.TryGetValue("zzz_zzz", out var _));

        Assert.True(await PingAsync(response.SasUri));
    }
}