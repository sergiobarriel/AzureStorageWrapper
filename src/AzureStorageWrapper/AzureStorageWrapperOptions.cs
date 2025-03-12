using AzureStorageWrapper.Exceptions;

namespace AzureStorageWrapper
{
    /// <summary>
    /// Options for configuring the Azure Storage Wrapper.
    /// </summary>
    public class AzureStorageWrapperOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageWrapperOptions"/> class.
        /// </summary>
        public AzureStorageWrapperOptions()
        {
            MaxSasUriExpiration = int.MaxValue;
        }

        private string _connectionString { get; set; }

        /// <summary>
        /// Gets or sets the connection string for the Azure Storage account.
        /// </summary>
        /// <exception cref="AzureStorageWrapperException">Thrown when the connection string is empty.</exception>
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

        /// <summary>
        /// Gets or sets the default container name.
        /// </summary>
        /// <exception cref="AzureStorageWrapperException">Thrown when the default container name is empty.</exception>
        public string DefaultContainer { get; set; }

        private int _maxSasUriExpiration;

        /// <summary>
        /// Gets or sets the maximum SAS URI expiration time in seconds.
        /// </summary>
        /// <exception cref="AzureStorageWrapperException">Thrown when the value is less than zero.</exception>
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

        private int _defaultSasUriExpiration;

        /// <summary>
        /// Gets or sets the default SAS URI expiration time in seconds.
        /// </summary>
        /// <exception cref="AzureStorageWrapperException">Thrown when the value is less than zero or greater than <see cref="MaxSasUriExpiration"/>.</exception>
        public int DefaultSasUriExpiration
        {
            get => _defaultSasUriExpiration;
            set
            {
                if (value < 0)
                    throw new AzureStorageWrapperException($"{nameof(DefaultSasUriExpiration)} should be greater than zero");

                if (value == 0)
                    _defaultSasUriExpiration = 360;

                if (value > MaxSasUriExpiration)
                    throw new AzureStorageWrapperException($"{nameof(DefaultSasUriExpiration)} should be lower than {nameof(MaxSasUriExpiration)}");

                _defaultSasUriExpiration = value;
            }
        }

        private bool _createContainerIfNotExists;

        /// <summary>
        /// Gets or sets a value indicating whether to create the container if it does not exist.
        /// </summary>
        public bool CreateContainerIfNotExists
        {
            get => _createContainerIfNotExists;
            set => _createContainerIfNotExists = value;
        }
    }
}
