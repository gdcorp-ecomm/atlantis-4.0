using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SessionCache;

namespace Atlantis.Framework.GetCoupons.Interface
{
  public class GetCouponsResponseData : IResponseData, ISessionSerializableResponse
  {
    #region Properties

    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private List<AdWordCoupon> _couponList = null;
    
    public List<AdWordCoupon> GetCouponList()
    {
      return new List<AdWordCoupon>(_couponList);
    }

    public bool IsSuccess
    {
      get { return _exception == null; }
    }
    #endregion

    public GetCouponsResponseData()
    { }
    public GetCouponsResponseData(string xml)
    {
      _resultXML = xml;
      _couponList = new List<AdWordCoupon>();
      SetCouponList(xml);
    }

    public GetCouponsResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public GetCouponsResponseData(RequestData requestData, TimeoutException toException)
    {
      _exception = new AtlantisException(requestData,
                                   "GetCouponsResponseData",
                                   toException.Message,
                                   requestData.ToXML());
    }

    public GetCouponsResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "GetCouponsResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    #region Deserializer
    private void SetCouponList(string xml)
    {
      XmlDocument xdoc = new XmlDocument();
      xdoc.LoadXml(xml);

      XmlNodeList couponNodes = xdoc.SelectNodes("/AdWordCoupons/Coupon");
      if (couponNodes != null)
      {
        foreach (XmlNode node in couponNodes)
        {
          string couponKey = node.Attributes["couponKey"].Value;
          string orderId = node.Attributes["orderID"].Value;

          int vendorId = 0;

          if (!int.TryParse(node.Attributes["vendorID"].Value, out vendorId))
          {
            vendorId = 0;
          }

          string vendor = string.Empty;

          switch (vendorId)
          {
            case 1:
              vendor = "Google";
              break;
            case 2:
              vendor = "MSN";
              break;
            case 3:
              vendor = "Facebook";
              break;
            case 4:
              vendor = "MySpace";
              break;
            case 5:
              vendor = "Fotolia";
              break;
            default:
              vendor = string.Empty;
              break;
          }

          int value = 0;
          if (!int.TryParse(node.Attributes["couponValue"].Value, out value))
          {
            value = 0;
          }

          bool outOfStock = false;
          if (node.Attributes["outOfStock"] != null && node.Attributes["outOfStock"].Value == "1")
          {
            outOfStock = true;
          }
          else
          {
            outOfStock = false;
          }

          string couponCode = string.Empty;
          if (node.Attributes["couponCode"] != null)
          {
            couponCode = node.Attributes["couponCode"].Value;
          }

          string provisionDate = string.Empty;
          if (node.Attributes["provisionDate"] != null && !string.IsNullOrEmpty(node.Attributes["provisionDate"].Value))
          {
            provisionDate = Convert.ToDateTime(node.Attributes["provisionDate"].Value).ToShortDateString();
          }

          string expirationDate = string.Empty;
          if (node.Attributes["expirationDate"] != null && !string.IsNullOrEmpty(node.Attributes["expirationDate"].Value))
          {
            expirationDate = Convert.ToDateTime(node.Attributes["expirationDate"].Value).ToShortDateString();
          }


          AdWordCoupon coupon = new AdWordCoupon(couponKey
            , orderId
            , vendor
            , vendorId
            , value
            , outOfStock
            , couponCode, provisionDate, expirationDate);

          _couponList.Add(coupon);
        }
      }
    }
    #endregion

    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

    #region ISessionSerializableResponse Members

    public string SerializeSessionData()
    {
      return ToXML();
    }

    public void DeserializeSessionData(string sessionData)
    {
      if (!string.IsNullOrEmpty(sessionData))
      {
        _couponList = new List<AdWordCoupon>();
        SetCouponList(sessionData);
      }
    }
    #endregion
  }
}
