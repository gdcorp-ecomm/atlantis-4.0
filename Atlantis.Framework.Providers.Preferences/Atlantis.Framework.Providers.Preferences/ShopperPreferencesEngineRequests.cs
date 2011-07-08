
namespace Atlantis.Framework.Providers.Preferences
{
  public static class ShopperPreferencesEngineRequests
  {
    private static int _GETMODDATE = 263;
    private static int _GET = 261;
    private static int _UPDATE = 271;

    public static int PreferencesModDate
    {
      get { return _GETMODDATE; }
      set { _GETMODDATE = value; }
    }

    public static int PreferencesGet
    {
      get { return _GET; }
      set { _GET = value; }
    }

    public static int PreferencesUpdate
    {
      get { return _UPDATE; }
      set { _UPDATE = value; }
    }
  }
}
