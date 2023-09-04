namespace AzureStorageWrapper
{
    public class AzureStorageWrapperConfiguration
    {
        public AzureStorageWrapperConfiguration() { }

        public string ConnectionString { get; set; }
        public int DefaultSasUriExpiration { get; set; }
        public int MaxSasUriExpiration { get; set; }
        public bool CreateContainerIfNotExists { get; set; }

        internal void Validate()
        {
            ValidateConnectionString(ConnectionString);
            ValidateMaxSasUriExpiration(MaxSasUriExpiration);
            ValidateDefaultSasUriExpiration(DefaultSasUriExpiration);

            void ValidateConnectionString(string connectionString)
            {
                if (string.IsNullOrEmpty(connectionString)) throw new AzureStorageWrapperException($"{nameof(connectionString)} is empty!");
            }

            void ValidateMaxSasUriExpiration(int maxSasUriExpiration)
            {
                if (maxSasUriExpiration <= 0) throw new AzureStorageWrapperException($"{nameof(maxSasUriExpiration)} should be greater then zero");
            }

            void ValidateDefaultSasUriExpiration(int defaultSasUriExpiration)
            {
                if (defaultSasUriExpiration <= 0) throw new AzureStorageWrapperException($"{nameof(defaultSasUriExpiration)} should be greater then zero");
            }
        }
    }
}