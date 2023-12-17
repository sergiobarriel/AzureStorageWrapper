namespace AzureStorageWrapper.Tests.Sources
{
    public static class Uris
    {
        public const string ExistingFile = "https://stgazstgwrapper001westeu.blob.core.windows.net/static/hello.md";

        public const string ExistingFileWithManyDots = "https://stgazstgwrapper001westeu.blob.core.windows.net/static/hello.hello.hello.hello.md";

        public const string ExistingFileWithManyExtensions = "https://stgazstgwrapper001westeu.blob.core.windows.net/static/hello.md.md";

        public const string UnExistingFile = "https://stgazstgwrapper001westeu.blob.core.windows.net/static/unexisting-hello.md";

        public const string ExistingFileWithBlanks = "https://stgazstgwrapper001westeu.blob.core.windows.net/static/hello hello.md";

        public const string ExistingFileWithUriEncoded = "https://stgazstgwrapper001westeu.blob.core.windows.net/static/hello%20hello.md";
    }
}