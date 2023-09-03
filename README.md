# AzureStorageWrapper

**AzureStorageWrapper** it's a wrapper for Azure Storage **blob** service, aimed at simplifying the file upload process and obtaining links for downloading them.

[![run tests](https://github.com/sergiobarriel/azure.storage.wrapper/actions/workflows/runt-tests.yml/badge.svg?branch=dev)](https://github.com/sergiobarriel/azure.storage.wrapper/actions/workflows/run-tests.yml) 
[![run tests and deploy](https://github.com/sergiobarriel/azure.storage.wrapper/actions/workflows/run-tests-and-deploy.yml/badge.svg?branch=main)](https://github.com/sergiobarriel/azure.storage.wrapper/actions/workflows/run-tests-and-deploy.yml)

# Usage

## Dependency injection

There are many ways to add **AzureStorageWrapper** to dependencies container:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAzureStorageWrapper(new AzureStorageWrapperConfiguration(
    "azure-storage-connection-string",
    maxSasUriExpiration: 360, 
    defaultSasUriExpiration: 360));
```

```csharp
public void ConfigureServices(IServiceCollection serviceCollection)
{
    serviceCollection.AddAzureStorageWrapper(new AzureStorageWrapperConfiguration(
        "azure-storage-connection-string", 
        maxSasUriExpiration: 360, 
        defaultSasUriExpiration: 360));
}
```
## Upload blobs

You have **3 options** to upload blobs, and regardless of the chosen upload mechanism, you will always receive this response.

```csharp
public class BlobReference
{
    public string Name { get; set; }
    public string Extension { get; set; }
    public string FullName { get; set; }
    public string Container { get; set; }
    public string Uri { get; set; }
    public string SasUri { get; set; }
    public DateTime SasExpires { get; set; }
    public IDictionary<string, string> Metadata { get; set; }
}
```

### Base64

```csharp
var base64 = "SGVsbG8gd29ybGQh";

var command = new UploadBase64()
{
    Base64 = base64,
    Container = "greetings",
    Name = "greeting_01",
    Extension = "md",
    Metadata = new Dictionary<string, string>() { {"person", "Sergio"} }
};

var response = await _azureStorageWrapper.UploadBlobAsync(command);
```

### Byte []

```csharp
var bytes = Convert.FromBase64String("SGVsbG8gd29ybGQh");

var command = new UploadBytes()
{
    Bytes = bytes,
    Container = "greetings",
    Name = "greeting_02",
    Extension = "md",
    Metadata = new Dictionary<string, string>() { {"person", "Sergio"} }
};

var response = await _azureStorageWrapper.UploadBlobAsync(command);
```

### Stream

```csharp
var stream = new MemoryStream(Convert.FromBase64String("SGVsbG8gd29ybGQh"));

var command = new UploadStream()
{
    Stream = stream,
    Container = "greetings",
    Name = "greeting_03",
    Extension = "md",
    Metadata = new Dictionary<string, string>() { {"person", "Sergio"} }
};

var response = await _azureStorageWrapper.UploadBlobAsync(command);
```

## Download blobs

```csharp

var command = new DownloadBlobReference()
{
    Container = "greetings",
    Name = "greeting_01",
    Extension = "md",
    ExpiresIn = 360,
};

var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);

```