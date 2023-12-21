using AzureStorageWrapper.Commands;
using Xunit;

namespace AzureStorageWrapper.Tests.Should.Upload
{
    public class UploadBlobWithNameVariations : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;

        public UploadBlobWithNameVariations(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }

        [Fact]
        public async Task UploadBlob_WithBlankSpacesInName_Should_UploadBlob()
        {
            var base64 = "SGVsbG8g8J+Zgg==";

            var command = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = "hello world",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                    {{"hello", "world"}}
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(command);

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));
        }

        [Fact]
        public async Task UploadBlobWithDiacriticsInName_Should_UploadBlob()
        {
            var base64 = "SGVsbG8g8J+Zgg==";

            var command = new UploadBase64()
            {
                Base64 = base64,
                Container = "files",
                Name = "aeiouáéíóúäëïöüàèìòù",
                Extension = "md",
                Metadata = new Dictionary<string, string>()
                    {{"hello", "world"}}
            };

            var response = await _azureStorageWrapper.UploadBlobAsync(command);

            Assert.True(response.Name.IndexOfAny(new char[]{ 'á','é', 'í', 'ó', 'ú', 'ä', 'ë','ï', 'ö', 'ü', 'à', 'è', 'ì', 'ò', 'ù'}) > 0); // no diacritics

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));
        }

    }
}