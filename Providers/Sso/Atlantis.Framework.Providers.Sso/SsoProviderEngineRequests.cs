
namespace Atlantis.Framework.Providers.Sso
{
  public static class SsoProviderEngineRequests
  {
    public static int AuthRequestRetrieve { get; set; }
    public static int DataCacheGeneric { get; set; }
    public static int Links { get; set; }

    static SsoProviderEngineRequests()
    {
      AuthRequestRetrieve = 533;
      DataCacheGeneric = 694;
      Links = 731;
    }
  }
}
