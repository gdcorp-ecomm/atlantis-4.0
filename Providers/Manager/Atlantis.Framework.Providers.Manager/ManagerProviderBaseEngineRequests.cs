namespace Atlantis.Framework.Providers.Manager
{
  public static class ManagerProviderBaseEngineRequests
  {
    private static int _manageruser = 65;
    private static int _managerproductdetail = 395;
    private static int _managercategories = 462;


    public static int ManagerUserLookup
    {
      get { return _manageruser; }
      set { _manageruser = value; }
    }

    public static int ManagerCategories
    {
      get { return _managercategories; }
      set { _managercategories = value; }
    }

    public static int ManagerProductDetail
    {
      get { return _managerproductdetail; }
      set { _managerproductdetail = value; }
    }
  }
}
