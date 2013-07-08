using System;
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
        var additionalData = providerContainer.GetData(FieldTypeDataKeyConstants.CLAIM_DATA_KEY, new Tuple<IDotTypeFormsField, string>(new DotTypeFormsField(), string.Empty));
        if (additionalData.Item2.Length > 0)
        {
          var field = additionalData.Item1;
          var domain = additionalData.Item2;
          var claimResponseData = LoadClaimData(domain);

          var claims = claimResponseData.DotTypeClaims;
          if (claims != null)
          {
            dataSourceHtml = ConvertClaimsToHtml(field, domain, claims);
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

    private static string ConvertClaimsToHtml(IDotTypeFormsField field, string domain, IDotTypeClaimsSchema claims)
    {
      var result = new StringBuilder();

      string claimsXml;
      if (claims.TryGetClaimsXmlByDomain(domain, out claimsXml))
      {
        result.Append("<div class='section-row groove'>");
        result.Append("<input type='checkbox' name='" + field.FieldName + "' value='" + claimsXml + "'>" + "</input>");
        result.Append("<label class='pad-lt-sm'>" + HttpUtility.HtmlEncode(claimsXml) + "</label>");
        result.Append("<input type='hidden' name='acceptedDate' value=''></input>");
        result.Append("</div>");
      }

      return result.ToString();
    }

    private static DotTypeClaimsResponseData LoadClaimData(string domain)
    {
      string[] domains = { domain };

      var request = new DotTypeClaimsRequestData(domains);
      return (DotTypeClaimsResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeRegistrationEngineRequests.DotTypeClaimsRequest);
    }
  }
}
