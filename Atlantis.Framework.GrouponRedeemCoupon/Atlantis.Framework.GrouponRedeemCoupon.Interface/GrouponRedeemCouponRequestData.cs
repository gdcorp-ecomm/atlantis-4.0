using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.GrouponRedeemCoupon.Interface
{
  public class GrouponRedeemCouponRequestData : RequestData
  {
    public GrouponRedeemCouponRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string couponCode) 
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      CouponCode = couponCode;
      RequestTimeout = new TimeSpan(0, 0, 10);
    }

    public string CouponCode { get; private set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

    public override string ToXML()
    {
      XElement element = new XElement("request");
      element.Add(new XAttribute("shopperid", ShopperID ?? string.Empty), new XAttribute("couponcode", CouponCode ?? string.Empty));
      return element.ToString();
    }
  }
}