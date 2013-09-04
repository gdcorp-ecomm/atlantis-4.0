namespace Atlantis.Framework.Providers.Products
{
  public static class ProductProviderEngineRequests
  {
    public static int ProductOffer { get; set; }
    public static int UnifiedProductId { get; set; }
    public static int ProductInfo { get; set; }
    public static int NonUnifiedProductId { get; set; }
    public static int ProductNames { get; set; }

    static ProductProviderEngineRequests()
    {
      NonUnifiedProductId = 699;
      UnifiedProductId = 700;
      ProductOffer = 701;
      ProductInfo = 702;
      ProductNames = 724;
    }
  }
}
