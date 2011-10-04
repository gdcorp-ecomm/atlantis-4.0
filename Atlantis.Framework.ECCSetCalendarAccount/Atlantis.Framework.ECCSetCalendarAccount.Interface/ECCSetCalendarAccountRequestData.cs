using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCSetCalendarAccount.Interface
{
  public class ECCSetCalendarAccountRequestData : RequestData
  {
    public int PrivateLabelId { get; private set; }
    public string EmailAddress { get; private set; }
    public Guid CalendarPlanGuid { get; private set; }
    public string Message { get; private set; }
    public string SubAccount { get; private set; }

    public ECCSetCalendarAccountRequestData(string shopperId, 
      int privateLabelId,
      string emailAddress,
      Guid calendarPlanGuid,
      string message,
      string subAccount,
      string sourceURL,
      string orderId,
      string pathway,
      int pageCount) 
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      EmailAddress = emailAddress;
      CalendarPlanGuid = calendarPlanGuid;
      Message = message;
      SubAccount = subAccount;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("ECCSetCalendarAccount is not a cacheable request.");
    }
  }
}
