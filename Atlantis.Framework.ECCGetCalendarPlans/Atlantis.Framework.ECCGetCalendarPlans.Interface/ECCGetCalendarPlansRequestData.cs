using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetCalendarPlans.Interface
{
  public class ECCGetCalendarPlansRequestData : RequestData
  {
    public int PrivateLabelId { get; private set; }
    public string CalendarPlanUid { get; private set; }
    public string SubAccount { get; private set; }
    
    public ECCGetCalendarPlansRequestData(string shopperId, int privateLabelId, string calendarPlanUid, string subAccount, string sourceURL, string orderId, string pathway, int pageCount) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      CalendarPlanUid = calendarPlanUid;
      SubAccount = subAccount;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("ECCGetCalendarPlans is not a cacheable request.");
    }
  }
}
