using System;
using AzureStorageWrapper.Exceptions;

namespace AzureStorageWrapper.Commands
{
    public class DeleteBlob
    {
        public string Uri { get; set; }
        
        internal void Validate()
        {
            if (string.IsNullOrEmpty(Uri))
                throw new AzureStorageWrapperException($"{nameof(Uri)} is empty!");
            
            if(!System.Uri.TryCreate(Uri, UriKind.Absolute, out var @_))
                throw new AzureStorageWrapperException($"{nameof(Uri)} is not a valid absolute URI!");
        }
    }
}