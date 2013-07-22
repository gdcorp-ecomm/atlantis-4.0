using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Net;

namespace Atlantis.Framework.CDS.Impl
{
  public class RoutingRulesRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      RoutingRulesResponseData result;

      CDSRequestData cdsRequestData = (CDSRequestData)requestData;

      WsConfigElement wsConfig = (WsConfigElement)config;

      CDSService service = new CDSService(wsConfig.WSURL + cdsRequestData.Query);

      try
      {
        string responseText = service.GetWebResponse();

        result = !string.IsNullOrEmpty(responseText) ? new RoutingRulesResponseData(responseText) : new RoutingRulesResponseData(cdsRequestData, new Exception("Empty response from the CDS service."));
      }
      catch (WebException ex)
      {
        if (ex.Response is HttpWebResponse && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
        {
          result = new RoutingRulesResponseData(ex.Message, false);
        }
        else
        {
          throw;
        }
      }
      catch (Exception ex)
      {
        result = new RoutingRulesResponseData(cdsRequestData, ex);
      }
      return result;
    }
  }
}
