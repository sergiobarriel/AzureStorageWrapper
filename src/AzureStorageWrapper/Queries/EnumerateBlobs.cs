using AzureStorageWrapper.Extensions;
using EnsureThat;

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
            Ensure.String.IsNotNullOrEmptySW(Container);
            Ensure.Bool.IsPaginateValid(Paginate, Size);
            Ensure.Bool.IsPaginateValid(Paginate, ContinuationToken);
        }
    }
}
