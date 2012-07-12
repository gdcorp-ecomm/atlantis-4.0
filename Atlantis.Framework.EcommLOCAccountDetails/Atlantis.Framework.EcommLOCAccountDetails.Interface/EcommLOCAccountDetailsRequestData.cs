using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommLOCAccountDetails.Interface
{
  public class EcommLOCAccountDetailsRequestData : RequestData
  {

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int AccountId { get; set; }

    public EcommLOCAccountDetailsRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, DateTime startDate, DateTime endDate, int accountId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      StartDate = startDate;
      EndDate = endDate;
      AccountId = accountId;
    }


    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

  }
}
