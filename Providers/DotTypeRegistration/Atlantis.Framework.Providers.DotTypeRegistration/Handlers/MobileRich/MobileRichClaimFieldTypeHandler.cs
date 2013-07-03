using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Atlantis.Framework.DotTypeClaims.Interface;
using Atlantis.Framework.DotTypeForms.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface.Handlers;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Handlers.MobileRich
{
  public class MobileRichClaimFieldTypeHandler : IDotTypeFormFieldTypeHandler
  {
    public bool RenderField(FormFieldTypes fieldType, IProviderContainer providerContainer, out string dataSourceHtml)
    {
      var result = false;
      dataSourceHtml = string.Empty;

      try
      {
        var additionalData = providerContainer.GetData(FieldTypeDataKeyConstants.CLAIM_DATA_KEY, new Tuple<IDotTypeFormsField, string[]>(new DotTypeFormsField(), new string[0]));
        if (additionalData.Item2.Length > 0)
        {
          var field = additionalData.Item1;
          var domains = additionalData.Item2;
          var claimResponseData = LoadClaimData(domains);

          var claims = claimResponseData.DotTypeClaims;
          if (claims != null)
          {
            dataSourceHtml = ConvertClaimsToHtml(field, domains, claims);
            result = true;
          }
        }
      }
      catch (Exception ex)
      {
        var message = ex.Message + Environment.NewLine + ex.StackTrace;
        const string SOURCE = "RenderField - MobileRichClaimDataSourceHandler";
        var aex = new AtlantisException(SOURCE, "0", message, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);

        result = false;
      }

      return result;
    }

    private static string ConvertClaimsToHtml(IDotTypeFormsField field, IEnumerable<string> domains, IDotTypeClaimsSchema claims)
    {
      var result = new StringBuilder();

      result.Append("<div class='section-row'>");

      foreach (var domain in domains)
      {
        string claimsXml;
        if (claims.TryGetClaimsXmlByDomain(domain, out claimsXml))
        {
          result.Append("<input type='checkbox' name='" + field.FieldName + "-" + domain + "' value='" + claimsXml + "'>" + "</input>");
          result.Append("<label class='pad-lt-sm'>" + HttpUtility.HtmlEncode(claimsXml) + "</label>");
        }
      }
      result.Append("<input type='hidden' name='acceptedDate' value=''></input>");
      result.Append("</div>");

      return result.ToString();
    }

    private static DotTypeClaimsResponseData LoadClaimData(string[] domains)
    {
      var request = new DotTypeClaimsRequestData(domains);
      return (DotTypeClaimsResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeRegistrationEngineRequests.DotTypeClaimsRequest);
    }
  }
}
