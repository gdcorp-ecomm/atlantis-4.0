using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Atlantis.Framework.CDS.Impl
{
  public class RoutingRulesRequest : IRequest
  {
    const string ApplicationNameKey = "ApplicationName";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      RoutingRulesResponseData result = null;
      CDSRequestData cdsRequestData = requestData as CDSRequestData;
      WsConfigElement wsConfig = (WsConfigElement)config;
      cdsRequestData.AppName = wsConfig.GetConfigValue(ApplicationNameKey); //used to identify the App in the errorlog entry

      CDSService service = new CDSService(wsConfig.WSURL + cdsRequestData.Query);
      try
      {
        string responseText = service.GetWebResponse();
        if (!string.IsNullOrEmpty(responseText))
        {
          result = new RoutingRulesResponseData(responseText);
        }
        else
        {
          result = new RoutingRulesResponseData(cdsRequestData, new Exception("Empty response from the CDS service."));
        }
      }
      catch (WebException ex)
      {
        if (ex.Response is HttpWebResponse && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
        {
          result = new RoutingRulesResponseData(ex.Message, false);
        }
        else
        {
          result = new RoutingRulesResponseData(cdsRequestData, ex);
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
