using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace samples
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly Samples_All _samplesAll;
        public Function1(ILogger<Function1> logger, Samples_All samplesAll)
        {
            _logger = logger;
            _samplesAll = samplesAll;
        }

        [Function("Function1")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            await _samplesAll.RunAsync();
            return new OkObjectResult("FIN");
        }
    }
}
