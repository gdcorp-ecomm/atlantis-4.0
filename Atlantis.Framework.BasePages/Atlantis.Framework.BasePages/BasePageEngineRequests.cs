namespace Atlantis.Framework.BasePages
{
  public static class BasePageEngineRequests
  {
    private static int _verifyShopper = 737;
    private static int _managerUserLookup = 65;
    private static int _managerProperties = 462;

    public static int VerifyShopper
    {
      get { return _verifyShopper; }
      set { _verifyShopper = value; }
    }

    public static int ManagerLookup
    {
      get { return _managerUserLookup; }
      set { _managerUserLookup = value; }
    }

    public static int ManagerProperties
    {
      get { return _managerProperties; }
      set { _managerProperties = value; }
    }
  }
}
