namespace Atlantis.Framework.Providers.DotTypeEoi
{
  public static class DotTypeEoiEngineRequests
  {
    static DotTypeEoiEngineRequests()
    {
      GeneralEoiJsonRequest = 698;
      ShopperWatchListJsonRequest = 703;
    }

    public static int GeneralEoiJsonRequest { get; set; }
    public static int ShopperWatchListJsonRequest { get; set; }
  }
}
