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
    private IEnumerable<ResellerPromoItem> _promotions;
    private IDictionary<int, ResellerPromoItem> _promotionsByPromoGroupId;

    public PromoOfferingResponseData(IEnumerable<ResellerPromoItem> promotions)
    {
      if (promotions == null)
        throw new ArgumentNullException("promotions", "promotions is null.");

      Promotions = promotions;
      foreach (ResellerPromoItem item in promotions)
      {
        PromotionsByPromoGroupId.Add(item.PromoGroupId, item);
      }
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
    
    public bool IsSuccess
    {
      get
      {
        return null == _exception;
      }
    }

    public IEnumerable<ResellerPromoItem> Promotions
    {
      get
      {
        if (null == _promotions)
        {
          _promotions = new List<ResellerPromoItem>();
        }
        return _promotions;
      }
      private set
      {
        _promotions = value;
      }
    }

    private IDictionary<int, ResellerPromoItem> PromotionsByPromoGroupId
    {
      get
      {
        if (null == _promotionsByPromoGroupId)
        {
          _promotionsByPromoGroupId = new Dictionary<int, ResellerPromoItem>();
        }
        return _promotionsByPromoGroupId;
      }
      set
      {
        _promotionsByPromoGroupId = value;
      }
    }

    public bool TryGetPromoItemByPromoGroupId(int promoGroupId, out ResellerPromoItem promoItem)
    {
      return PromotionsByPromoGroupId.TryGetValue(promoGroupId, out promoItem);
    }

    #region IResponseData Members

    public string ToXML()
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ResellerPromoItem>));
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
