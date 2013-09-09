namespace Atlantis.Framework.Providers.Shopper
{
  public static class ShopperProviderEngineRequests
  {
    public static int GetShopper { get; set; }
    public static int ShopperPriceType { get; set; }
    public static int VerifyShopper { get; set; }
    public static int CreateShopper { get; set; }
    public static int UpdateShopper { get; set; }

    static ShopperProviderEngineRequests()
    {
      GetShopper = 735;
      ShopperPriceType = 736;
      VerifyShopper = 737;
      CreateShopper = 738;
      UpdateShopper = 739;
    }
  }
}
