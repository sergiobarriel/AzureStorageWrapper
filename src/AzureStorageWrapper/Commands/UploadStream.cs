using System.IO;
using AzureStorageWrapper.Extensions;
using EnsureThat;

namespace AzureStorageWrapper.Commands
{
    public class UploadStream : UploadBlob
    {
        public Stream Stream { get; set; }

        public override Stream GetContent() => Ensure.Any.IsNotZero(Stream);
    }
}
