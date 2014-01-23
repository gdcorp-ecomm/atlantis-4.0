using Atlantis.Framework.CDS.Impl;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Products.Interface;
using System;
using System.Net;

namespace Atlantis.Framework.Products.Impl
{
  public class ProductGroupsOfferedMarketsRequest : IRequest
  {
    private const string QUERY = "content/atlantis/productgroups/availablemarkets";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ProductGroupsOfferedMarketsResponseData result = null;

      var cdsRequestData = (ProductGroupsOfferedMarketsRequestData)requestData;

      var wsConfig = (WsConfigElement)config;

      var service = new CDSService(wsConfig.WSURL + QUERY);

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
