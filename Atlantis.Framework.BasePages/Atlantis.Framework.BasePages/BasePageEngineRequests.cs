
namespace Atlantis.Framework.BasePages
{
  public static class BasePageEngineRequests
  {
    private static int _VERIFYSHOPPER = 737;
    private static int _MANAGERLOOKUP = 65;

    public static int VerifyShopper
    {
      get { return _VERIFYSHOPPER; }
      set { _VERIFYSHOPPER = value; }
    }

    public static int ManagerLookup
    {
      get { return _MANAGERLOOKUP; }
      set { _MANAGERLOOKUP = value; }
    }
  }
}
