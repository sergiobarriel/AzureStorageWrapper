using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AzureStorageWrapper.Commands;
using AzureStorageWrapper.Exceptions;

namespace AzureStorageWrapper
{
    public abstract class AzureStorageWrapperBase
    {
        internal readonly AzureStorageWrapperConfiguration Configuration;

        protected AzureStorageWrapperBase(AzureStorageWrapperConfiguration configuration)
        {
            Configuration = configuration;
        }

        internal void Validate(UploadBlob command)
        {
            if (string.IsNullOrEmpty(command.Container))
                throw new AzureStorageWrapperException($"{nameof(command.Container)} is empty!");

            if (string.IsNullOrEmpty(command.Name))
                throw new AzureStorageWrapperException($"{nameof(command.Name)} is empty!");

            if (string.IsNullOrEmpty(command.Extension))
                throw new AzureStorageWrapperException($"{nameof(command.Extension)} is empty!");
        }

        internal void Validate(DownloadBlobReference command)
        {
            if (string.IsNullOrEmpty(command.Uri))
                throw new AzureStorageWrapperException($"{nameof(command.Uri)} is empty!");

            if (command.ExpiresIn > Configuration.MaxSasUriExpiration)
                throw new AzureStorageWrapperException($"{nameof(command.ExpiresIn)} should be lower than {Configuration.MaxSasUriExpiration}");
        }

        internal void Validate(DeleteBlob command)
        {
            if (string.IsNullOrEmpty(command.Uri))
                throw new AzureStorageWrapperException($"{nameof(command.Uri)} is empty!");
        }

        internal void Validate(GetSasUri command)
        {
            if (string.IsNullOrEmpty(command.Uri))
                throw new AzureStorageWrapperException($"{nameof(command.Uri)} is empty!");

            if (command.ExpiresIn > Configuration.MaxSasUriExpiration)
                throw new AzureStorageWrapperException($"{nameof(command.ExpiresIn)} should be lower than {Configuration.MaxSasUriExpiration}");
        }

        /// <summary>
        /// ~2 centuries of work are needed in order to have a 1% probability of at least one collision: https://alex7kom.github.io/nano-nanoid-cc
        /// </summary>
        internal static string RandomString()
        {
            var guid = Guid.NewGuid().ToString("N");

            return guid.Substring(0, 15);
        }

        internal static Dictionary<string, string> SanitizeDictionary(Dictionary<string, string> metadata)
        {
            return metadata.ToDictionary(item => SanitizeKey(item.Key), item => SanitizeValue(item.Value));

            static string SanitizeKey(string key)
            {
                key = RemoveDiacritics(key);
                
                key = Regex.Replace(key, @"[^a-zA-Z0-9]+", "_");

                return key.ToUpper();
            }
            
            static string SanitizeValue(string @value)
            {
                value = RemoveDiacritics(value);

                return value;
            }
            
            static string RemoveDiacritics(string fileName)
            {
                var normalizedString = fileName.Normalize(NormalizationForm.FormD);
            
                var stringBuilder = new StringBuilder(capacity: normalizedString.Length);
            
                foreach (var @char in normalizedString)
                {
                    var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(@char);
            
                    if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(@char);
                    }
                }
            
                return stringBuilder
                    .ToString()
                    .Normalize(NormalizationForm.FormC);
            }
        }

    }
}