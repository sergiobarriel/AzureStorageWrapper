using System;
using System.IO;

namespace AzureStorageWrapper.Commands
{
    public class UploadStream : UploadBlob
    {
        public Stream Stream { get; set; }

        public override Stream GetContent()
        {
            if (Stream.Length == 0) throw new Exception($"{nameof(Stream)} length is 0");

            return Stream;
        }
    }
}