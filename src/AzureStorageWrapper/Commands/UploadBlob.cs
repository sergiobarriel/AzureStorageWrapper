using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using AzureStorageWrapper.Extensions;
using EnsureThat;

namespace AzureStorageWrapper.Commands
{
    public abstract class UploadBlob
    {
        protected UploadBlob()
        {
            Metadata["_timestamp"] = $"{DateTime.UtcNow}";
            UseVirtualFolder = true;
        }

        public string Name { get; set; }
        public string Extension { get; set; }
        public string Container { get; set; }
        
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// If you set this property to 'false' the files will NOT be saved in virtual directories, and file names may collide, causing files to be overwritten
        /// </summary>
        public bool UseVirtualFolder { get; set; }

        public abstract Stream GetContent();
        
        internal void Validate()
        {
            Ensure.String.IsNotNullOrEmptySW(Container);
            Ensure.String.IsNotNullOrEmptySW(Name);
            Ensure.String.IsNotNullOrEmptySW(Extension);
        }
    }
}
