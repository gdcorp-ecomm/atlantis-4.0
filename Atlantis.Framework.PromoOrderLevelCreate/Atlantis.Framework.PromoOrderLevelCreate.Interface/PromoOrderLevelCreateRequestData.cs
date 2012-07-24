using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoOrderLevelCreate.Interface
{
  public class PromoOrderLevelCreateRequestData : RequestData
  {
    #region Private memebers


    private OrderLevelPromo _orderLevelPromo;
    private string _clientIP = null;
    private string _serverIP = null;

    #endregion

    #region Constructors

    public PromoOrderLevelCreateRequestData(string sourceUrl, string pathway, int pageCount)
      : base(string.Empty, sourceUrl, string.Empty, pathway, pageCount)
    {
      this._clientIP = string.Empty;
      this._serverIP = GetHostIpAddress();
    }

    public PromoOrderLevelCreateRequestData(string sourceUrl, string pathway, int pageCount, OrderLevelPromo orderLevelPromo)
      : base(string.Empty, sourceUrl, string.Empty, pathway, pageCount)
    {
      this._clientIP = string.Empty;
      this._orderLevelPromo = orderLevelPromo;
      this._serverIP = GetHostIpAddress();
    }

    public PromoOrderLevelCreateRequestData(string sourceUrl, string pathway, int pageCount,  OrderLevelPromo orderLevelPromo, string clientIP)
      : base(string.Empty, sourceUrl, string.Empty, pathway, pageCount)
    {

      this._orderLevelPromo = orderLevelPromo;
      this._clientIP = clientIP;
      this._serverIP = GetHostIpAddress();
    }

    #endregion

    #region Public Members

    public string ClientIPAddress
    {
      get { return this._clientIP; }
      private set
      {
        this._clientIP = string.Empty;
        IPAddress address = null;

        if (IPAddress.TryParse(value, out address))
        {
          this._clientIP = address.ToString();
        }
      }
    }

    public OrderLevelPromo PromoCode
    {
      get { return this._orderLevelPromo; }
      set { this._orderLevelPromo = value; }
    }

    #endregion

    #region Baseclass implementation

    public override string GetCacheMD5()
    {
      throw new Exception("PLOrderLevelPromoRequestData is not a cacheable item");
    }

    public override string ToXML()
    {
      StringBuilder result = new StringBuilder();

      try
      {
        using (XmlTextWriter orderLevelPromoWriter = new XmlTextWriter(new StringWriter(result)))
        {
          orderLevelPromoWriter.WriteStartElement("OrderPromoCreate");
          orderLevelPromoWriter.WriteAttributeString("id", this._orderLevelPromo.PromoId);
          orderLevelPromoWriter.WriteAttributeString("startDate", this._orderLevelPromo.StartDate);
          orderLevelPromoWriter.WriteAttributeString("endDate", this._orderLevelPromo.EndDate);
          orderLevelPromoWriter.WriteAttributeString("isActive", (this._orderLevelPromo.IsActive) ? "1" : "0");
          orderLevelPromoWriter.WriteAttributeString("promoTrackingCode", this._orderLevelPromo.ISCCode);
          orderLevelPromoWriter.WriteAttributeString("iscDescription", this._orderLevelPromo.ISCDescription);

          orderLevelPromoWriter.WriteStartElement("ColumnTypes");
          orderLevelPromoWriter.WriteStartElement("ColumnType");
          orderLevelPromoWriter.WriteAttributeString("columnTypeID", this._orderLevelPromo.ColumnType.ToString());
          orderLevelPromoWriter.WriteEndElement();
          orderLevelPromoWriter.WriteEndElement();

          orderLevelPromoWriter.WriteStartElement("Currencies");
          foreach (PrivateLabelPromoCurrency currency in this._orderLevelPromo.Currencies)
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
          foreach(KeyValuePair<OrderLevelPromo.ResellerType, bool> kvp in this._orderLevelPromo.ResellerTypeList)
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

    private string GetHostIpAddress()
    {
      IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

      return addressList[0].ToString();
    }


  }
}
