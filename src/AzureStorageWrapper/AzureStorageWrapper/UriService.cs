using System;
using System.IO;

namespace AzureStorageWrapper
{
    public class UriService : IUriService
    {
        public UriService() { }
        
        public string GetHost(string uri)
        {
            var uriObject = new Uri(uri);

            return $"{uriObject.Scheme}://{uriObject.Authority}";
        }

        public string GetContainer(string uri)
        {
            var uriObject = new Uri(uri);

            return uriObject.Segments[1].TrimEnd('/');
        }

        public string GetFileName(string uri)
        {
            var uriObject = new Uri(uri);

            return Path.GetFileNameWithoutExtension(uriObject.LocalPath);
        }
        
        public string GetFileExtension(string uri)
        {
            var uriObject = new Uri(uri);

            return Path.GetExtension(uriObject.LocalPath).TrimStart('.');
        }

    }
}