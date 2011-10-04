using System;
using Atlantis.Framework.Ecc.Interface.Enums;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetOFFPlanInfo.Interface
{
  public class ECCGetOFFPlanInfoRequestData : RequestData 
  {
    public ECCGetOFFPlanInfoRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int resellerId, string accountUid, string subaccount, EccOFFPlanInfoRequestStatus status) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      ResellerId = resellerId;
      AccountUid = accountUid;
      Subaccount = subaccount;
      Status = status;
    }

    public int ResellerId { get; set; }

    public string AccountUid { get; set; }

    public string Subaccount { get; set; }

    public EccOFFPlanInfoRequestStatus Status { get; set; }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
