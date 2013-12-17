using System.Collections.Generic;

namespace Atlantis.Framework.DomainsRAA.Interface.Items
{
  public class RequestElement 
  {
    public IEnumerable<ItemElement> Items { get; private set; }

    public string RequestedIp { get; private set; }

    public static RequestElement Create(string requestedIp, IEnumerable<ItemElement> verificationItems)
    {
      var verifyItems = new RequestElement
      {
        RequestedIp = requestedIp,
        Items = verificationItems
      };

      return verifyItems;
    }
  }
}
