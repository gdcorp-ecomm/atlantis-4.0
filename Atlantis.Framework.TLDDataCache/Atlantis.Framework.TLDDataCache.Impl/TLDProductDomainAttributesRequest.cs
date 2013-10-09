using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.TLDDataCache.Impl
{
  public class TLDProductDomainAttributesRequest : IRequest
  {
    private const string REQUESTFORMAT =  "<ProductDomainAttributesGet><param name=\"n_tldID\" value=\"{0}\"/><param name=\"n_gdshop_tldPhase\" value=\"{1}\"/>" +
                                          "<param name=\"n_privateLabelResellerTypeID\" value=\"{2}\"/><param name=\"n_gdshop_product_typeID\" value=\"{3}\"/></ProductDomainAttributesGet>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        int tldId = ((TLDProductDomainAttributesRequestData)requestData).TldId;
        string tldPhase = ((TLDProductDomainAttributesRequestData)requestData).TldPhase;
        int privateLabelResellerTypeId = ((TLDProductDomainAttributesRequestData)requestData).PrivateLabelResellerTypeId;
        int productTypeId = ((TLDProductDomainAttributesRequestData)requestData).ProductTypeId;

        if (tldId <= 0 || privateLabelResellerTypeId <= 0 || productTypeId <= 0)
        {
          throw new ArgumentException("TldId, PrivateLabelResellerTypeId and productTypeId must be greater than zero.");
        }

        if (string.IsNullOrEmpty(tldPhase))
        {
          throw new ArgumentException("TldPhase cannot be empty.");
        }

        string requestXml = string.Format(REQUESTFORMAT, tldId, tldPhase, privateLabelResellerTypeId, productTypeId);

        string responseXml;
        using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          responseXml = comCache.GetCacheData(requestXml);
        }

        XElement datacacheElements = XElement.Parse(responseXml);
        result = TLDProductDomainAttributesResponseData.FromDataCacheElement(datacacheElements);
      }
      catch (Exception ex)
      {
        result = TLDProductDomainAttributesResponseData.FromException(requestData, ex);
      }

      return result;
    }
  }
}
