using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DomainsRAA.Interface.Items
{
  public class VerifyRequestItems : IVerifyRequestItems
  {
    public string RegistrationType { get; private set; }

    public string DomainId { get; private set; }

    public DomainsRAAReasonCodes ReasonCode { get; private set; }

    public IEnumerable<IVerifyRequestItem> Items { get; private set; }

    public static IVerifyRequestItems Create(string registrationType, IEnumerable<IVerifyRequestItem> verificationItems, DomainsRAAReasonCodes reasonCode = DomainsRAAReasonCodes.None, string domainId = "")
    {
      var verifyItems = new VerifyRequestItems
      {
        RegistrationType = registrationType, 
        Items = verificationItems,
        DomainId = domainId,
        ReasonCode = reasonCode
      };

      return verifyItems;
    }
  }
}
