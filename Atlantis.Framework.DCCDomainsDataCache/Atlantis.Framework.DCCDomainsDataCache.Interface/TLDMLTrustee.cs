using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TLDMLTrustee : ITLDTrustee
  {
    public static ITLDTrustee Create(IEnumerable<XElement> trusteeElements)
    {
      var tldmlTrustee = new TLDMLTrustee();

      if (trusteeElements != null)
      {
        var trusterItems = trusteeElements as XElement[] ?? trusteeElements.ToArray();

        if (trusterItems.Length > 0)
        {
          tldmlTrustee.IsRequired = trusterItems[0].IsEnabled();
        }
      }

      return tldmlTrustee;
    }

    public bool IsRequired { get; private set; }
  }
}