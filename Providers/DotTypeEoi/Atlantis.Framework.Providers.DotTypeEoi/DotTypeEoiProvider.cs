using System;
using Atlantis.Framework.DotTypeEoi.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.Providers.DotTypeEoi
{
  public class DotTypeEoiProvider : ProviderBase, IDotTypeEoiProvider
  {
    private readonly IProviderContainer _container;

    private ISiteContext SiteContext
    {
      get
      {
        return _container.Resolve<ISiteContext>();
      }
    }

    private IShopperContext ShopperContext
    {
      get
      {
        return _container.Resolve<IShopperContext>();
      }
    }

    public DotTypeEoiProvider(IProviderContainer container) : base(container)
    {
      _container = container;
    }

    public bool GetGeneralEoi(out IDotTypeEoiResponse dotTypeEoiResponse)
    {
      var success = false;
      dotTypeEoiResponse = null;

      try
      {
        var request = new GeneralEoiJsonRequestData();
        var response = (GeneralEoiJsonResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEoiEngineRequests.GeneralEoiJsonRequest);
        if (response.IsSuccess)
        {
          dotTypeEoiResponse = response.DotTypeEoiResponse;
          success = true;
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("DotTypeEoiProvider.GetGeneralEoi", "0", ex.Message + ex.StackTrace, string.Empty, SiteContext, ShopperContext);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool GetShopperWatchList(out IShopperWatchListResponse shopperWatchListResponse)
    {
      var success = false;
      shopperWatchListResponse = null;

      try
      {
        var request = new ShopperWatchListJsonRequestData();
        var response = (ShopperWatchListResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEoiEngineRequests.ShopperWatchListJsonRequest);
        if (response.IsSuccess)
        {
          shopperWatchListResponse = response.ShopperWatchListResponse;
          success = true;
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("DotTypeEoiProvider.GetShopperWatchList", "0", ex.Message + ex.StackTrace, string.Empty, SiteContext, ShopperContext);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }
  }
}
