namespace Atlantis.Framework.Providers.DotTypeEoi
{
  public static class DotTypeEoiEngineRequests
  {
    static DotTypeEoiEngineRequests()
    {
      GeneralEoiJsonRequest = 698;
      ShopperWatchListJsonRequest = 703;
      AddToShopperWatchListRequest = 704;
      RemoveFromShopperWatchListRequest = 705;
    }

    public static int GeneralEoiJsonRequest { get; set; }
    public static int ShopperWatchListJsonRequest { get; set; }
    public static int AddToShopperWatchListRequest { get; set; }
    public static int RemoveFromShopperWatchListRequest { get; set; }
  }
}
