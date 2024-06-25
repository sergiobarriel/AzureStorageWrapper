using AzureStorageWrapper.Exceptions;

namespace AzureStorageWrapper.Commands
{
    public class EnumerateAllBlobs
    {
        public string Container { get; set; }
        
        internal void Validate()
        {
            if (string.IsNullOrEmpty(Container))
                throw new AzureStorageWrapperException($"{nameof(Container)} is empty!");
            
        }
    }
}