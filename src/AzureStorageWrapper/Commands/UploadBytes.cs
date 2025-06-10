using System.IO;
using AzureStorageWrapper.Extensions;
using EnsureThat;

namespace AzureStorageWrapper.Commands
{
    public class UploadBytes : UploadBlob
    {
        public byte[] Bytes { get; set; }

        public override Stream GetContent() 
        {
            Ensure.Any.IsNotZero(Bytes);
            return new MemoryStream(Bytes);
        }
    }
}
