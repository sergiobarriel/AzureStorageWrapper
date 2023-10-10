namespace AzureStorageWrapper.Tests.Sources;

public static class Uris
{
    public static string ExistingBlobUri = "https://stgazstgwrapper001westeu.blob.core.windows.net/static-files/hello.md";
    public static string UnExistingBlobUri = "https://stgazstgwrapper001westeu.blob.core.windows.net/static-files/unexisting-hello.md";

    public static string ExistingBlobUriWithBlanks = "https://stgazstgwrapper001westeu.blob.core.windows.net/static-files/hello world.md";
    public static string ExistingBlobUriWithEncodedBlanks = "https://stgazstgwrapper001westeu.blob.core.windows.net/static-files/hello%20world.md";
}