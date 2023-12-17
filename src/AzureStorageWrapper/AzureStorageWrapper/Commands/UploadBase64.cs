using System;
using System.IO;
using System.Text.RegularExpressions;

namespace AzureStorageWrapper.Commands
{
    public class UploadBase64 : UploadBlob
    {
        public string Base64 { get; set; }

        public override Stream GetContent()
        {
            if (string.IsNullOrEmpty(Base64)) throw new AzureStorageWrapperException($"{nameof(Base64)} is empty");

            RemoveBase64Header();

            var bytes = Convert.FromBase64String(Base64);

            return new MemoryStream(bytes);
        }

        /// <summary>
        /// Remove starting tags from base64 like:
        /// data:image/png;base64,...
        /// data:application/pdf;base64,...
        /// </summary>
        private void RemoveBase64Header()
        {
            var regex = new Regex(@"^data:image\/[a-zA-Z]+;base64,");

            Base64 = regex.Replace(Base64, string.Empty);
        }
    }
}