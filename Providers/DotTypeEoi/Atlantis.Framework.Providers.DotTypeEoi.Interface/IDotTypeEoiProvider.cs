namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IDotTypeEoiProvider
  {
    bool GetGeneralEoi(out IDotTypeEoiResponse dotTypeEoi);
    bool GetShopperWatchList(string shopperId, out IShopperWatchListResponse shopperWatchListResponse);
    bool AddToShopperWatchList(string displayTime, string gTldsXml, out string responseMessage);
    bool RemoveFromShopperWatchList(string gTldsXml, out string responseMessage);
  }
}
