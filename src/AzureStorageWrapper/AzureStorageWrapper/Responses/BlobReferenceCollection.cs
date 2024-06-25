using System.Collections.Generic;

namespace AzureStorageWrapper.Responses
{
    public class BlobReferenceCollection
    {
        public IEnumerable<BlobReference> References { get; set; }
        public string ContinuationToken { get; set; }
    }
}