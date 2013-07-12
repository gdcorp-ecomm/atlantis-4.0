namespace Atlantis.Framework.Providers.TargetedShopperService.Interface
{
  public interface ITargetedShopperServiceProvider
  {
    string ShopperDecodeXid(string encodedXid);
    string ShopperEncodeXid(string decodedXid);
  }
}
