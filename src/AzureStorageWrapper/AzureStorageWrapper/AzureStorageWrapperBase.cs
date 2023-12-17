using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AzureStorageWrapper.Commands;

namespace AzureStorageWrapper
{
    public abstract class AzureStorageWrapperBase
    {
        internal readonly AzureStorageWrapperConfiguration _configuration;

        protected AzureStorageWrapperBase(AzureStorageWrapperConfiguration configuration)
        {
            _configuration = configuration;
        }

        internal void ValidateCommand(UploadBlob command)
        {
            if (string.IsNullOrEmpty(command.Container))
                throw new AzureStorageWrapperException($"{nameof(command.Container)} is empty!");

            if (string.IsNullOrEmpty(command.Name))
                throw new AzureStorageWrapperException($"{nameof(command.Name)} is empty!");

            if (string.IsNullOrEmpty(command.Extension))
                throw new AzureStorageWrapperException($"{nameof(command.Extension)} is empty!");
        }

        internal void ValidateCommand(DownloadBlobReference command)
        {
            if (string.IsNullOrEmpty(command.Uri))
                throw new AzureStorageWrapperException($"{nameof(command.Uri)} is empty!");

            if (command.ExpiresIn > _configuration.MaxSasUriExpiration)
                throw new AzureStorageWrapperException($"{nameof(command.ExpiresIn)} should be lower than {_configuration.MaxSasUriExpiration}");
        }

        internal void ValidateCommand(DeleteBlob command)
        {
            if (string.IsNullOrEmpty(command.Uri))
                throw new AzureStorageWrapperException($"{nameof(command.Uri)} is empty!");
        }

        internal void ValidateCommand(GetSasUri command)
        {
            if (string.IsNullOrEmpty(command.Uri))
                throw new AzureStorageWrapperException($"{nameof(command.Uri)} is empty!");

            if (command.ExpiresIn > _configuration.MaxSasUriExpiration)
                throw new AzureStorageWrapperException($"{nameof(command.ExpiresIn)} should be lower than {_configuration.MaxSasUriExpiration}");
        }

        /// <summary>
        /// ~2 centuries of work are needed in order to have a 1% probability of at least one collision: https://alex7kom.github.io/nano-nanoid-cc
        /// </summary>
        internal string RandomString()
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