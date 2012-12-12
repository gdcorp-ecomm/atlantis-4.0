using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GrouponRedeemCoupon.Interface
{
  public class GrouponRedeemCouponResponseData : IResponseData
  {
    public int Status { get; private set; }
    public int Amount { get; private set; }
    public string Currency { get; private set; }

    public GrouponRedeemCouponResponseData(int status, int amount, string currency)
    {
      Status = status;
      Amount = amount;
      Currency = currency;
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      // don't bother using this, just throw any exeception from the request
      return null;
    }
  }
}
