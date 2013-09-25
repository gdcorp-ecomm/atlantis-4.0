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
      ProductLineNameRequestData productLineNameRequest = (ProductLineNameRequestData)requestData;
      string productLineNameXml = ProductLineNamesData.ProductLineNamesXml;

      return ProductLineNameResponseData.FromProductLineNameXml(productLineNameXml, productLineNameRequest.ContextId);
    }
  }
}