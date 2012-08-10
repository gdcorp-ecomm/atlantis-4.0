using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using System.Linq;

namespace Atlantis.Framework.PromoOffering.Interface
{
  public class PromoOfferingResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private IEnumerable<ResellerPromoItem> _activePromotions;
    private IDictionary<int, ResellerPromoItem> _activePromotionsByPromoGroupId;
    private IEnumerable<ResellerPromoItem> _promotions;
    private IDictionary<int, ResellerPromoItem> _promotionsByPromoGroupId;
    
    public PromoOfferingResponseData(IEnumerable<ResellerPromoItem> promotions)
    {
      Promotions = promotions;
      if (promotions != null)
      {
        ActivePromotions = promotions.Where(p => p.IsActive);
        ActivePromotionsByPromoGroupId = ActivePromotions.ToDictionary(p => p.PromoGroupId);
        PromotionsByPromoGroupId = promotions.ToDictionary(p => p.PromoGroupId);
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

    public IEnumerable<ResellerPromoItem> ActivePromotions
    {
      get
      {
        if (null == _activePromotions)
        {
          _activePromotions = new List<ResellerPromoItem>().AsReadOnly();
        }
        return _activePromotions;
      }
      private set
      {
        _activePromotions = value;
      }
    }

    public IDictionary<int, ResellerPromoItem> ActivePromotionsByPromoGroupId
    {
      get
      {
        if (null == _activePromotionsByPromoGroupId)
        {
          _activePromotionsByPromoGroupId = new Dictionary<int, ResellerPromoItem>();
        }
        return _activePromotionsByPromoGroupId;
      }
      private set
      {
        _activePromotionsByPromoGroupId = value;
      }
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
          _promotions = new List<ResellerPromoItem>().AsReadOnly();
        }
        return _promotions;
      }
      private set
      {
        _promotions = value;
      }
    }

    public IDictionary<int, ResellerPromoItem> PromotionsByPromoGroupId
    {
      get
      {
        if (null == _promotionsByPromoGroupId)
        {
          _promotionsByPromoGroupId = new Dictionary<int, ResellerPromoItem>();
        }
        return _promotionsByPromoGroupId;
      }
      private set
      {
        _promotionsByPromoGroupId = value;
      }
    }

    public bool HasActivePromotion(int promoGroupId)
    {
      bool returnValue = false;

      if (ActivePromotionsByPromoGroupId != null)
      {
        returnValue = ActivePromotionsByPromoGroupId.ContainsKey(promoGroupId);
      }

      return returnValue;
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
