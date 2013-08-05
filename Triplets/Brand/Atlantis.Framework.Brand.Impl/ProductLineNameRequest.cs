using System;
using Atlantis.Framework.Brand.Impl.Data;
using Atlantis.Framework.Brand.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Brand.Impl
{
  public class ProductLineNameRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        ProductLineNameRequestData productLineNameRequest = (ProductLineNameRequestData) requestData;
        string productLineNameXml = ProductLineNamesData.ProductLineNamesXml;

        result = ProductLineNameResponseData.FromProductLineNameXml(productLineNameXml, productLineNameRequest.ContextId);
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException(requestData, "ProductLineNameRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        result = ProductLineNameResponseData.FromException(exception);
      }

      return result;
    }
  }
}