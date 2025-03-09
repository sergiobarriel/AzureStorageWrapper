using AzureStorageWrapper.Extensions;
using EnsureThat;

namespace AzureStorageWrapper.Queries
{
    public class DownloadBlobReference
    {
        public string Uri { get; set; }
        public int ExpiresIn { get; set; }
        
        internal void Validate(AzureStorageWrapperOptions option)
        {
            Ensure.String.IsNotUri(Uri);
            Ensure.Comparable.IsLteSW(ExpiresIn, option.MaxSasUriExpiration);
        }
    }
}
