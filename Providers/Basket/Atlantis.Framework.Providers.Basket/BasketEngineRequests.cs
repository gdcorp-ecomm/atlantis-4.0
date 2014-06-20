namespace Atlantis.Framework.Providers.Basket
{
  public static class BasketEngineRequests
  {
    public static int AddItems { get; set; }
    public static int ItemCount { get; set; }
    public static int DeleteItem { get; set; }

    static BasketEngineRequests()
    {
      AddItems = 746;
      ItemCount = 760;
      DeleteItem = 836;
    }
  }
}
