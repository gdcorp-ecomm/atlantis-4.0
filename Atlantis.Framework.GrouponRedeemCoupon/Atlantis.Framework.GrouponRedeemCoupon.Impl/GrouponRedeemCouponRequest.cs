using Atlantis.Framework.GrouponRedeemCoupon.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Atlantis.Framework.GrouponRedeemCoupon.Impl
{
  class GrouponRedeemCouponRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      GrouponRedeemCouponResponseData response = null;
      string redeemXml = string.Empty;

      try
      {
        var grouponRequest = (GrouponRedeemCouponRequestData)requestData;

        if (string.IsNullOrEmpty(grouponRequest.ShopperID))
        {
          response = new GrouponRedeemCouponResponseData(GrouponRedeemStatus.UnknownShopper, "Empty shopper");
        }
        else if (string.IsNullOrEmpty(grouponRequest.CouponCode))
        {
          response = new GrouponRedeemCouponResponseData(GrouponRedeemStatus.UnknownCode, "Empty coupon code");
        }
        else
        {
          using (var couponService = new InStoreCreditCouponService.Coupons())
          {
            string wsURL = ((WsConfigElement)config).WSURL;
            couponService.Url = wsURL;
            couponService.Timeout = (int)Math.Truncate(grouponRequest.RequestTimeout.TotalMilliseconds);

            couponService.ClientCertificates.Add(((WsConfigElement)config).GetClientCertificate());
            redeemXml = couponService.Redeem(grouponRequest.ShopperID, grouponRequest.CouponCode);
          }

          XElement element = XElement.Parse(redeemXml);
          if (!"result".Equals(element.Name.ToString(), StringComparison.OrdinalIgnoreCase))
          {
            element = element.Descendants("Result").FirstOrDefault();
          }

          int status = -1;
          int parsedStatus;
          if (int.TryParse(element.Attribute("status").Value, out parsedStatus))
          {
            status = parsedStatus;
          }

          if (status == 0)
          {
            int amount = Convert.ToInt32(element.Attribute("amount").Value);
            string currency = element.Attribute("currency").Value;
            response = new GrouponRedeemCouponResponseData(amount, currency);
          }
          else
          {
            string message = element.Attribute("error") != null ? element.Attribute("error").Value : "Empty error attribute";
            GrouponRedeemStatus couponStatus = GrouponRedeemStatus.UnknownError;

            switch (status)
            {
              case 1:
                couponStatus = GrouponRedeemStatus.UnknownShopper;
                break;
              case -1:
                couponStatus = GrouponRedeemStatus.UnknownCode;
                break;
              case -2:
                couponStatus = GrouponRedeemStatus.InvalidCodeAlreadyUsed;
                break;
              case -3:
                couponStatus = GrouponRedeemStatus.ExpiredCode;
                break;
              case -4:
                couponStatus = GrouponRedeemStatus.InactiveCode;
                break;
              case 5: // yes positive 5 is correct
                couponStatus = GrouponRedeemStatus.InvalidCodeTooLongOrEmpty;
                break;
              case -5:
                couponStatus = GrouponRedeemStatus.InvalidCodeShopperAlreadyUsedAnother;
                break;
            }

            response = new GrouponRedeemCouponResponseData(couponStatus, message);
          }
          
        }

      }

      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException(requestData, "GrouponRedeemCouponRequest.RequestHandler", ex.Message + ex.StackTrace, redeemXml);
        response = new GrouponRedeemCouponResponseData(aex);
      }

      return response;
    }    
  }
}
