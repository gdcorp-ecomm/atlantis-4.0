using Atlantis.Framework.CDS.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Net;

namespace Atlantis.Framework.CDS.Impl
{
  public class ProductGroupsOfferedMarketsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ProductGroupsOfferedMarketsResponseData result = null;

      var cdsRequestData = (CDSRequestData)requestData;

      var wsConfig = (WsConfigElement)config;

      var service = new CDSService(wsConfig.WSURL + cdsRequestData.Query);

      try
      {
        var responseText = service.GetWebResponse();

        if (string.IsNullOrEmpty(responseText))
        {
          throw new Exception("Empty response from the CDS service.");
        }

        result = ProductGroupsOfferedMarketsResponseData.FromCDSResponse(responseText);
      }
      catch (WebException ex)
      {
        if (ex.Response is HttpWebResponse && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
        {
          result = ProductGroupsOfferedMarketsResponseData.NotFound;
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
