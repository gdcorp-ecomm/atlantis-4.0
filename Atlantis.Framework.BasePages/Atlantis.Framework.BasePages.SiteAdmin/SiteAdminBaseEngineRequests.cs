
namespace Atlantis.Framework.BasePages.SiteAdmin
{
  public static class SiteAdminBaseEngineRequests
  {
    private static int _MANAGERUSER = 65;
    private static int _MANAGERCATEGORIES = 462;

    public static int ManagerUserLookup
    {
      get { return _MANAGERUSER; }
      set { _MANAGERUSER = value; }
    }

    public static int ManagerCategories
    {
      get { return _MANAGERCATEGORIES; }
      set { _MANAGERCATEGORIES = value; }
    }
  }
}
