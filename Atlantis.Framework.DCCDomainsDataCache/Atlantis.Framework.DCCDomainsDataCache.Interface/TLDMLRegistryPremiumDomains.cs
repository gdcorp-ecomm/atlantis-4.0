using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DCCDomainsDataCache.Interface
{
  public class TLDMLRegistryPremiumDomains : ITLDRegistryPremiumDomains
  {
    public int DefaultPremiumTier { get; private set; }

    public static ITLDRegistryPremiumDomains Create(IEnumerable<XElement> registryPremiumDomainElements)
    {
      var tldmlRegistryPremiumDomains = new TLDMLRegistryPremiumDomains();

      if (registryPremiumDomainElements != null)
      {
        var premiumDomainItems = registryPremiumDomainElements as XElement[] ?? registryPremiumDomainElements.ToArray();
        if (premiumDomainItems.Length > 0)
        {
          if (premiumDomainItems[0].IsEnabled())
          {
            foreach (var defaultPremiumTier in premiumDomainItems[0].Descendants("defaultpremiumtier"))
            {
              var defaultPremiumTierAttr = defaultPremiumTier.Attribute("value");
              if (defaultPremiumTierAttr != null)
              {
                var premierTierValue = 0;
                int.TryParse(defaultPremiumTierAttr.Value, out premierTierValue);

                tldmlRegistryPremiumDomains.DefaultPremiumTier = premierTierValue;
              }
            }
          }
        }
      }

      return tldmlRegistryPremiumDomains;
    }
  }
}