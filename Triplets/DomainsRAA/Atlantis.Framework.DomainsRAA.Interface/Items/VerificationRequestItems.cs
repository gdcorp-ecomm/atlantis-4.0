using System.Collections.Generic;

namespace Atlantis.Framework.DomainsRAA.Interface.Items
{
  public class VerifyRequestItems 
  {
    public string RegistrationType { get; private set; }

    public string DomainId { get; private set; }

    public string RequestedIp { get; private set; }

    public DomainsRAAReasonCodes ReasonCode { get; private set; }

    public IEnumerable<VerifyRequestItem> Items { get; private set; }

    public static VerifyRequestItems Create(string registrationType, string requestedIp, IEnumerable<VerifyRequestItem> verificationItems, DomainsRAAReasonCodes reasonCode = DomainsRAAReasonCodes.None, string domainId = "")
    {
      var verifyItems = new VerifyRequestItems
      {
        RegistrationType = registrationType, 
        RequestedIp = requestedIp,
        Items = verificationItems,
        DomainId = domainId,
        ReasonCode = reasonCode
      };

      return verifyItems;
    }
  }
}
