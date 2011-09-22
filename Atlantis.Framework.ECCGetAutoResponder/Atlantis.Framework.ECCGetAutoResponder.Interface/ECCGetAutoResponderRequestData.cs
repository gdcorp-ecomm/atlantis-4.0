using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetAutoResponder.Interface
{
  public class ECCGetAutoResponderRequestData : RequestData
  {
    public int PrivateLabelId { get; private set; }

    public string EmailAddress { get; private set; }

    public string SubAccount { get; private set; }

    public ECCGetAutoResponderRequestData(string shopperId, int privateLabelId, string emailAddress, string subAccount, string sourceURL, string orderId, string pathway, int pageCount) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      EmailAddress = emailAddress;
      SubAccount = subAccount;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("ECCGetAutoResponder is not a cacheable request.");
    }
  }
}
