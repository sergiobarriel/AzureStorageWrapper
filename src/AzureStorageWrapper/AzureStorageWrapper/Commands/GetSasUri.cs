namespace AzureStorageWrapper.Commands
{
    public class GetSasUri
    {
        public string Folder { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Container { get; set; }
        public int ExpiresIn { get; set; }

        public string GeneratePath() => $"{Folder}/{Name}.{Extension}";
    }
}