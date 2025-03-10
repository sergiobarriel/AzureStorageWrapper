using AzureStorageWrapper.Extensions;
using EnsureThat;

namespace AzureStorageWrapper.Commands
{
    /// <summary>
    /// Represents a command to delete a blob.
    /// </summary>
    public class DeleteBlob
    {
        /// <summary>
        /// Gets or sets the URI of the blob to be deleted.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Validates the URI of the blob.
        /// </summary>
        internal void Validate() => Ensure.String.IsNotUri(Uri);
    }
}
