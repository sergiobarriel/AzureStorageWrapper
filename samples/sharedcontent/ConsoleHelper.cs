using System;
using System.Security.Cryptography;
using Newtonsoft.Json;

#if NETCOREAPP
using System.Text.Json;
#elif NETFRAMEWORK

#endif

namespace samples.Helpers
{
    public static class ConsoleHelper
    {
        private const ConsoleColor _blockColor = ConsoleColor.Cyan;
        private const ConsoleColor _resultColor = ConsoleColor.Green;
        private const ConsoleColor _moduleColor = ConsoleColor.Yellow;
        private const ConsoleColor _infoColor = ConsoleColor.White;

#if NET
        private static JsonSerializerOptions _jsonSerializerOptions => new JsonSerializerOptions() { WriteIndented = true };
#elif NETFRAMEWORK
        private static JsonSerializerSettings _jsonSerializerSettings => new JsonSerializerSettings { Formatting = Formatting.Indented };
#endif

        public static void Info(string message) => WriteImpl(message, _infoColor);
        public static void Module(string message) => WriteImpl($"\r\n{message}\r\n", _moduleColor);

        public static void Start(string message) => WriteImpl($"\r\n=== {message} ===", _blockColor);
        public static void Finalized(string message) => WriteImpl($"=== FIN - {message} ===", _blockColor);

        public static void Result(object value)
        {
#if NET
            var message = $"Result: {JsonSerializer.Serialize(value, _jsonSerializerOptions)}";
#elif NETFRAMEWORK
            var message = $"Result:  {JsonConvert.SerializeObject(value, _jsonSerializerSettings)}";
#endif
            Result(message);
        }
        public static void Result(string message) => WriteImpl(message, _resultColor);



        private static void WriteImpl(string message, ConsoleColor color, bool isLine = true)
        {
            Console.ForegroundColor = color;
            if (isLine)
                Console.WriteLine(message);
            else
                Console.Write(message);
            Console.ResetColor();
        }
    }
}
