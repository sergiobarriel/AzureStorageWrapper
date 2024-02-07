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
            
            var host = GetHost(uri);
            var container = GetContainer(uri);
            var extension = GetFileExtension(uri);

            var remainder = uriObject.LocalPath
                .Replace($"{host}", string.Empty)
                .Replace($"/{container}/", string.Empty);

            var index = remainder.LastIndexOf($".{extension}", StringComparison.Ordinal);

            var fileName = remainder.Substring(0, index);

            return fileName;
        }
        
        public string GetFileExtension(string uri)
        {
            var uriObject = new Uri(uri);

            return Path.GetExtension(uriObject.LocalPath).TrimStart('.');
        }

    }
}