using System.Collections.Generic;

namespace Atlantis.Framework.DomainsRAA.Interface.Items
{
  public class VerificationItemsElement 
  {
    public string RegistrationType { get; private set; }

    public string DomainId { get; private set; }

    public IEnumerable<ItemElement> Items { get; private set; }

    public string VerfiedIp { get; private set; }

    public static VerificationItemsElement Create(string registrationType, IEnumerable<ItemElement> verificationItems, string domainId = "", string verifiedIp = "")
    {
      var verifyItems = new VerificationItemsElement
      {
        RegistrationType = registrationType, 
        Items = verificationItems,
        DomainId = domainId,
        VerfiedIp = verifiedIp
      };

      return verifyItems;
    }
  }
}
