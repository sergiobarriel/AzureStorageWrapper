using System.Collections.Generic;

namespace AzureStorageWrapper.Responses
{
    public class BlobReferenceCollection
    {
        public BlobReferenceCollection()
            => References = new List<BlobReference>();    
        
        public IEnumerable<BlobReference> References { get; set; }
        public string ContinuationToken { get; set; }
    }
}
