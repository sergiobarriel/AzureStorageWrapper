using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AzureStorageWrapper.Models;

namespace AzureStorageWrapper
{
    public abstract class AzureStorageWrapperBase
    {
        private readonly AzureStorageWrapperConfiguration _configuration;

        public AzureStorageWrapperBase(AzureStorageWrapperConfiguration configuration)
        {
            _configuration = configuration;
        }

        internal void ValidateUploadBlobCommand(UploadBlob command)
        {
            if (string.IsNullOrEmpty(command.Container)) throw new Exception($"{nameof(command.Container)} is empty!");
            if (string.IsNullOrEmpty(command.Name)) throw new Exception($"{nameof(command.Name)} is empty!");
            if (string.IsNullOrEmpty(command.Extension)) throw new Exception($"{nameof(command.Extension)} is empty!");
        }

        internal void ValidateDownloadBlobReferenceCommand(DownloadBlobReference command)
        {
            if (string.IsNullOrEmpty(command.Container)) throw new Exception($"{nameof(command.Container)} is empty!");
            if (string.IsNullOrEmpty(command.Name)) throw new Exception($"{nameof(command.Name)} is empty!");
            if (string.IsNullOrEmpty(command.Extension)) throw new Exception($"{nameof(command.Extension)} is empty!");
        }

        internal void ValidateGetSasUriCommand(GetSasUri command)
        {
            if (string.IsNullOrEmpty(command.Container)) throw new Exception($"{nameof(command.Container)} is empty!");
            if (string.IsNullOrEmpty(command.Name)) throw new Exception($"{nameof(command.Name)} is empty!");
            if (string.IsNullOrEmpty(command.Extension)) throw new Exception($"{nameof(command.Extension)} is empty!");
            
            if (command.ExpiresIn > _configuration.MaxSasUriExpiration) throw new Exception($"{nameof(command.ExpiresIn)} should be lower than {_configuration.MaxSasUriExpiration}");
        }

        /// <summary>
        /// ~2 centuries of work are needed in order to have a 1% probability of at least one collision: https://alex7kom.github.io/nano-nanoid-cc
        /// </summary>
        internal string GenerateBlobName()
        {
            var guid = Guid.NewGuid().ToString("N");

            return guid.Substring(0, 15);
        }

        internal Dictionary<string, string> SanitizeDictionary(Dictionary<string, string> metadata)
        {
            return metadata.ToDictionary(item => SanitizeDictionaryKey(item.Key), item => item.Value);

            string SanitizeDictionaryKey(string key)
            {
                key = Regex.Replace(key, @"[^a-zA-Z0-9]+", "_");

                return key.ToUpper();
            }
        }

    }
}