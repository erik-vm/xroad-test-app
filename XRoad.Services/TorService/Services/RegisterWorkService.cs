using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using XRoadTestApp.XRoad.Core.Constants.ServicesInfo.EMTA;
using XRoadTestApp.XRoad.DTO.EMTA.TOR;
using XRoadTestApp.XRoad.DTO.Shared;
using XRoadTestApp.XRoad.Services.SoapService.Interfaces;
using XRoadTestApp.XRoad.Services.TorService.Interfaces;
using XRoadTestApp.XRoad.Services.XRoadService.Interface;

namespace XRoadTestApp.XRoad.Services.TorService.Services
{
    public class RegisterWorkService : IRegisterWorkService
    {
        private readonly IXRoadService _xRoadService;
        private readonly ISoapService _soapService;

        public RegisterWorkService(IXRoadService xRoadService, ISoapService soapService)
        {
            _xRoadService = xRoadService ?? throw new ArgumentNullException(nameof(xRoadService));
            _soapService = soapService ?? throw new ArgumentNullException(nameof(soapService));
        }


        /// <summary>
        /// Full registration method supporting multiple records and all fields
        /// </summary>
        public async Task<string> RegisterWorkAsync(List<RegisterWorkRequest> requests, string userId)
        {
            if (requests == null || !requests.Any())
                throw new ArgumentException("At least one work registration request is required");

            var config = new SoapBodyConfig
            {
                RootElement = "tns:EMPLOYEES",
                ItemsContainerElement = "employee_list"
            };

            var soapBody = _soapService.BuildSoapBody(requests, config, employee => new Dictionary<string, object>
            {
                ["emp_id"] = employee.PersonalCode
            });
            return await _xRoadService.SendRequestAsync(Tor.TOOTREG, soapBody, userId);
        }


        private Dictionary<string, object> MapWorkRequestFields(RegisterWorkRequest request)
        {
            return new Dictionary<string, object>
            {
                ["toot_id"] = request.WorkId,
                ["kande_tyyp"] = request.RecordType,
                ["kande_aeg"] = request.StartDate?.ToString("yyyy-MM-ddTHH:mm:ss"),
                ["isikukood"] = request.PersonalCode,
                ["tootaja_synniaeg"] = request.BirthDate?.ToString("yyyy-MM-dd"),
                ["tootaja_perenimi"] = request.LastName,
                ["tootaja_eesnimi"] = request.FirstName,
                ["tootamise_algus"] = request.StartDate?.ToString("yyyy-MM-dd"),
                ["tootamise_liik"] = request.EmploymentType,
                ["tooaja_maar"] = request.WorkTimeRatio.ToString("F1", CultureInfo.InvariantCulture),
                ["leping"] = request.Contract,
                ["tegeliku_tooandja_kood"] = request.ActualEmployerCode,
                ["ameti_kood"] = request.OccupationCode,
                ["tootamise_asukoha_aadressi_ID"] = request.WorkLocationAddressId,
                ["imo"] = request.ImoCode,
                ["tootamise_lopp"] = request.EndDate?.ToString("yyyy-MM-dd"),
                ["tootamise_lopetamise_aluse_kood"] = request.TerminationReasonCode,
                ["peatamise_perioodi_algus"] = request.SuspensionStartDate?.ToString("yyyy-MM-dd"),
                ["esmane_peatamise_algus"] = request.InitialSuspensionStartDate?.ToString("yyyy-MM-dd"),
                ["peatamise_aluse_kood"] = request.SuspensionReasonCode,
                ["peatamise_perioodi_lopp"] = request.SuspensionEndDate?.ToString("yyyy-MM-dd"),
                ["tvoim_markus"] = request.WorkProviderRemarks,
                ["riigi_kood"] = request.CountryCode,
                ["rka_marge"] = request.DefenseServiceMark,
                ["tka_marge"] = request.BackgroundCheckMark,
                ["amet_vabatekstina"] = request.OccupationFreeText,
                ["amet_kommentaar"] = request.OccupationComment,
                ["aadress_vabatekstina"] = request.AddressFreeText
            };
        }
    }
}