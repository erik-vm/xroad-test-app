using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using XRoadTestApp.XRoad.DTO.EMTA.TOR;
using XRoadTestApp.XRoad.DTO.Validation;
using XRoadTestApp.XRoad.Services.TorService.Interfaces;
using XRoadTestApp.XRoad.Services.TorService.Services;

namespace XRoadTestApp.XRoad.API.EMTA.TOR
{
    public class FindWork
    {
        
        private readonly IFindWorkService _findWorkService;

        public FindWork(IFindWorkService findWorkService)
        {
            _findWorkService = findWorkService ?? throw new ArgumentNullException(nameof(findWorkService));
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

                var request = await ParseRequestAsync(req);

                // Validate the request
                var validationResult = ValidateRequest(request);
                if (!validationResult.IsValid)
                {
                    return new BadRequestObjectResult(new FindWorkResponse
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = validationResult.Errors
                    });
                }

                string response = await ProcessSearchRequest(request, log);

                log.LogInformation("Find work request completed successfully");
                return new OkObjectResult(new FindWorkResponse
                {
                    Success = true,
                    Message = "Work search completed successfully",
                    Data = ParseSearchResponse(response) // This would need implementation
                });
            }
            catch (ArgumentException ex)
            {
                log.LogWarning(ex, "Invalid request parameters");
                return new BadRequestObjectResult(new FindWorkResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error processing find work request");
                return new StatusCodeResult(500);
            }
        }

        private async Task<FindWorkRequest> ParseRequestAsync(HttpRequest req)
        {
            FindWorkRequest request;

            if (req.Method == "GET")
            {
                // Handle GET request with query parameters
                request = new FindWorkRequest
                {
                    EmployerCode = req.Query["employerCode"].FirstOrDefault(),
                    QueryType = req.Query["queryType"].FirstOrDefault() ?? WorkSearchQueryTypes.Active,
                    FromRecord = int.TryParse(req.Query["fromRecord"], out int fromRec) ? fromRec : 1,
                    UserId = req.Query["userId"].FirstOrDefault()
                };

                // Parse work period dates for GET requests
                if (DateTime.TryParse(req.Query["workPeriodStart"], out DateTime startDate))
                    request.WorkPeriodStart = startDate;

                if (DateTime.TryParse(req.Query["workPeriodEnd"], out DateTime endDate))
                    request.WorkPeriodEnd = endDate;
            }
            else
            {
                // Handle POST request with JSON body
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                
                if (string.IsNullOrWhiteSpace(requestBody))
                {
                    throw new ArgumentException("Request body is required for POST requests");
                }

                try
                {
                    request = JsonConvert.DeserializeObject<FindWorkRequest>(requestBody);
                }
                catch (JsonException ex)
                {
                    throw new ArgumentException($"Invalid JSON in request body: {ex.Message}", ex);
                }
            }

            return request ?? throw new ArgumentException("Failed to parse request");
        }

        private async Task<string> ProcessSearchRequest(FindWorkRequest request, ILogger log)
        {
            // Normalize query type
            var queryType = request.QueryType?.ToUpper() ?? WorkSearchQueryTypes.Active;

            log.LogInformation($"Processing {WorkSearchQueryTypes.GetDescription(queryType)} search for employer {request.EmployerCode}");

            return queryType switch
            {
                WorkSearchQueryTypes.Active => 
                    await _findWorkService.FindActiveWorkAsync(request.EmployerCode, request.FromRecord, request.UserId),
                
                WorkSearchQueryTypes.All => 
                    await _findWorkService.FindAllWorkAsync(request.EmployerCode, request.FromRecord, request.UserId),
                
                WorkSearchQueryTypes.Period when request.WorkPeriodStart.HasValue && request.WorkPeriodEnd.HasValue => 
                    await _findWorkService.FindWorkByPeriodAsync(
                        request.EmployerCode, 
                        request.WorkPeriodStart.Value, 
                        request.WorkPeriodEnd.Value, 
                        request.FromRecord, 
                        request.UserId),
                
                _ => throw new ArgumentException($"Invalid or incomplete request for query type {queryType}")
            };
        }

        private (bool IsValid, List<ValidationError> Errors) ValidateRequest(FindWorkRequest request)
        {
            var errors = new List<ValidationError>();

            // Employer code is mandatory
            if (string.IsNullOrWhiteSpace(request.EmployerCode))
            {
                errors.Add(new ValidationError
                {
                    Field = nameof(request.EmployerCode),
                    Code = "EMPLOYER_CODE_REQUIRED",
                    Message = "EmployerCode is required",
                    Type = "Error"
                });
            }

            // Validate query type
            if (!string.IsNullOrEmpty(request.QueryType) && !WorkSearchQueryTypes.IsValid(request.QueryType))
            {
                errors.Add(new ValidationError
                {
                    Field = nameof(request.QueryType),
                    Code = "INVALID_QUERY_TYPE",
                    Message = $"QueryType must be one of: {string.Join(", ", WorkSearchQueryTypes.ValidQueryTypes)}",
                    Type = "Error"
                });
            }

            // For period queries, both dates are required
            if (request.QueryType?.ToUpper() == WorkSearchQueryTypes.Period)
            {
                if (!request.WorkPeriodStart.HasValue)
                {
                    errors.Add(new ValidationError
                    {
                        Field = nameof(request.WorkPeriodStart),
                        Code = "WORK_PERIOD_START_REQUIRED",
                        Message = "WorkPeriodStart is required when QueryType is P (Period)",
                        Type = "Error"
                    });
                }

                if (!request.WorkPeriodEnd.HasValue)
                {
                    errors.Add(new ValidationError
                    {
                        Field = nameof(request.WorkPeriodEnd),
                        Code = "WORK_PERIOD_END_REQUIRED",
                        Message = "WorkPeriodEnd is required when QueryType is P (Period)",
                        Type = "Error"
                    });
                }

                // Validate date range
                if (request.WorkPeriodStart.HasValue && request.WorkPeriodEnd.HasValue && 
                    request.WorkPeriodEnd < request.WorkPeriodStart)
                {
                    errors.Add(new ValidationError
                    {
                        Field = nameof(request.WorkPeriodEnd),
                        Code = "INVALID_DATE_RANGE",
                        Message = "WorkPeriodEnd cannot be earlier than WorkPeriodStart",
                        Type = "Error"
                    });
                }
            }

            // Validate FromRecord
            if (request.FromRecord < 1)
            {
                errors.Add(new ValidationError
                {
                    Field = nameof(request.FromRecord),
                    Code = "INVALID_FROM_RECORD",
                    Message = "FromRecord must be greater than 0",
                    Type = "Error"
                });
            }

            // Business logic validations
            if (request.FromRecord > 10000)
            {
                errors.Add(new ValidationError
                {
                    Field = nameof(request.FromRecord),
                    Code = "FROM_RECORD_TOO_LARGE",
                    Message = "FromRecord cannot exceed 10000 for performance reasons",
                    Type = "Warning"
                });
            }

            return (errors.Count(e => e.Type == "Error") == 0, errors);
        }

        private WorkSearchData ParseSearchResponse(string xmlResponse)
        {
            // This would contain XML parsing logic to convert the SOAP response
            // to the structured WorkSearchData model
            // Implementation would depend on your XML parsing strategy
            
            // Placeholder implementation
            return new WorkSearchData
            {
                // Parse XML response and populate the model
                WorkRecords = new List<WorkRecord>()
            };
        }
    }  
    }
