using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using XRoadTestApp.XRoad.DTO.Shared;
using XRoadTestApp.XRoad.Services.SoapService.Interfaces;

namespace XRoadTestApp.XRoad.Services.SoapService.Services
{
    public class SoapService : ISoapService
    {
        public string BuildSoapBody<T>(IEnumerable<T> items, SoapBodyConfig config,
            Func<T, Dictionary<string, object>> fieldMapper)
        {
            var itemsXml = new StringBuilder();

            foreach (var item in items)
            {
                itemsXml.Append("<item>");

                var fields = fieldMapper(item);
                foreach (var field in fields)
                {
                    AppendFieldIfNotNull(itemsXml, field.Key, field.Value);
                }

                itemsXml.Append("</item>");
            }

            return $@"<{config.RootElement}>
    <request>
        <{config.ItemsContainerElement}>
            {itemsXml}
        </{config.ItemsContainerElement}>
    </request>
</{config.RootElement}>";
        }


        private void AppendField(StringBuilder sb, string fieldName, object value)
        {
            if (value != null)
            {
                sb.Append($"<{fieldName}>{SecurityElement.Escape(value.ToString())}</{fieldName}>");
            }
        }

        private void AppendFieldIfNotNull(StringBuilder sb, string fieldName, object value)
        {
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                AppendField(sb, fieldName, value);
            }
        }
    }
}