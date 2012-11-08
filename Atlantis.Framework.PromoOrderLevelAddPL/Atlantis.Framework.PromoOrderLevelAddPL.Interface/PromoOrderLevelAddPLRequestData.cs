using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Net;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PromoOrderLevelAddPL.Interface
{
  public class ResellerPromoProcessedEventArgs : EventArgs
  {
    public readonly int PrivateLabelId;
    public readonly bool IsSuccessful;
    public readonly string ReasonOmmitted;

    public ResellerPromoProcessedEventArgs(int privateLabelId, bool isSuccessful, string reasonOmmitted)
    {
      this.PrivateLabelId = privateLabelId;
      this.IsSuccessful = isSuccessful;
      this.ReasonOmmitted = (string.IsNullOrEmpty(reasonOmmitted)) ? string.Empty : reasonOmmitted;
    }
  }

  public delegate void ResellerPromoProcessedHandler(object sender, ResellerPromoProcessedEventArgs e);

  public class PromoOrderLevelAddPLRequestData : RequestData
  {
    public event ResellerPromoProcessedHandler ResellerPromoProcessed;

    #region Private memebers

    private Dictionary<int, PrivateLabelPromo>  _resellerList;
    private string                              _promoCode;
    private string                              _clientIP = null;
    private string                              _serverIP = null;

    #endregion

    #region Constructors

    public PromoOrderLevelAddPLRequestData(string sourceUrl, string pathway, int pageCount) : base(string.Empty, sourceUrl, string.Empty, pathway, pageCount) 
    {
      this._resellerList = new Dictionary<int, PrivateLabelPromo>();
      this._clientIP = string.Empty;
      this._serverIP = GetHostIpAddress();
    }

    public PromoOrderLevelAddPLRequestData(string sourceUrl, string pathway, int pageCount, string promoCodeToUse, string clientIP)
      : base(string.Empty, sourceUrl, string.Empty, pathway, pageCount)
    {
      this._promoCode = promoCodeToUse;
      this._clientIP = clientIP;
      this._resellerList = new Dictionary<int, PrivateLabelPromo>();
      this._serverIP = GetHostIpAddress();
    }

    public PromoOrderLevelAddPLRequestData(string sourceUrl, string pathway, int pageCount, string promoCodeToUse, string clientIP, Dictionary<int, PrivateLabelPromo> resellerPromoList)
      : base(string.Empty, sourceUrl, string.Empty, pathway, pageCount)
    {
      this._promoCode = promoCodeToUse;
      this._resellerList = resellerPromoList;
      this._clientIP = clientIP;
      this._serverIP = GetHostIpAddress();
    }

    public PromoOrderLevelAddPLRequestData(string sourceUrl, string pathway, int pageCount, Dictionary<int, PrivateLabelPromo> resellerPromoList, string promoCodeToUse, string clientIP)
      :base(string.Empty, sourceUrl, string.Empty, pathway, pageCount)
    {
      this._resellerList = resellerPromoList;
      this._promoCode = promoCodeToUse;
      this._clientIP = clientIP;
      this._serverIP = GetHostIpAddress();
    }

    public PromoOrderLevelAddPLRequestData(string sourceUrl, string pathway, int pageCount, string promoCodeToUse, PrivateLabelPromo resellerPromo)
      : base(string.Empty, sourceUrl, string.Empty, pathway, pageCount)
    {
      this._resellerList = new Dictionary<int, PrivateLabelPromo>();
      this._promoCode = promoCodeToUse;
      this._resellerList.Add(resellerPromo.PrivateLabelId, resellerPromo);
      this._serverIP = GetHostIpAddress();
      this._clientIP = string.Empty;
    }

    public PromoOrderLevelAddPLRequestData(string sourceUrl, string pathway, int pageCount, string promoCodeToUse, PrivateLabelPromo resellerPromo, string clientIP)
      : base(string.Empty, sourceUrl, string.Empty, pathway, pageCount)
    {
      this._promoCode = promoCodeToUse;
      this._resellerList = new Dictionary<int, PrivateLabelPromo>();
      this._resellerList.Add(resellerPromo.PrivateLabelId, resellerPromo);
      this._serverIP = GetHostIpAddress();
      this._clientIP = clientIP;
    }

    #endregion

    #region Public members

    /// <summary>
    /// Gets the IP address of the requesting client
    /// </summary>
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

    /// <summary>
    /// Gets or sets the order-level promo ID to which the reseller will be added to.
    /// </summary>
    public string PromoCode
    {
      get { return this._promoCode; }
      set { this._promoCode = value; }
    }

    /// <summary>
    /// Gets the list of PLID and corresponding PrivateLabelPromo objects that will be appended to the order-level promo.
    /// </summary>
    public Dictionary<int, PrivateLabelPromo> ResellerPromoList
    {
      get { return this._resellerList; }
    }

    #endregion

    #region public methods

    /// <summary>
    /// Adds the specified PrivateLabelPromo object to the dictionary set of resellers that will be appended to the order-level promo.
    /// </summary>
    /// <param name="resellerToAdd">The PrivateLabelPromo object to add to the set of resellers being appended to the order-level promo.</param>
    public void AddResellerToPromoList(PrivateLabelPromo resellerToAdd)
    {
      if (!this._resellerList.ContainsKey(resellerToAdd.PrivateLabelId))
      {
        this._resellerList.Add(resellerToAdd.PrivateLabelId, resellerToAdd);
      }
    }

    public override string GetCacheMD5()
    {
      throw new Exception("ResellerOrderLevelPromoRequestData is not a cacheable item.");
    }

    public override string ToXML()
    {
      StringBuilder builder = new StringBuilder();

      try
      {
        using (XmlTextWriter resellerOrderLevelPromoWriter = new XmlTextWriter(new StringWriter(builder)))
        {
          resellerOrderLevelPromoWriter.WriteStartElement("OrderPromoPrivateLabel");
          resellerOrderLevelPromoWriter.WriteAttributeString("id", this._promoCode);
          resellerOrderLevelPromoWriter.WriteStartElement("PrivateLabels");

          foreach (PrivateLabelPromo promo in this._resellerList.Values)
          {
            if (ValidateDate(promo.StartDate) && ValidateDate(promo.EndDate))
            {
              if (ValidatePromoEndDate(promo.EndDate) || !promo.isActive)
              {
                resellerOrderLevelPromoWriter.WriteStartElement("PrivateLabelAdd");
                resellerOrderLevelPromoWriter.WriteAttributeString("privateLabelID", promo.PrivateLabelId.ToString());
                resellerOrderLevelPromoWriter.WriteAttributeString("isActive", (promo.isActive) ? "1" : "0");
                resellerOrderLevelPromoWriter.WriteAttributeString("startDate", promo.StartDate);
                resellerOrderLevelPromoWriter.WriteAttributeString("endDate", promo.EndDate);
                resellerOrderLevelPromoWriter.WriteEndElement(); //Close out tag for <PrivateLabelAdd> element

                this.OnProcessResellerRequest(this, new ResellerPromoProcessedEventArgs(promo.PrivateLabelId, true, null));

              }
              else
              {
                this.OnProcessResellerRequest(this, new ResellerPromoProcessedEventArgs(promo.PrivateLabelId, false, "The promo end date is in the past - [plid:" + promo.PrivateLabelId + "],[endDate:" + promo.EndDate + "]"));
              }
            }
            else
            {
              this.OnProcessResellerRequest(this, new ResellerPromoProcessedEventArgs(promo.PrivateLabelId, false, "Either the startDate or endDate of the promo is invalid - [startDate:" + promo.StartDate + "],[endDate:" + promo.EndDate + "]"));
            }
          }

          resellerOrderLevelPromoWriter.WriteEndElement(); //Close out tag for <PrivateLabels> element
          resellerOrderLevelPromoWriter.WriteEndElement(); //Close out tag for root <OrderPromoPrivateLabel element
        }
      }
      catch (AtlantisException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        throw new AtlantisException(this, "ResellerOrderLevelPromoRequestData::ToXml", ex.Message, ex.StackTrace);
      }

      return builder.ToString();
    }

    #endregion

    #region Private methods

    private string GetHostIpAddress()
    {
      IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

      return addressList[0].ToString();
    }

    private static bool ValidateDate(string dateToValidate)
    {
      DateTime dummy;
      return DateTime.TryParse(dateToValidate, out dummy);
    }

    private static bool ValidatePromoEndDate(string endDate)
    {
      bool result = true;
      DateTime evalDate;
      try
      {
        if (DateTime.TryParse(endDate, out evalDate))
        {
          if (DateTime.Now.Date > evalDate.Date)
          {
            result = false;
          }
        }
      }
      catch
      {
        result = false;
      }

      return result;
    }

    private void OnProcessResellerRequest(object sender, ResellerPromoProcessedEventArgs e)
    {
      if (this.ResellerPromoProcessed != null)
      {
        this.ResellerPromoProcessed(sender, e);
      }
    }

    #endregion
  }
}
