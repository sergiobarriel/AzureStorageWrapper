using System;
using System.IO;

namespace AzureStorageWrapper.Models
{
    public class UploadBytes : UploadBlob
    {
        public byte[] Bytes { get; set; }

        public override Stream GetContent()
        {
            if (Bytes.Length == 0) throw new Exception($"{nameof(Bytes)} length is 0");

            return new MemoryStream(Bytes);
        }
    }
}