namespace AzureStorageWrapper.Models
{
    public class GetSasUri
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Container { get; set; }
        public int ExpiresIn { get; set; }
        public string Path { get; set; }

        public string GetFilePath()
        {
            string name = $"{Name}.{Extension}";

            return $"{Path}/{name}";
        }
    }
}