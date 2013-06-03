using System;
using System.Collections.Generic;
using Atlantis.Framework.DotTypeEoi.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.Providers.DotTypeEoi
{
  public class DotTypeEoiProvider : ProviderBase, IDotTypeEoiProvider
  {
    private readonly Lazy<IShopperContext> _shopperContext;

    public DotTypeEoiProvider(IProviderContainer container) : base(container)
    {
      _shopperContext = new Lazy<IShopperContext>(() => { return Container.Resolve<IShopperContext>(); });
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
        var exception = new AtlantisException("DotTypeEoiProvider.GetGeneralEoi", "0", ex.Message + ex.StackTrace, string.Empty, null, null);
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
        var request = new ShopperWatchListJsonRequestData(_shopperContext.Value.ShopperId);
        var response = (ShopperWatchListResponseData)Engine.Engine.ProcessRequest(request, DotTypeEoiEngineRequests.ShopperWatchListJsonRequest);
        if (response.IsSuccess)
        {
          shopperWatchListResponse = response.ShopperWatchListResponse;
          success = true;
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("DotTypeEoiProvider.GetShopperWatchList", "0", ex.Message + ex.StackTrace, string.Empty, null, _shopperContext.Value);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool AddToShopperWatchList(string displayTime, IList<IDotTypeEoiGtld> gTlds, out string responseMessage)
    {
      var success = false;
      responseMessage = string.Empty;

      try
      {
          var request = new AddToShopperWatchListRequestData(_shopperContext.Value.ShopperId, displayTime, gTlds);
        var response = (AddToShopperWatchListResponseData) Engine.Engine.ProcessRequest(request, DotTypeEoiEngineRequests.AddToShopperWatchListRequest);
        if (response.IsSuccess)
        {
          success = true;
        }
        responseMessage = response.ResponseMessage;
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("DotTypeEoiProvider.AddToShopperWatchList", "0", ex.Message + ex.StackTrace, string.Empty, null, _shopperContext.Value);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool RemoveFromShopperWatchList(IList<IDotTypeEoiGtld> gTlds, out string responseMessage)
    {
      var success = false;
      responseMessage = string.Empty;

      try
      {
          var request = new RemoveFromShopperWatchListRequestData(_shopperContext.Value.ShopperId, gTlds);
        var response = (RemoveFromShopperWatchListResponseData)Engine.Engine.ProcessRequest(request, DotTypeEoiEngineRequests.RemoveFromShopperWatchListRequest);
        if (response.IsSuccess)
        {
          success = true;
        }
        responseMessage = response.ResponseMessage;
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("DotTypeEoiProvider.RemoveFromShopperWatchList", "0", ex.Message + ex.StackTrace, string.Empty, null, _shopperContext.Value);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }
  }
}
