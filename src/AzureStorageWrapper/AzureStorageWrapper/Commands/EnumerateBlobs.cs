using AzureStorageWrapper.Exceptions;

namespace AzureStorageWrapper.Commands
{
    public class EnumerateBlobs
    {
        public string Container { get; set; }
        public int Size { get; set; }
        public string ContinuationToken { get; set; }
        
        internal void Validate()
        {
            if (string.IsNullOrEmpty(Container))
                throw new AzureStorageWrapperException($"{nameof(Container)} is empty!");
            
            if (Size <= 0)
                throw new AzureStorageWrapperException($"{nameof(Size)} should be greater than zero.");
        }
    }
}