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

    public bool GetGeneralEoi(string languageCode, out IGeneralEoiData generalEoiData)
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

          AddGtldButtonStatus(dotTypeEoiResponse.Categories, languageCode);

          generalEoiData = new GeneralEoiData(displayTime, dotTypeEoiResponse.Categories);
          success = true;
        }
      }
      catch (Exception ex)
      {
        var data = "languageCode: " + languageCode;
        var exception = new AtlantisException("DotTypeEoiProvider.GetGeneralEoi(languageCode)", "0", ex.Message + ex.StackTrace, data, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool GetGeneralEoi(int page, int entriesPerPage, int categoryId, string languageCode, out IGeneralGtldData generalGtldData)
    {
      var success = false;
      generalGtldData = null;

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
              var gtlds = new List<IDotTypeEoiGtld>();
              var gtldList = category.Gtlds;

              var startPos = (page - 1) * entriesPerPage;
              var endPos = startPos + entriesPerPage;
              if (endPos > gtldList.Count)
              {
                endPos = gtldList.Count;
              }

              for (int i = startPos; i < endPos; i++)
              {
                gtlds.Add(gtldList[i]);
              }

              AddGtldButtonStatus(gtlds, languageCode);

              var totalPages = (int)Math.Ceiling(gtldList.Count / (double)entriesPerPage);
              generalGtldData = new GeneralGtldData(displayTime, gtlds, totalPages);
              success = true;
            }
          }
        }
      }
      catch (Exception ex)
      {
        var data = "page: " + page + ", entriesPerPage: " + entriesPerPage + ", categoryId: " + categoryId + ", languageCode: " + languageCode;
        var exception = new AtlantisException("DotTypeEoiProvider.GetGeneralEoi(page, entriesPerPage, categoryId, languageCode)", "0", ex.Message + ex.StackTrace, data, null, null);
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

    public bool SearchEoi(string searchString, string languageCode, out IGeneralGtldData generalGtldData)
    {
      var success = false;
      generalGtldData = null;

      try
      {
        var request = new GeneralEoiJsonRequestData(languageCode);
        var response = (GeneralEoiJsonResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEoiEngineRequests.GeneralEoiJsonRequest);
        if (response.IsSuccess && response.DotTypeEoiResponse != null)
        {
          foreach (var category in response.DotTypeEoiResponse.Categories)
          {
            if (category.Name.ToLower() == "all categories" && category.Gtlds != null && category.Gtlds.Count > 0)
            {
              var gtlds = new List<IDotTypeEoiGtld>();
              foreach (var gtld in category.Gtlds)
              {
                if (gtld.Name.ToLower().Contains(searchString.ToLower()))
                {
                  gtlds.Add(gtld);
                }
              }

              AddGtldButtonStatus(gtlds, languageCode);

              generalGtldData = new GeneralGtldData(response.DotTypeEoiResponse.DisplayTime, gtlds, 0);
              success = true;
              break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("DotTypeEoiProvider.SearchEoi", "0", ex.Message + ex.StackTrace, searchString, null, null);
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

    private void AddGtldButtonStatus(IEnumerable<IDotTypeEoiCategory> categories, string languageCode)
    {
      foreach (var category in categories)
      {
        var subCategories = category.SubCategories;
        foreach (var subCategory in subCategories)
        {
          var gtlds = subCategory.Gtlds;
          AddGtldButtonStatus(gtlds, languageCode);
        }
      }
    }

    private void AddGtldButtonStatus(IEnumerable<IDotTypeEoiGtld> gtlds, string languageCode)
    {
      if (_shopperContext.Value.ShopperStatus == ShopperStatusType.Authenticated)
      {
        IShopperWatchListResponse shopperWatchListResponse;
        if (GetShopperWatchList(languageCode, out shopperWatchListResponse))
        {
          foreach (var gtld in gtlds)
          {
            if (shopperWatchListResponse.GtldIdDictionary.ContainsKey(gtld.Id))
            {
              gtld.ActionButtonType = ActionButtonTypes.DontWatch;
            }
            else
            {
              gtld.ActionButtonType = ActionButtonTypes.Watch;
            }
          }
        }
      }
    }
  }
}
