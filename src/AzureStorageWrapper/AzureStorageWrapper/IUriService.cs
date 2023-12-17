namespace AzureStorageWrapper
{
    public interface IUriService
    {
        string GetHost(string uri);
        string GetContainer(string uri);
        string GetFileName(string uri);
        string GetFileExtension(string uri);
    }
}