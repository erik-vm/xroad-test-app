using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using XRoadTestApp.Services;

namespace XRoadTestApp.XRoad.API.XRoad
{
    public class TestConnection
    {
        private IXRoadService _xroadService;

        public TestConnection(IXRoadService xroadService)
        {
            _xroadService = xroadService;
        }

        [FunctionName("TestConnection")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "test")]
            HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Testing X-Road connection...");

                var result = await _xroadService.TestConnectionAsync();

                return new OkObjectResult(new
                {
                    success = true,
                    message = "Connection successful",
                    response = result
                });
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Connection test failed");
                return new BadRequestObjectResult(new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }
    }
}