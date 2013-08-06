using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Localization.Interface
{
  public class CountrySitesActiveRequestData : RequestData
  {
    public override string GetCacheMD5()
    {
      return "ATLANTIS_COUNTRYSITES_ACTIVE";
    }
  }
}
