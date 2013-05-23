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

    public bool AddToShopperWatchList(string displayTime, string gTldsXml, out string responseMessage)
    {
      var success = false;
      responseMessage = string.Empty;

      try
      {
        var request = new AddToShopperWatchListRequestData(ShopperContext.ShopperId, string.Empty, string.Empty, string.Empty, 0, displayTime, gTldsXml);
        var response = (AddToShopperWatchListResponseData) Atlantis.Framework.Engine.Engine.ProcessRequest(request, DotTypeEoiEngineRequests.AddToShopperWatchListRequest);
        if (response.IsSuccess)
        {
          success = true;
        }
        responseMessage = response.ResponseMessage;
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("DotTypeEoiProvider.AddToShopperWatchList", "0", ex.Message + ex.StackTrace, string.Empty, SiteContext, ShopperContext);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool RemoveFromShopperWatchList(string gTldsXml, out string responseMessage)
    {
      var success = false;
      responseMessage = string.Empty;

      try
      {
        var request = new RemoveFromShopperWatchListRequestData(ShopperContext.ShopperId, string.Empty, string.Empty, string.Empty, 0, gTldsXml);
        var response = (RemoveFromShopperWatchListResponseData)Atlantis.Framework.Engine.Engine.ProcessRequest(request, DotTypeEoiEngineRequests.RemoveFromShopperWatchListRequest);
        if (response.IsSuccess)
        {
          success = true;
        }
        responseMessage = response.ResponseMessage;
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("DotTypeEoiProvider.RemoveFromShopperWatchList", "0", ex.Message + ex.StackTrace, string.Empty, SiteContext, ShopperContext);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }
  }
}
