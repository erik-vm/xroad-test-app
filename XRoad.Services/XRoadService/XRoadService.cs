using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XRoadTestApp.Models;
using XRoadTestApp.Services;
using XRoadTestApp.XRoad.Data.EMTA;

namespace XRoadTestApp.XRoad.Services.XRoadService
{
    

    public class XRoadService : IXRoadService
    {
        private readonly HttpClient _httpClient;
        private readonly XRoadConfig _config;
        private readonly ILogger<XRoadService> _logger;

        public XRoadService(IHttpClientFactory httpClientFactory, IOptions<XRoadConfig> config,
            ILogger<XRoadService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("XRoadClient");
            _config = config.Value;
            _logger = logger;

            // Debug logging
            _logger.LogInformation($"[SERVICE] XRoad Service initialized with URL: {_config.SecurityServerUrl}");
            _logger.LogInformation($"[SERVICE] SSL Errors Ignored: {_config.IgnoreSslErrors}");

            // Additional debug info
            Console.WriteLine($"[SERVICE] XRoadService constructor called");
            Console.WriteLine($"[SERVICE] HttpClient type: {_httpClient.GetType().Name}");
            Console.WriteLine($"[SERVICE] Config IgnoreSslErrors: {_config.IgnoreSslErrors}");
        }

        public async Task<string> SendRequestAsync(XRoadServiceInfo service, string soapBody, string userId = null)
        {
            try
            {
                var requestId = GenerateRequestId();
                var soapRequest = CreateSoapEnvelope(service, soapBody, requestId, userId);

                _logger.LogInformation($"Sending request to X-Road service: {service.ServiceCode}");
                _logger.LogDebug($"Request URL: {_config.SecurityServerUrl}");
                _logger.LogDebug($"Request ID: {requestId}");

                var response = await SendSoapRequestAsync(soapRequest);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending request to service: {service.ServiceCode}");
                throw;
            }
        }

        public async Task<string> TestConnectionAsync()
        {
            var soapBody = $@"<tns:TORRGNO>
                <request>
                    <paringu_liik>A</paringu_liik>
                    <tvoim_id>{_config.ClientMemberCode}</tvoim_id>
                    <alates_kandest>1</alates_kandest>
                </request>
            </tns:TORRGNO>";

            return await SendRequestAsync(Tor.TORRGNO, soapBody, "48906090292");
        }

       

        private async Task<string> SendSoapRequestAsync(string soapRequest)
        {
            try
            {
                _logger.LogDebug($"Sending SOAP request to: {_config.SecurityServerUrl}");

                var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
                content.Headers.Add("SOAPAction", "");

                var response = await _httpClient.PostAsync(_config.SecurityServerUrl, content);

                _logger.LogInformation($"Received response with status: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"HTTP Error: {response.StatusCode}, Content: {errorContent}");
                    throw new HttpRequestException($"Request failed with status {response.StatusCode}: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogDebug($"Response content length: {responseContent.Length}");
                return responseContent;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP request failed");
                throw;
            }
            catch (TaskCanceledException tcEx)
            {
                _logger.LogError(tcEx, "Request timed out");
                throw new TimeoutException("The request timed out", tcEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during SOAP request");
                throw;
            }
        }

        private string CreateSoapEnvelope(XRoadServiceInfo service, string soapBody, string requestId, string userId)
        {
            return $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" 
                   xmlns:id=""http://x-road.eu/xsd/identifiers"" 
                   xmlns:tns=""http://emta-v6.x-road.eu"" 
                   xmlns:xroad=""http://x-road.eu/xsd/xroad.xsd"">
    <SOAP-ENV:Header>
        <xroad:client id:objectType=""SUBSYSTEM"">
            <id:xRoadInstance>{_config.ClientInstance}</id:xRoadInstance>
            <id:memberClass>{_config.ClientMemberClass}</id:memberClass>
            <id:memberCode>{_config.ClientMemberCode}</id:memberCode>
            <id:subsystemCode>{_config.ClientSubsystemCode}</id:subsystemCode>
        </xroad:client>
        <xroad:service id:objectType=""SERVICE"">
            <id:xRoadInstance>{service.Instance}</id:xRoadInstance>
            <id:memberClass>{service.MemberClass}</id:memberClass>
            <id:memberCode>{service.MemberCode}</id:memberCode>
            <id:subsystemCode>{service.SubsystemCode}</id:subsystemCode>
            <id:serviceCode>{service.ServiceCode}</id:serviceCode>
            <id:serviceVersion>{service.ServiceVersion}</id:serviceVersion>
        </xroad:service>
        <xroad:id>{requestId}</xroad:id>
        <xroad:protocolVersion>{_config.ProtocolVersion}</xroad:protocolVersion>
        {(string.IsNullOrEmpty(userId) ? "" : $"<xroad:userId>{userId}</xroad:userId>")}
    </SOAP-ENV:Header>
    <SOAP-ENV:Body>
        {soapBody}
    </SOAP-ENV:Body>
</SOAP-ENV:Envelope>";
        }

        private string GenerateRequestId()
        {
            return $"req-{DateTime.Now:yyyyMMdd-HHmmss}-{Guid.NewGuid().ToString("N")[..8]}";
        }
    }
}