using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using XRoadTestApp.Models;
using XRoadTestApp.Services;
using XRoadTestApp.XRoad.DTO.EMTA.TOR;
using XRoadTestApp.XRoad.Services.TorService;

namespace XRoadTestApp.XRoad.API.EMTA.TOR
{
    public class FindWork
    {
        private ITorService _torService;

        public FindWork(ITorService torService)
        {
            _torService = torService;
        }

        [FunctionName("FindWork")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "find-work")]
            HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Processing find work request");

                FindWorkRequest request;

                if (req.Method == "GET")
                {
                    // Handle GET request with query parameters
                    request = new FindWorkRequest
                    {
                        EmployerCode = req.Query["employerCode"],
                        QueryType = req.Query["queryType"].FirstOrDefault() ?? "A",
                        FromRecord = int.TryParse(req.Query["fromRecord"], out int fromRec) ? fromRec : 1,
                        UserId = req.Query["userId"]
                    };
                }
                else
                {
                    // Handle POST request with JSON body
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    request = JsonConvert.DeserializeObject<FindWorkRequest>(requestBody);
                }

                if (request == null || string.IsNullOrEmpty(request.EmployerCode))
                {
                    return new BadRequestObjectResult("EmployerCode is required");
                }

                // Call the X-Road service
                var response = await _torService.FindWorkAsync(
                    request.EmployerCode,
                    request.QueryType,
                    request.FromRecord,
                    request.UserId);

                log.LogInformation("Find work request completed successfully");
                return new OkObjectResult(new { success = true, response });
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error processing find work request");
                return new StatusCodeResult(500);
            }
        }
    }
}