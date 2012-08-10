using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoOffering.Interface
{
  public class PromoOfferingResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public IEnumerable<ResellerPromoItem> Promotions { get; private set; }

    public PromoOfferingResponseData(IEnumerable<ResellerPromoItem> promotions)
    {
      Promotions = promotions;
    }

    public PromoOfferingResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public PromoOfferingResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "PromoOfferingResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof(IEnumerable<ResellerPromoItem>));
      StringWriter writer = new StringWriter();

      xmlSerializer.Serialize(writer, Promotions);

      return writer.ToString();

    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}
