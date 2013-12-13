using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Net;

namespace Atlantis.Framework.CDS.Impl
{
  public class PackageXMLRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      PackageXMLResponseData result;

      CDSRequestData cdsRequestData = (CDSRequestData)requestData;

      WsConfigElement wsConfig = (WsConfigElement)config;

      CDSService service = new CDSService(wsConfig.WSURL + cdsRequestData.Query);

      try
      {
        string responseText = service.GetWebResponse();

        if (string.IsNullOrEmpty(responseText))
        {
          throw new Exception("Empty response from the CDS service.");
        }

        result = PackageXMLResponseData.FromCDSResponse(responseText);
      }
      catch (WebException ex)
      {
        if (ex.Response is HttpWebResponse && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
        {
          //no error logging is required
          result = PackageXMLResponseData.NotFound;
        }
        else
        {
          throw;
        }
      }
      
      return result;
    }
  }
}
