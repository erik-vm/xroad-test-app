using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using XRoadTestApp.Models;
using XRoadTestApp.Services;
using XRoadTestApp.XRoad.Services.TorService;

namespace XRoadTestApp.XRoad.API.EMTA.TOR
{
    public class RegisterWork
    {
        private ITorService _torService;

        public RegisterWork(ITorService torService)
        {
            _torService = torService;
        }

        [FunctionName("RegisterWork")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "register-work")]
            HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Processing work registration request");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var request = JsonConvert.DeserializeObject<RegisterWorkRequest>(requestBody);

                if (request == null)
                {
                    return new BadRequestObjectResult("Invalid request body");
                }

                // Validate required fields
                if (string.IsNullOrEmpty(request.PersonalCode))
                {
                    return new BadRequestObjectResult("PersonalCode is required");
                }

                if (request.StartDate == default(DateTime))
                {
                    return new BadRequestObjectResult("StartDate is required");
                }

                // Call the X-Road service
                var response = await _torService.RegisterWorkAsync(
                    request.PersonalCode,
                    request.StartDate,
                    request.WorkTimeRatio,
                    request.UserId);

                log.LogInformation("Work registration completed successfully");
                return new OkObjectResult(new { success = true, response });
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error processing work registration");
                return new StatusCodeResult(500);
            }
        }
    }
}