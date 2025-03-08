using AzureStorageWrapper.Exceptions;

namespace AzureStorageWrapper.Queries
{
    public class EnumerateBlobs
    {
        public string Container { get; set; }
        public bool Paginate { get; set; }
        public int Size { get; set; }
        public string ContinuationToken { get; set; }
        
        internal void Validate()
        {
            if (string.IsNullOrEmpty(Container))
                throw new AzureStorageWrapperException($"{nameof(Container)} is empty!");
            
            if (Paginate && Size <= 0)
                throw new AzureStorageWrapperException($"{nameof(Size)} should be greater than zero when {nameof(Paginate)} is true.");
            
            if(!Paginate && !string.IsNullOrEmpty(ContinuationToken))
                throw new AzureStorageWrapperException($"{nameof(ContinuationToken)} should be empty when {nameof(Paginate)} is false.");
        }
    }
}