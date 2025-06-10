using AzureStorageWrapper.Extensions;
using EnsureThat;

namespace AzureStorageWrapper.Queries
{
    public class DownloadBlob
    {
        public string Uri { get; set; }
        
        internal void Validate() => Ensure.String.IsNotUri(Uri);   
    }
}
