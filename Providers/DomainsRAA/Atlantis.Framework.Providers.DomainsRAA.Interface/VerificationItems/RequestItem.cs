using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DomainsRAA.Interface.VerificationItems
{
  public class RequestItem
  {
    public IList<IItem> Items { get; private set; }

    public static RequestItem Create(IList<IItem> verificationItems)
    {
      var verifyItems = new RequestItem
      {
        Items = verificationItems
      };

      return verifyItems;
    }
  }
}
