using System.Runtime.InteropServices.ComTypes;

namespace AzureStorageWrapper
{
    public class AzureStorageWrapperConfiguration
    {
        public AzureStorageWrapperConfiguration() { }

        private string _connectionString { get; set; }
        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new AzureStorageWrapperException($"{nameof(ConnectionString)} is empty");

                _connectionString = value;
            }
        }

        private int defaultSasUriExpiration;
        public int DefaultSasUriExpiration
        {
            get => defaultSasUriExpiration;
            set
            {
                if (value < 0)
                    throw new AzureStorageWrapperException($"{nameof(DefaultSasUriExpiration)} should be greater than zero");
                
                if (value == 0)
                    defaultSasUriExpiration = 360;

                if (value > MaxSasUriExpiration)
                    throw new AzureStorageWrapperException($"{nameof(DefaultSasUriExpiration)} should be lower than {nameof(MaxSasUriExpiration)}");


                defaultSasUriExpiration = value;
            }
        }

        private int _maxSasUriExpiration;
        public int MaxSasUriExpiration
        {
            get => _maxSasUriExpiration;
            set
            {
                if (value < 0)
                    throw new AzureStorageWrapperException($"{nameof(MaxSasUriExpiration)} should be greater than zero");

                if (value == 0)
                    _maxSasUriExpiration = 360;

                _maxSasUriExpiration = value;
            }
        }

        private bool _createContainerIfNotExists;
        public bool CreateContainerIfNotExists
        {
            get => _createContainerIfNotExists;
            set
            {
                _createContainerIfNotExists = value;
            }
        }
    }
}