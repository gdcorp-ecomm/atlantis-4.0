using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.GrouponRedeemCoupon.Interface
{
  public class GrouponRedeemCouponResponseData : IResponseData
  {
    private AtlantisException _exception = null;

    public GrouponRedeemStatus Status { get; private set; }
    public int Amount { get; private set; }
    public string Currency { get; private set; }
    public string Message { get; private set; }

    public GrouponRedeemCouponResponseData(int amount, string currency)
    {
      Status = GrouponRedeemStatus.Success;
      Amount = amount;
      Currency = currency;
      Message = string.Empty;
    }

    public GrouponRedeemCouponResponseData(GrouponRedeemStatus errorStatus, string errorMessage)
    {
      Status = errorStatus;
      Amount = 0;
      Currency = string.Empty;
      Message = errorMessage;
    } 

    public GrouponRedeemCouponResponseData(AtlantisException exception)
    {
      _exception = exception;
      Status = GrouponRedeemStatus.UnknownError;
      Amount = 0;
      Currency = string.Empty;
      Message = string.Empty;
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
