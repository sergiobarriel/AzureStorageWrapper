![AzureStorageWrapper poster](https://raw.githubusercontent.com/sergiobarriel/AzureStorageWrapper/main/images/poster.png)

# AzureStorageWrapper

**AzureStorageWrapper** it's a wrapper for Azure Storage **blob** service, aimed at simplifying the file upload process and obtaining links for downloading them.

[![run tests](https://github.com/sergiobarriel/AzureStorageWrapper/actions/workflows/run-tests.yml/badge.svg?branch=dev)](https://github.com/sergiobarriel/AzureStorageWrapper/actions/workflows/run-tests.yml) 
[![run tests and deploy](https://github.com/sergiobarriel/AzureStorageWrapper/actions/workflows/run-tests-and-deploy.yml/badge.svg?branch=main)](https://github.com/sergiobarriel/AzureStorageWrapper/actions/workflows/run-tests-and-deploy.yml)
![downloads](https://img.shields.io/nuget/dt/AzureStorageWrapper)

📦 View package on [NuGet Gallery](https://www.nuget.org/packages/AzureStorageWrapper/)
📦 View package on [nuget.info](https://nuget.info/packages/AzureStorageWrapper)

# Usage

## Dependency injection

To add **AzureStorageWrapper** to dependencies container, just use the method `AddAzureStorageWrapper(...)`

```csharp
public void ConfigureServices(IServiceCollection serviceCollection)
{
    serviceCollection.AddAzureStorageWrapper(configuration =>
    {
        configuration.ConnectionString = "azure-storage-connection-string"
        configuration.MaxSasUriExpiration = 600;
        configuration.DefaultSasUriExpiration = 300;
        configuration.CreateContainerIfNotExists = true;
    });
}
```

These are the *main* properties:
- **ConnectionString**: The connection string of your Azure Storage Account. You can export by following [this document](https://learn.microsoft.com/en-us/azure/storage/common/storage-account-keys-manage?tabs=azure-portal#view-account-access-keys)
- **MaxSasUriExpiration**: You can set a maximum duration value for the Shared Access Signature (SAS) of an Azure Storage file to prevent someone from attempting to generate a token with a longer expiration time. Expressed in seconds.
- **DefaultSasUriExpiration**: You can download a file using AzureStorageWrapper without specifying the `ExpiresIn` property. By doing so, this value will be automatically set. Expressed in seconds
- **CreateContainerIfNotExists**: When uploading a file to Azure Storage, you need to specify the container, which may not exist and can be created automatically. You can set it to `true` or `false` based on your requirements. Please consider this property if you have automated your infrastructure with any Infrastructure as Code (IaC) mechanism because it affects the state of your infrastructure.

Then you can inject `IAzureStorageWrapper` into your services through constructor:

```csharp
public class MyService
{
    private IAzureStorageWrapper _storageWrapper;

    public MyService(IAzureStorageWrapper storageWrapper)
    {
        _storageWrapper = storageWrapper;
    }
}
```

## Upload blobs

There are **3 options** to upload blobs, all the ways follow the same pattern:

- You need to specify the *file name* and *extension*.
- You need to specify the *container* where you want to store the file.
- You can add additional *metadata* with relevant information.

The file will be placed in *Base64*, *Bytes* or *Stream* property.

### Base64

```csharp
var base64 = "SGVsbG8g8J+Zgg==";

var command = new UploadBase64()
{
    Base64 = base64,
    Container = "greetings",
    Name = "greeting",
    Extension = "md",
    Metadata = new Dictionary<string, string>() { {"GREETING_PLACE", "Office"} }
};

var response = await _azureStorageWrapper.UploadBlobAsync(command);
```

### Byte []

```csharp
var bytes = Convert.FromBase64String("SGVsbG8g8J+Zgg==");

var command = new UploadBytes()
{
    Bytes = bytes,
    Container = "greetings",
    Name = "greeting",
    Extension = "md",
    Metadata = new Dictionary<string, string>() { {"GREETING_PLACE", "Street"} }
};

var response = await _azureStorageWrapper.UploadBlobAsync(command);
```

### Stream

```csharp
var stream = new MemoryStream(Convert.FromBase64String("SGVsbG8g8J+Zgg=="));

var command = new UploadStream()
{
    Stream = stream,
    Container = "greetings",
    Name = "greeting",
    Extension = "md",
    Metadata = new Dictionary<string, string>() { {"GREETING_PLACE", "Park"} }
};

var response = await _azureStorageWrapper.UploadBlobAsync(command);
```

## Response after upload blobs

Regardless of the chosen upload mechanism, you will always receive this response after upload a file.

```csharp
public class BlobReference
{
    public string Container { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public string Uri { get; set; }
    public string SasUri { get; set; }
    public DateTime SasExpires { get; set; }
    public IDictionary<string, string> Metadata { get; set; }
}
```

In example, if you upload the file `greeting.md` file to container `greetings` you will receive a response like:

```json
{
    "Container": "greetings",
    "Name": "greeting",
    "Extension": "md",
    "Uri": "https://stgazstgwrapper001westeu.blob.core.windows.net/tests/5a19306fc5014a4/greeting.md",
    "SasUri": "https://stgazstgwrapper001westeu.blob.core.windows.net/tests/5a19306fc5014a4/greeting.md?sv=2021-10-04\u0026se=2023-09-03T16%3A17%3A02Z\u0026sr=b\u0026sp=r\u0026sig=8hs8AzxABevSTc5y%2BhOWDDN%2FH5qFSpA8Omj4uqoxzms%3D",
    "SasExpires": "2023-09-03T16:17:02.8220993Z",
    "Metadata": {
        "GREETING_PLACE": "Office",
        "ASW_FOLDER": "5a19306fc5014a4",
        "ASW_TIMESTAMP": "03/09/2023 16:11:02"
    }
}
```

It is your responsibility to save the reference (URI property) of the file you have uploaded to Azure Storage somewhere, as you will need it for later downloads.

## Download blobs

To download a blob reference, you need specify the *Uri*.

The *Folder* it's mandatory.

```csharp
var command = new DownloadBlobReference()
{
    Uri = "https://stgazstgwrapper001westeu.blob.core.windows.net/tests/5a19306fc5014a4/greeting.md"
    ExpiresIn = 60,
};

var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);
```

The response when *downloading* file reference resembles the response when *uploading* files:

```json
{
    "Container": "greetings",
    "Name": "greeting",
    "Extension": "md",
    "Uri": "https://stgazstgwrapper001westeu.blob.core.windows.net/tests/5a19306fc5014a4/greeting.md",
    "SasUri": "https://stgazstgwrapper001westeu.blob.core.windows.net/tests/5a19306fc5014a4/greeting.md?sv=2021-10-04\u0026se=2023-09-03T16%3A17%3A02Z\u0026sr=b\u0026sp=r\u0026sig=8hs8AzxABevSTc5y%2BhOWDDN%2FH5qFSpA8Omj4uqoxzms%3D",
    "SasExpires": "2023-09-03T16:17:02.8220993Z",
    "Metadata": {
        "GREETING_PLACE": "Office",
        "ASW_FOLDER": "5a19306fc5014a4",
        "ASW_TIMESTAMP": "03/09/2023 16:11:02"
    }
}
```
# Sponsor

If you like the project, you can consider making a donation at [ko-fi.com](https://ko-fi.com/sergiobarriel)

<a href='https://ko-fi.com/W7W6O05JU' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://storage.ko-fi.com/cdn/kofi1.png?v=3' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a>

# Support

You can contact me via Twitter [@sergiobarriel](https://twitter.com/sergiobarriel), or if you have an [issue](https://github.com/sergiobarriel/AzureStorageWrapper/issues), you can open one 🙂
