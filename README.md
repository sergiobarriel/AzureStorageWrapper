![AzureStorageWrapper poster](https://raw.githubusercontent.com/sergiobarriel/AzureStorageWrapper/main/images/poster.png)

# AzureStorageWrapper

**AzureStorageWrapper** it's a wrapper for Azure Storage **blob** service, aimed at simplifying the file upload process and obtaining links for downloading them.

[![run tests](https://github.com/sergiobarriel/AzureStorageWrapper/actions/workflows/run-tests.yml/badge.svg?branch=dev)](https://github.com/sergiobarriel/AzureStorageWrapper/actions/workflows/run-tests.yml) 
[![run tests and deploy](https://github.com/sergiobarriel/AzureStorageWrapper/actions/workflows/run-tests-and-deploy.yml/badge.svg?branch=main)](https://github.com/sergiobarriel/AzureStorageWrapper/actions/workflows/run-tests-and-deploy.yml)
![downloads](https://img.shields.io/nuget/dt/AzureStorageWrapper)

📦 View package on [NuGet Gallery](https://www.nuget.org/packages/AzureStorageWrapper/)
📦 View package on [nuget.info](https://nuget.info/packages/AzureStorageWrapper)

## Supported framework

* .Net Standard 2.0
* .Net Standard 2.1

## Installation the package
Install the AzureStorageWrapper client library for .NET with NuGet:

```dotnetcli
dotnet add package AzureStorageWrapper
 ```

``` nuget
NuGet Install-Package AzureStorageWrapper -Version x.y.z
```
This command is intended to be used within the Package Manager Console in Visual Studio, as it uses the NuGet module's version of [Install-Package](https://learn.microsoft.com/es-es/nuget/reference/ps-reference/ps-ref-install-package).

``` 
<PackageReference Include="AzureStorageWrapper" Version="x.y.z" />
```
 For projects that support [PackageReference](https://learn.microsoft.com/es-es/nuget/consume-packages/package-references-in-project-files), copy this XML node into the project file to reference the package.

# Usage

## Dependency injection

### File configuration
#### local.settings.json (Azure Function)
```json
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
        
        //Configuration StorageWrapper
        StorageWrapper_ConnectionsString="..." //Is a configuration setting that contains the necessary information to connect to your Azure Storage account. Type: String. Recomended
        StorageWrapper_MaxSasUriExpiration="..." //It sets the maximum duration for the Shared Access Signature (SAS) URI of an Azure Storage file. Type: int. Default: 600. Optional
        StorageWrapper_DefaultSasUriExpiration="..." //It sets the default duration for the Shared Access Signature (SAS) URI of an Azure Storage file. Type: int. Default: 300. Optional
        StorageWrapper_ReateContainerIfNotExists="..." //Determines whether the Azure Storage container should be automatically created if it does not already exist.Type: bool. Defult: true. Optional
    }
}
```
#### app.settings.json 
```json
{
    //Configuration StorageWrapper
        //Configuration StorageWrapper
        StorageWrapper_ConnectionsString="..." //Is a configuration setting that contains the necessary information to connect to your Azure Storage account. Type: String. Recomended
        StorageWrapper_MaxSasUriExpiration="..." //It sets the maximum duration for the Shared Access Signature (SAS) URI of an Azure Storage file. Type: int. Default: 600. Optional
        StorageWrapper_DefaultSasUriExpiration="..." //It sets the default duration for the Shared Access Signature (SAS) URI of an Azure Storage file. Type: int. Default: 300. Optional
        StorageWrapper_ReateContainerIfNotExists="..." //Determines whether the Azure Storage container should be automatically created if it does not already exist.Type: bool. Defult: true. Optional
}
```

> **Remember** that the variable names are suggested and you can change them according to your needs.

### Minimal Configuration
To add **AzureStorageWrapper** to dependencies container.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAzureStorageWrapper();
```
> **Warning:** You must configure the **StorageWrapper_ConnectionString** variable in the configuration file.

### ConnectionString Configuration
To add **AzureStorageWrapper** to dependencies container, just use the method `AddAzureStorageWrapper(string connectionstring)`

```csharp
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["Connection_String"];
builder.Services.AddAzureStorageWrapper(connectionString);
```
### Configuration
To add **AzureStorageWrapper** to dependencies container, just use the method `AddAzureStorageWrapper(...)`

```csharp
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["Connection_String"];
builder.Services.AddAzureStorageWrapper(options =>
{
    options.ConnectionString = "azure-storage-connection-string";
    options.MaxSasUriExpiration = 600;
    options.DefaultSasUriExpiration = 300;
    options.CreateContainerIfNotExists = true;
});
```

These are the *main* properties:
- **ConnectionString**: The connection string of your Azure Storage Account. You can export it by following [this documentation](https://learn.microsoft.com/en-us/azure/storage/common/storage-account-keys-manage?tabs=azure-portal#view-account-access-keys)
- **MaxSasUriExpiration**: You can set a maximum duration value for the Shared Access Signature (SAS) of an Azure Storage file to prevent someone from attempting to generate a token with a longer expiration time. The duration is expressed in seconds.
- **DefaultSasUriExpiration**: You can download a file using AzureStorageWrapper without specifying the `ExpiresIn` property. By doing so, this value will be automatically set. The duration is xpressed in seconds
- **CreateContainerIfNotExists**: When uploading a file to Azure Storage, you need to specify the container, which may not exist and can be created automatically. You can set it to `true` or `false` based on your requirements. Consider this property if you have automated your infrastructure with an Infrastructure as Code (IaC) mechanism because it affects the state of your infrastructure.

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

- You need to specify the `Name` and `Extension`.
- You need to specify the `Container` where you want to store the file.
- You can add additional `Metadata` with relevant information.

The file will be placed in `Base64`, `Bytes` or `Stream` property.

Optionally you can specify the `UseVirtualFolder` property to save the file in a virtual folder. By default, it is set to `true`. We delve deeper into this point later.

### Base64

```csharp
var base64 = "SGVsbG8g8J+Zgg==";

var command = new UploadBase64()
{
    Base64 = base64,
    Container = "files",
    Name = "hello",
    Extension = "md",
    Metadata = new Dictionary<string, string>() { {"key", "value"} }
};

var response = await _azureStorageWrapper.UploadBlobAsync(command);
```

### Byte []

```csharp
var bytes = Convert.FromBase64String("SGVsbG8g8J+Zgg==");

var command = new UploadBytes()
{
    Bytes = bytes,
    Container = "files",
    Name = "hello",
    Extension = "md",
    Metadata = new Dictionary<string, string>() { {"key", "value"} }
};

var response = await _azureStorageWrapper.UploadBlobAsync(command);
```

### Stream

```csharp
var stream = new MemoryStream(Convert.FromBase64String("SGVsbG8g8J+Zgg=="));

var command = new UploadStream()
{
    Stream = stream,
    Container = "files",
    Name = "hello",
    Extension = "md",
    Metadata = new Dictionary<string, string>() { {"key", "value"} }
};

var response = await _azureStorageWrapper.UploadBlobAsync(command);
```

## Response after upload blobs

Regardless of the chosen upload mechanism, you will always receive a `BlobReference` object after uploading a file.

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

In example, if you upload the file `hello.md` file to container `files` you will receive a response like:

```json
{
    "Container": "files",
    "Name": "hello",
    "Extension": "md",
    "Uri": "https://accountName.blob.core.windows.net/files/5a19306fc5014a4/hello.md",
    "SasUri": "https://accountName.blob.core.windows.net/files/5a19306fc5014a4/hello.md?sv=2021-10-04\u0026se=2023-09-03T16%3A17%3A02Z\u0026sr=b\u0026sp=r\u0026sig=8hs8AzxABevSTc5y%2BhOWDDN%2FH5qFSpA8Omj4uqoxzms%3D",
    "SasExpires": "2023-09-03T16:17:02.8220993Z",
    "Metadata": {
        "key": "value",
        "_timestamp": "03/09/2023 16:11:02"
    }
}
```

> It is your responsibility to save the reference (URI property) of the file you have uploaded to Azure Storage somewhere, as you will need it for later downloads.

## Virtual Folders

By default, files are stored in the desired container using **virtual folders**, allowing you to upload files with the same name without risking name collisions.

For example, a virtual folder with a unique identifier is automatically created between the container name `files` and the file name `hello.md`, resulting in a URI like this:

`https://accountName.blob.core.windows.net/files/5a19306fc5014a4/hello.md`

However, you can customize the `UseVirtualFolder` property, which by default has a value of `true` but you can set it to `false` if you wish.

> ⚠️ When `UseVirtualFolder` is set to `false`, files will **NOT** be stored in virtual directories. This change may lead to file name collisions, causing files to be **overwritten**.

In this scenario, you must implement your own mechanism to generate unique file names.

```csharp
var base64 = "SGVsbG8g8J+Zgg==";

var command = new UploadBase64()
{
    Base64 = base64,
    Container = "files",
    Name = $"{Guid.NewGuid()}",
    Extension = "md",
    UseVirtualFolder = false // be careful!
};

var response = await _azureStorageWrapper.UploadBlobAsync(command);
```

## Download blob references

To download a blob reference, you need to specify the `Uri`, which you should have stored in your system in some way

```csharp
var query = new DownloadBlobReference()
{
    Uri = "https://accountName.blob.core.windows.net/files/5a19306fc5014a4/hello.md"
    ExpiresIn = 60,
};

var response = await _azureStorageWrapper.DownloadBlobReferenceAsync(query);
```

The response when *downloading* file reference resembles the response when *uploading* files:

```json
{
    "Container": "files",
    "Name": "hello",
    "Extension": "md",
    "Uri": "https://accountName.blob.core.windows.net/files/5a19306fc5014a4/hello.md",
    "SasUri": "https://accountName.blob.core.windows.net/files/5a19306fc5014a4/hello.md?sv=2021-10-04\u0026se=2023-09-03T16%3A17%3A02Z\u0026sr=b\u0026sp=r\u0026sig=8hs8AzxABevSTc5y%2BhOWDDN%2FH5qFSpA8Omj4uqoxzms%3D",
    "SasExpires": "2023-09-03T16:17:02.8220993Z",
    "Metadata": {
        "_timestamp": "03/09/2023 16:11:02"
    }
}
```

## Delete blobs

You can delete a blob by specifying the `Uri`.

```csharp
var command = new DeleteBlob()
{
    Uri = "https://accountName.blob.core.windows.net/files/5a19306fc5014a4/hello.md"
};

await _azureStorageWrapper.DeleteBlobAsync(command);
```

## Enumerate blobs

You can list all blobs in a container by using the method `EnumerateBlobsAsync`. 

The response `BlobReferenceCollection` will contain a collection of `BlobReference` elements.

### Without pagination

You should only run this query if you are certain that your container stores a small number of blobs.

```csharp
var query = new EnumerateBlobs()
{
    Container = "files",
    Paginate = false
};

var response = await _azureStorageWrapper.EnumerateAllBlobsAsync(query);
```

### With pagination

```csharp
var query = new EnumerateBlobs()
{
    Container = "files",
    Paginate = true.
    Size = 10,
};

var response = await _azureStorageWrapper.EnumerateBlobsAsync(query);

```
Then you can request additional pages by using the `ContinuationToken` property in the next request.

```csharp
var firstQuery = new EnumerateBlobs()
{
    Container = "files",
    Paginate = true,
    Size = 10,
};

var firstResponse = await _azureStorageWrapper.EnumerateBlobsAsync(firstQuery);

var secondQuery = new EnumerateBlobs()
{
    Container = "files",
    Paginate = true,
    Size = 10,
    ContinuationToken = firstResponse.ContinuationToken
};

var secondResponse = await _azureStorageWrapper.EnumerateBlobsAsync(secondQuery);
```

# Contributors / Collaborators

These individuals have contributed to the repository through suggestions, error corrections, or by opening issues. Thanks 😊

- [ginodcs](https://github.com/ginodcs)
- [christian-cell](https://github.com/christian-cell)
- [scabrera](https://github.com/scabrera)

# Sponsor

If you like the project, you can consider making a donation at [ko-fi.com](https://ko-fi.com/sergiobarriel)

# Support

You can contact me via Bluesky [@sergiobarriel.bsky.social](https://bsky.app/profile/sergiobarriel.bsky.social) or Twitter [@sergiobarriel](https://twitter.com/sergiobarriel), or if you have an [issue](https://github.com/sergiobarriel/AzureStorageWrapper/issues), you can open one 🙂

