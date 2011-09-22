using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetCalendarDetails.Interface
{
  public class ECCGetCalendarDetailsRequestData : RequestData
  {
    public int PrivateLabelId { get; private set; }
    public bool IncludeActiveOnly { get; private set; }
    public string EmailAddress { get; private set; }
    public string SubAccount { get; private set; }

    
    public ECCGetCalendarDetailsRequestData(string shopperId,
      int privateLabelId,
      bool includeActiveOnly,
      string emailAddress,
      string subAccount,
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      IncludeActiveOnly = includeActiveOnly;
      EmailAddress = emailAddress;
      SubAccount = subAccount;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("ECCGetCalendarDetails is not a cacheable request.");
    }
  }
}
