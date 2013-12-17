using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DomainsRAA.Interface.VerificationItems
{
  public class VerificationItems : IVerificationItems
  {
    public string RegistrationType { get; private set; }

    public string DomainId { get; private set; }

    public IList<IItem> Items { get; private set; }

    public string VerfiedIp { get; private set; }
    
    public static VerificationItems Create(string registrationType, IList<IItem> verificationItems, string domainId = "", string verifiedIp = "")
    {
      var verifyItems = new VerificationItems
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
