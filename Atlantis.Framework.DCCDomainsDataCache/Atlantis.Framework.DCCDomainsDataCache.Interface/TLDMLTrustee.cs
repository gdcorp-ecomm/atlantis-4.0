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

            XAttribute trusteeVendorId = trusterItems[0].Attribute("trusteevendorid");
            int trusteeValue = 0;
            if (trusteeVendorId != null && int.TryParse(trusteeVendorId.Value, out trusteeValue))
            {
                tldmlTrustee.TrusteeVendorId = trusteeValue;
            }
        }
      }

      return tldmlTrustee;
    }

    public bool IsRequired { get; private set; }

    public int TrusteeVendorId
    {
        get;
        private set;
    }
  }
}