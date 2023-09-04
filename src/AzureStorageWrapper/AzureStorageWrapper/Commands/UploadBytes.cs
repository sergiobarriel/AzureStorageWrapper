using System.IO;

namespace AzureStorageWrapper.Commands
{
    public class UploadBytes : UploadBlob
    {
        public byte[] Bytes { get; set; }

        public override Stream GetContent()
        {
            if (Bytes.Length == 0) throw new AzureStorageWrapperException($"{nameof(Bytes)} length is 0");

            return new MemoryStream(Bytes);
        }
    }
}