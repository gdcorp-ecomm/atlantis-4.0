
namespace Atlantis.Framework.Providers.Sso
{
  public static class SsoProviderEngineRequests
  {
    public static int AuthRequestRetrieve { get; set; }
    public static int Links { get; set; }

    static SsoProviderEngineRequests()
    {
      AuthRequestRetrieve = 533;
      Links = 731;
    }
  }
}
