namespace AzureStorageWrapper.Tests.Sources
{
    public static class Uris
    {
        public static string ExistingFile = "https://stgazstgwrapper001westeu.blob.core.windows.net/static-files/hello.md";
        public static string UnexistingFile = "https://stgazstgwrapper001westeu.blob.core.windows.net/static-files/unexisting-hello.md";

        public static string ExistingFileWithBlanks = "https://stgazstgwrapper001westeu.blob.core.windows.net/static-files/hello world.md";
        public static string ExistingFileWithUriEncoded = "https://stgazstgwrapper001westeu.blob.core.windows.net/static-files/hello%20world.md";
    }
}