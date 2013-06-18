using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPricing.Interface
{
    public class PriceGroupsByCountrySiteRequestData : RequestData
    {
      public override string GetCacheMD5()
      {
        return "ATLANTIS_PRICEGROUP_COUNTRYSITE_MAPPING";
      }
    }
}
