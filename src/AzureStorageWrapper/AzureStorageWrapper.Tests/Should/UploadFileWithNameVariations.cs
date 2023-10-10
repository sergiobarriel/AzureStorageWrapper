using AzureStorageWrapper.Commands;
using System.Buffers.Text;
using Xunit;

namespace AzureStorageWrapper.Tests.Should
{
    public class UploadFileWithNameVariations : BaseShould
    {
        private readonly IAzureStorageWrapper _azureStorageWrapper;

        public UploadFileWithNameVariations(IAzureStorageWrapper azureStorageWrapper)
        {
            _azureStorageWrapper = azureStorageWrapper;
        }

        [Fact]
        public async Task UploadFileWithBlankSpacesInName_ShouldUploadFile()
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

            Assert.True(response.Name.IndexOf(" ", StringComparison.Ordinal) == -1); // no blank spaces

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));
        }

        [Fact]
        public async Task UploadFileWithDiacriticsInName_ShouldUploadFile()
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

            Assert.True(response.Name.IndexOfAny(new char[]{ 'á','é', 'í', 'ó', 'ú', 'ä', 'ë','ï', 'ö', 'ü', 'à', 'è', 'ì', 'ò', 'ù'}) == -1); // no diacritics

            Assert.NotNull(response);

            Assert.True(await PingAsync(response.SasUri));
        }

    }
}