using System;

namespace AzureStorageWrapper.Exceptions
{
    public class AzureStorageWrapperException : Exception
    {
        public AzureStorageWrapperException(string message) : base(message) { }
    }
}
