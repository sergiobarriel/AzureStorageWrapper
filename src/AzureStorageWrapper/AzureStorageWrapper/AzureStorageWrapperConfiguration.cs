using System;

namespace AzureStorageWrapper
{
    public class AzureStorageWrapperConfiguration
    {
        public AzureStorageWrapperConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public AzureStorageWrapperConfiguration(string connectionString, int maxSasUriExpiration)
        {
            ConnectionString = connectionString; 
            MaxSasUriExpiration = maxSasUriExpiration;
        }

        public AzureStorageWrapperConfiguration(string connectionString, int maxSasUriExpiration, int defaultSasUriExpiration)
        {
            ConnectionString = connectionString;
            MaxSasUriExpiration = maxSasUriExpiration;
            DefaultSasUriExpiration = defaultSasUriExpiration;
        }

        internal string ConnectionString { get; }
        internal int DefaultSasUriExpiration { get; }
        internal int MaxSasUriExpiration { get; }

        internal void Validate()
        {
            ValidateConnectionString(ConnectionString);
            ValidateMaxSasUriExpiration(MaxSasUriExpiration);
            ValidateDefaultSasUriExpiration(DefaultSasUriExpiration);

            void ValidateConnectionString(string connectionString)
            {
                if (string.IsNullOrEmpty(connectionString)) throw new Exception($"{nameof(connectionString)} is empty!");
            }

            void ValidateMaxSasUriExpiration(int maxSasUriExpiration)
            {
                if (maxSasUriExpiration <= 0) throw new Exception($"{nameof(maxSasUriExpiration)} should be greater then zero");
            }

            void ValidateDefaultSasUriExpiration(int defaultSasUriExpiration)
            {
                if (defaultSasUriExpiration <= 0) throw new Exception($"{nameof(defaultSasUriExpiration)} should be greater then zero");
            }
        }
    }
}