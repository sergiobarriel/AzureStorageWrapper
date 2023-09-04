namespace AzureStorageWrapper.Tests.Should
{
    public class BaseShould
    {
        protected static async Task<bool> PingAsync(string uri)
        {
            using var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(uri);

            var response = await httpClient.GetAsync(uri);

            return response.IsSuccessStatusCode;
        }

        public static IEnumerable<object[]> WrongMetadata() => new List<object[]>()
        {
            new object[] { new Dictionary<string, string>() },
            new object[] { null },
        };

        public static IEnumerable<object[]> WrongExpiresIn() => new List<object[]>()
        {
            new object[] { int.MinValue },
            new object[] { - 360 },
            new object[] { - 1 },
            new object[] { 0 },
            new object[] { null }
        };

        /// <summary>
        /// order: container, fileName, fileExtension
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> WrongFileProperties() => new List<object[]>()
        {
            new object[] { "", "", "" },
            new object[] { "greetings", "", "" },
            new object[] { "greetings", "hello", "" },
            new object[] { "", "hello", "md" },
            new object[] { "", "", "md" },
            new object[] { "greetings", "", "md" },
        };
    }
}