using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivacyPlusBundleXML.Impl.DomainProtectionXMLService;
using Atlantis.Framework.PrivacyPlusBundleXML.Interface;

namespace Atlantis.Framework.PrivacyPlusBundleXML.Impl
{
  public class PrivacyPlusBundleXMLRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      PrivacyPlusBundleXMLResponseData responseData = null;
      int plRenewalUnifiedProductId = 0;
      int plBundleId = 0;
      string errorMsg = string.Empty;
      string customXml = string.Empty;

      try
      {
        PrivacyPlusBundleXMLRequestData request = (PrivacyPlusBundleXMLRequestData)requestData;
        wscgdDomainProtectionXMLService ws = new wscgdDomainProtectionXMLService();
        ws.Url = ((WsConfigElement)config).WSURL;
        ws.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

        customXml = ws.GetDomainAddOnBundleXML(request.DomainId, request.Duration, out plRenewalUnifiedProductId, out plBundleId, out errorMsg);

        if (string.IsNullOrEmpty(errorMsg))
        {
          responseData = new PrivacyPlusBundleXMLResponseData(customXml, plRenewalUnifiedProductId, plBundleId);
        }
        else
        {
          AtlantisException aex = new AtlantisException(request, "PrivacyPlusBundleXMLRequest::RequestHandler", errorMsg, string.Empty);
          responseData = new PrivacyPlusBundleXMLResponseData(aex);
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new PrivacyPlusBundleXMLResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new PrivacyPlusBundleXMLResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}
