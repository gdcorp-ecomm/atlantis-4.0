using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.TargetedShopperService.Interface;
using Atlantis.Framework.TargetedShopperService.Interface;

namespace Atlantis.Framework.Providers.TargetedShopperService
{
  public class TargetedShopperServiceProvider : ProviderBase, ITargetedShopperServiceProvider 
  {
    public TargetedShopperServiceProvider(IProviderContainer container) : base(container)
    {
    }

    public string ShopperDecodeXid(string encodedXid)
    {
      ShopperXidDecodeRequestData  request = new ShopperXidDecodeRequestData(encodedXid);
      ShopperXidDecodeResponseData response =
        SessionCache.SessionCache.GetProcessRequest<ShopperXidDecodeResponseData>(request,TargetedShopperServiceEngineRequest.DecodeXidRequestId);
      return response.ResultStatus == "Success" ? response.ResultData : string.Empty;
    }

    public string ShopperEncodeXid(string decodedXid)
    {
      ShopperXidEncodeRequestData request = new ShopperXidEncodeRequestData(decodedXid);
      ShopperXidEncodeResponseData response =
        SessionCache.SessionCache.GetProcessRequest<ShopperXidEncodeResponseData>(request,TargetedShopperServiceEngineRequest.EncodeXidRequestId);
      return response.ResultStatus == "Success" ? response.ResultData : string.Empty;
    }
  }
}
