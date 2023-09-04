![AzureStorageWrapper poster](https://raw.githubusercontent.com/sergiobarriel/AzureStorageWrapper/main/images/poster.png)

# AzureStorageWrapper

**AzureStorageWrapper** it's a wrapper for Azure Storage **blob** service, aimed at simplifying the file upload process and obtaining links for downloading them.

[![run tests](https://github.com/sergiobarriel/AzureStorageWrapper/actions/workflows/run-tests.yml/badge.svg?branch=dev)](https://github.com/sergiobarriel/AzureStorageWrapper/actions/workflows/run-tests.yml) 
[![run tests and deploy](https://github.com/sergiobarriel/AzureStorageWrapper/actions/workflows/run-tests-and-deploy.yml/badge.svg?branch=main)](https://github.com/sergiobarriel/AzureStorageWrapper/actions/workflows/run-tests-and-deploy.yml)

📦 View package on [NuGet Gallery](https://www.nuget.org/packages/AzureStorageWrapper/)

# Usage

## Dependency injection

To add **AzureStorageWrapper** to dependencies container, just use the method `AddAzureStorageWrapper`

```csharp
public void ConfigureServices(IServiceCollection serviceCollection)
{
    serviceCollection.AddAzureStorageWrapper(configuration =>
    {
        configuration.ConnectionString = "azure-storage-connection-string"
        configuration.DefaultSasUriExpiration = 360;
        configuration.MaxSasUriExpiration = 360;
        configuration.CreateContainerIfNotExists = true;
    });
}
```

These are the main properties:
- **ConnectionString**: The connection string of your Azure Storage Account. You can export by following [this document](https://learn.microsoft.com/en-us/azure/storage/common/storage-account-keys-manage?tabs=azure-portal#view-account-access-keys)
- **MaxSasUriExpiration**: You can set a maximum duration value for the Shared Access Signature (SAS) of an Azure Storage file to prevent someone from attempting to generate a token with a longer expiration time.
- **DefaultSasUriExpiration**: You can download a file using AzureStorageWrapper without specifying the `ExpiresIn` property. By doing so, this value will be automatically set.
- **CreateContainerIfNotExists**: When uploading a file to Azure Storage, you need to specify the container, which may not exist and can be created automatically. You can set it to `true` or `false` based on your requirements. Please consider this property if you have automated your infrastructure with any Infrastructure as Code (IaC) mechanism because it affects the state of your infrastructure.

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
    public string Folder { get;set; }
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

In example, if you upload the file `greeting.md` file to container `greetings` you will receive a response like:

```json
{
    "Container": "greetings",
    "Folder": "5a19306fc5014a4",
    "Name": "greeting",
    "Extension": "md",
    "FullName": "5a19306fc5014a4/greeting.md",
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

It is your responsibility to save the references (folder, file name, and extension) of the file you have uploaded to Azure Storage somewhere, as you will need it for later downloads.

## Download blobs

To download a blob reference, you need specify the *container*, the *file name* and *extension*.

The *Folder* it's mandatory.

```csharp

var command = new DownloadBlobReference()
{
    Container = "greetings",
    Folder = "04bc4c89e547478",
    Name = "greeting",
    Extension = "md",
    ExpiresIn = 360,
};

var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);

```

The response when *downloading* file reference resembles the response when *uploading* files:

```json
{
    "Container": "greetings",
    "Folder": "5a19306fc5014a4",
    "Name": "greeting",
    "Extension": "md",
    "FullName": "5a19306fc5014a4/greeting.md",
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

With **AzureStorageWrapper**, you can download files that haven't been uploaded using this tool. You just need to make some adjustments when downloading. Simply modify the *Folder* parameter and set the file path as it exists in your container.

In example, if you have saved invoices inside a container named *invoices* and you have virtual folders like *2020/08* you can donwload as:

```csharp

var command = new DownloadBlobReference()
{
    Container = "invoices",
    Folder = "2020/08",
    Name = "file",
    Extension = "pdf",
    ExpiresIn = 360,
};

var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(command);

```

# Support

You can contact me via Twitter [@sergiobarriel](https://twitter.com/sergiobarriel), or if you have an [issue](https://github.com/sergiobarriel/AzureStorageWrapper/issues), you can open one 🙂