using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AzureStorageWrapper.Tests
{
    public class DependencyInjectionTests
    {
        [Fact]
        public void AddAzureStorageWrapper_WithDefaultParameters_ShouldConfigureServices()
        {
            // Arrange
            var services = new ServiceCollection();
            Environment.SetEnvironmentVariable("StorageWrapper_ConnectionString", "DefaultConnectionString");
            Environment.SetEnvironmentVariable("StorageWrapper_DefaultContainer", "DefaultContainer");

            // Act
            services.AddAzureStorageWrapper();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<AzureStorageWrapperOptions>();
            Assert.NotNull(options);
            Assert.Equal("DefaultConnectionString", options.ConnectionString);
            Assert.Equal("DefaultContainer", options.DefaultContainer);
        }

        [Fact]
        public void AddAzureStorageWrapper_WithConnectionStringAndContainer_ShouldConfigureServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var connectionString = "TestConnectionString";
            var defaultContainer = "TestContainer";

            // Act
            services.AddAzureStorageWrapper(connectionString, defaultContainer);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<AzureStorageWrapperOptions>();
            Assert.NotNull(options);
            Assert.Equal(connectionString, options.ConnectionString);
            Assert.Equal(defaultContainer, options.DefaultContainer);
        }

        [Fact]
        public void AddAzureStorageWrapper_WithOptions_ShouldConfigureServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var options = new AzureStorageWrapperOptions
            {
                ConnectionString = "TestConnectionString",
                DefaultContainer = "TestContainer",
                MaxSasUriExpiration = 600,
                DefaultSasUriExpiration = 300,
                CreateContainerIfNotExists = true
            };

            // Act
            services.AddAzureStorageWrapper(options);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var resolvedOptions = serviceProvider.GetService<AzureStorageWrapperOptions>();
            Assert.NotNull(resolvedOptions);
            Assert.Equal(options.ConnectionString, resolvedOptions.ConnectionString);
            Assert.Equal(options.DefaultContainer, resolvedOptions.DefaultContainer);
            Assert.Equal(options.MaxSasUriExpiration, resolvedOptions.MaxSasUriExpiration);
            Assert.Equal(options.DefaultSasUriExpiration, resolvedOptions.DefaultSasUriExpiration);
            Assert.Equal(options.CreateContainerIfNotExists, resolvedOptions.CreateContainerIfNotExists);
        }

        [Fact]
        public void AddAzureStorageWrapper_WithOptionsAction_ShouldConfigureServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddAzureStorageWrapper(options =>
            {
                options.ConnectionString = "TestConnectionString";
                options.DefaultContainer = "TestContainer";
                options.MaxSasUriExpiration = 600;
                options.DefaultSasUriExpiration = 300;
                options.CreateContainerIfNotExists = true;
            });

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<AzureStorageWrapperOptions>();
            Assert.NotNull(options);
            Assert.Equal("TestConnectionString", options.ConnectionString);
            Assert.Equal("TestContainer", options.DefaultContainer);
            Assert.Equal(600, options.MaxSasUriExpiration);
            Assert.Equal(300, options.DefaultSasUriExpiration);
            Assert.True(options.CreateContainerIfNotExists);
        }
    }
}
