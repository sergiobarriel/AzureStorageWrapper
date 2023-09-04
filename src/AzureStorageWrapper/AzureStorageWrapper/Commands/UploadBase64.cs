using System;
using System.IO;

namespace AzureStorageWrapper.Commands
{
    public class UploadBase64 : UploadBlob
    {
        public string Base64 { get; set; }

        public override Stream GetContent()
        {
            if (string.IsNullOrEmpty(Base64)) throw new AzureStorageWrapperException($"{nameof(Base64)} is empty");

            var bytes = Convert.FromBase64String(Base64);

            return new MemoryStream(bytes);
        }
    }
}