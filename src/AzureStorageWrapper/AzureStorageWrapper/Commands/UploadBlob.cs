using System.Collections.Generic;
using System.IO;

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

        public bool UseVirtualFolder { get; set; }

        public abstract Stream GetContent();
    }
}
