using System.Collections.Generic;
using System.IO;
using AzureStorageWrapper.Exceptions;

namespace AzureStorageWrapper.Commands
{
    public abstract class UploadBlob
    {
        protected UploadBlob()
        {
            Metadata = new Dictionary<string, string>();
            UseVirtualFolder = true;
        }

        public string Name { get; set; }
        public string Extension { get; set; }
        public string Container { get; set; }
        
        public Dictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// If you set this property to 'false' the files will NOT be saved in virtual directories, and file names may collide, causing files to be overwritten
        /// </summary>
        public bool UseVirtualFolder { get; set; }

        public abstract Stream GetContent();
        
        internal void Validate()
        {
            if (string.IsNullOrEmpty(Container))
                throw new AzureStorageWrapperException($"{nameof(Container)} is empty!");

            if (string.IsNullOrEmpty(Name))
                throw new AzureStorageWrapperException($"{nameof(Name)} is empty!");

            if (string.IsNullOrEmpty(Extension))
                throw new AzureStorageWrapperException($"{nameof(Extension)} is empty!");
        }
    }
}
