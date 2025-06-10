using System;
using System.IO;
using System.Text.RegularExpressions;
using AzureStorageWrapper.Exceptions;

namespace AzureStorageWrapper.Commands
{
    public class UploadBase64 : UploadBlob
    {
        public string Base64 { get; set; }

        public override Stream GetContent()
        {
            if (string.IsNullOrEmpty(Base64)) throw new AzureStorageWrapperException($"{nameof(Base64)} is empty");

            var regex = new Regex(@"^data:[^;]+;base64,");

            Base64 = regex.Replace(Base64, string.Empty);

            try
            {
                var bytes = Convert.FromBase64String(Base64);
                return new MemoryStream(bytes);
            }
            catch (Exception)
            {
                throw new AzureStorageWrapperException("Invalid base64 string");
            }
        }
    }
}
