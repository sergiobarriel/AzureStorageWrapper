namespace AzureStorageWrapper
{
    public interface IUriService
    {
        /// <summary>
        /// Returns the host of the URI
        /// In example: https://accountName.blob.core.windows.net/container/virtualFolder/file.extension -> https://accountName.blob.core.windows.net
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        string GetHost(string uri);
        
        /// <summary>
        /// Returns the container of the file in the URI
        /// In example: https://accountName.blob.core.windows.net/container/virtualFolder/file.extension -> container
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        string GetContainer(string uri);
        
        /// <summary>
        /// Returns the file name of the file in the URI
        /// In example: https://accountName.blob.core.windows.net/container/virtualFolder/file.extension -> virtualFolder/file
        /// In example: https://accountName.blob.core.windows.net/container/multiple/virtual/folder/file.extension -> multiple/virtual/folder/file
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        string GetFileName(string uri);
        
        /// <summary>
        /// Returns the file extension of the file in the URI
        /// In example: https://accountName.blob.core.windows.net/container/virtualFolder/file.extension -> extension
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        string GetFileExtension(string uri);
    }
}