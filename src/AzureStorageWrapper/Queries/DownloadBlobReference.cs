using System;
using AzureStorageWrapper.Exceptions;

namespace AzureStorageWrapper.Queries
{
    public class DownloadBlobReference
    {
        public string Uri { get; set; }
        public int ExpiresIn { get; set; }
        
        internal void Validate(AzureStorageWrapperOptions options)
        {
            if (string.IsNullOrEmpty(Uri))
                throw new AzureStorageWrapperException($"{nameof(Uri)} is empty!");
            
            if(!System.Uri.TryCreate(Uri, UriKind.Absolute, out var @_))
                throw new AzureStorageWrapperException($"{nameof(Uri)} is not a valid absolute URI!");
            
            if (ExpiresIn > options.MaxSasUriExpiration)
                throw new AzureStorageWrapperException($"{nameof(ExpiresIn)} should be lower than {options.MaxSasUriExpiration}");
        }
    }
}
