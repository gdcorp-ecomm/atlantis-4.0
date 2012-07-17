using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductOfferingFreeOffers.Interface
{
  public class ProductOfferingFreeOffersResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public List<ResellerFreeProductItem> FreeProductsList { get; private set; }

    public ProductOfferingFreeOffersResponseData(List<ResellerFreeProductItem> _freeProductsList)
    {
      FreeProductsList = _freeProductsList;
    }

    public ProductOfferingFreeOffersResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public ProductOfferingFreeOffersResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "ProductOfferingFreeOffersResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ResellerFreeProductItem>));
      StringWriter writer = new StringWriter();

      xmlSerializer.Serialize(writer, FreeProductsList);

      return writer.ToString();

    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
