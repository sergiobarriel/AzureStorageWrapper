using AzureStorageWrapper.Extensions;
using EnsureThat;

namespace AzureStorageWrapper.Queries
{
    internal class GetSasUri
    {
        public string Uri { get; set; }
        public int ExpiresIn { get; set; }
        
        internal void Validate(AzureStorageWrapperOptions configuration)
        {
            Ensure.String.IsNotUri(Uri);
            Ensure.Comparable.IsLteSW(ExpiresIn, configuration.MaxSasUriExpiration);
        }
    }
}
