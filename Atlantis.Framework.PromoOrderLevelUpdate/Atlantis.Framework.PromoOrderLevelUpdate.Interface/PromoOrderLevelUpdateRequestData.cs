using System;
using System.Text;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PromoOrderLevelCreate.Interface;

namespace Atlantis.Framework.PromoOrderLevelUpdate.Interface
{
    public class PromoOrderLevelUpdateRequestData : PromoOrderLevelCreateRequestData
    {

      #region Constructors

      public PromoOrderLevelUpdateRequestData(string sourceUrl, string pathway, int pageCount)
        : base(sourceUrl, pathway, pageCount)
      {
      }

      public PromoOrderLevelUpdateRequestData(string sourceUrl, string pathway, int pageCount, OrderLevelPromoVersion orderLevelPromo)
        :base(sourceUrl, pathway, pageCount, orderLevelPromo)
      {
      }

      public PromoOrderLevelUpdateRequestData(string sourceUrl, string pathway, int pageCount, OrderLevelPromoVersion orderLevelPromo, string clientIP)
        : base(sourceUrl, pathway, pageCount, orderLevelPromo, clientIP)
      {
      }

      #endregion

      #region base methods

      public override string GetCacheMD5()
      {
        throw new Exception("UpdateOrderPromoPLRequestData is not a cacheable item.");
      }

      public override string ToXML()
      {
        StringBuilder result = new StringBuilder();
        OrderLevelPromoVersion promo = null;
        try
        {
          promo = this.PromoCode as OrderLevelPromoVersion;
          using (XmlTextWriter orderLevelPromoWriter = new XmlTextWriter(new StringWriter(result)))
          {
            orderLevelPromoWriter.WriteStartElement("OrderPromoUpdate");
            orderLevelPromoWriter.WriteAttributeString("id",promo.PromoId);
            orderLevelPromoWriter.WriteAttributeString("version", promo.VersionId.ToString());
            orderLevelPromoWriter.WriteAttributeString("startDate", promo.StartDate);
            orderLevelPromoWriter.WriteAttributeString("endDate", promo.EndDate);
            orderLevelPromoWriter.WriteAttributeString("isActive", (promo.IsActive) ? "1" : "0");
            orderLevelPromoWriter.WriteAttributeString("promoTrackingCode", promo.ISCCode);
            orderLevelPromoWriter.WriteAttributeString("iscDescription", promo.ISCDescription);

            orderLevelPromoWriter.WriteStartElement("ColumnTypes");
            orderLevelPromoWriter.WriteStartElement("ColumnType");
            orderLevelPromoWriter.WriteAttributeString("columnTypeID", promo.ColumnType.ToString());
            orderLevelPromoWriter.WriteEndElement();
            orderLevelPromoWriter.WriteEndElement();

            orderLevelPromoWriter.WriteStartElement("Currencies");
            foreach (PrivateLabelPromoCurrency currency in promo.Currencies)
            {
              orderLevelPromoWriter.WriteStartElement("Currency");
              orderLevelPromoWriter.WriteAttributeString("awardValue", currency.AwardValue.ToString());
              orderLevelPromoWriter.WriteAttributeString("currencyType", currency.CurrencyType);
              orderLevelPromoWriter.WriteAttributeString("awardType", ((int)currency.TypeOfAward).ToString());
              orderLevelPromoWriter.WriteAttributeString("minSubtotal", currency.MinSubtotal.ToString());
              orderLevelPromoWriter.WriteEndElement();
            }
            orderLevelPromoWriter.WriteEndElement();

            orderLevelPromoWriter.WriteStartElement("ResellerTypes");
            foreach (KeyValuePair<OrderLevelPromo.ResellerType, bool> kvp in promo.ResellerTypeList)
            {
              orderLevelPromoWriter.WriteStartElement("ResellerType");
              orderLevelPromoWriter.WriteAttributeString("resellerTypeID", ((int)kvp.Key).ToString());
              orderLevelPromoWriter.WriteAttributeString("isActive", (kvp.Value) ? "1" : "0");
              orderLevelPromoWriter.WriteEndElement();
            }
            orderLevelPromoWriter.WriteEndElement();

            orderLevelPromoWriter.WriteEndElement();

          }

        }
        catch (AtlantisException ex)
        {
          throw ex;
        }
        catch (Exception ex)
        {
          throw new AtlantisException(this, "PLOrderLevelPromoRequestData::ToXml", ex.Message, ex.StackTrace);
        }

        return result.ToString();

      } 

      #endregion


    }
}
