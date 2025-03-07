using System;
using AzureStorageWrapper.Exceptions;

namespace AzureStorageWrapper.Queries
{
    internal class GetSasUri
    {
        public string Uri { get; set; }
        public int ExpiresIn { get; set; }
        
        internal void Validate(AzureStorageWrapperConfiguration configuration)
        {
            if (string.IsNullOrEmpty(Uri))
                throw new AzureStorageWrapperException($"{nameof(Uri)} is empty!");
            
            if(!System.Uri.TryCreate(Uri, UriKind.Absolute, out var @_))
                throw new AzureStorageWrapperException($"{nameof(Uri)} is not a valid absolute URI!");
            
            if (ExpiresIn > configuration.MaxSasUriExpiration)
                throw new AzureStorageWrapperException($"{nameof(ExpiresIn)} should be lower than {configuration.MaxSasUriExpiration}");
        }
    }
}