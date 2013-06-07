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
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
    }

    private readonly IList<ICategoryData> _categoryList = new List<ICategoryData>();

    public bool GetGeneralEoi(int page, int entriesPerPage, int categoryId, string languageCode, out IGeneralEoiData generalEoiData)
    {
      var success = false;
      generalEoiData = null;

      try
      {
        var request = new GeneralEoiJsonRequestData(languageCode);
        var response = (GeneralEoiJsonResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEoiEngineRequests.GeneralEoiJsonRequest);
        if (response.IsSuccess && response.DotTypeEoiResponse != null)
        {
          var dotTypeEoiResponse = response.DotTypeEoiResponse;
          var displayTime = dotTypeEoiResponse.DisplayTime;
          foreach (var category in dotTypeEoiResponse.Categories)
          {
            if (categoryId == category.CategoryId)
            {
              var gTlds = new List<IDotTypeEoiGtld>();
              var gTldList = category.Gtlds;

              int startPos = (page - 1) * entriesPerPage;
              int endPos = startPos + entriesPerPage;
              if (endPos > gTldList.Count)
              {
                endPos = gTldList.Count;
              }

              for (int i = startPos; i < endPos; i++)
              {
                gTlds.Add(gTldList[i]);
              }

              if (_shopperContext.Value.ShopperStatus == ShopperStatusType.Authenticated)
              {
                IShopperWatchListResponse shopperWatchListResponse;
                if (GetShopperWatchList(languageCode, out shopperWatchListResponse))
                {
                  foreach (var gTld in gTlds)
                  {
                    if (shopperWatchListResponse.GtldIdDictionary.ContainsKey(gTld.Id))
                    {
                      gTld.ActionButtonType = ActionButtonTypes.DontWatch;
                    }
                    else
                    {
                      gTld.ActionButtonType = ActionButtonTypes.Watch;
                    }
                  }
                }
              }

              var totalPages = (int)Math.Ceiling(gTldList.Count/(double)entriesPerPage);
              generalEoiData = new GeneralEoiData(displayTime, gTlds, totalPages);
              success = true;
            }
          }
        }
      }
      catch (Exception ex)
      {
        var data = "page: " + page + ", entriesPerPage: " + entriesPerPage + ", categoryId: " + categoryId;
        var exception = new AtlantisException("DotTypeEoiProvider.GetGeneralEoi(page, entriesPerPage, categoryId)", "0", ex.Message + ex.StackTrace, data, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool GetGeneralEoiCategoryList(string languageCode, out IList<ICategoryData> categoryList)
    {
      var success = false;
      categoryList = null;

      try
      {
        var request = new GeneralEoiJsonRequestData(languageCode);
        var response = (GeneralEoiJsonResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEoiEngineRequests.GeneralEoiJsonRequest);
        if (response.IsSuccess && response.DotTypeEoiResponse != null)
        {
          foreach (var category in response.DotTypeEoiResponse.Categories)
          {
            _categoryList.Add(new CategoryData(category));
          }
          categoryList = _categoryList;
          success = true;
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("DotTypeEoiProvider.GetGeneralEoiCategoryList", "0", ex.Message + ex.StackTrace, string.Empty, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool GetShopperWatchList(string languageCode, out IShopperWatchListResponse shopperWatchListResponse)
    {
      var success = false;
      shopperWatchListResponse = null;

      try
      {
        var request = new ShopperWatchListJsonRequestData(_shopperContext.Value.ShopperId, languageCode);
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

    public bool AddToShopperWatchList(string displayTime, IList<IDotTypeEoiGtld> gTlds, string languageCode, out string responseMessage)
    {
      var success = false;
      responseMessage = string.Empty;

      try
      {
          var request = new AddToShopperWatchListRequestData(_shopperContext.Value.ShopperId, displayTime, gTlds, languageCode);
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
