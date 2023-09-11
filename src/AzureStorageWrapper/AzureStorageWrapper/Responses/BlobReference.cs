using System;
using System.Collections.Generic;

namespace AzureStorageWrapper.Responses
{
    public class BlobReference
    {
        public string Container { get; set; }

        public string Name { get; set; }
        public string Extension { get; set; }

        public string Uri { get; set; }
        public string SasUri { get; set; }
        public DateTime SasExpires { get; set; }

        public IDictionary<string, string> Metadata { get; set; }
    }
}