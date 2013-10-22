using System;
using System.Collections.Generic;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.DotTypeEoi.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;
using Atlantis.Framework.Providers.Localization.Interface;

namespace Atlantis.Framework.Providers.DotTypeEoi
{
  public class DotTypeEoiProvider : ProviderBase, IDotTypeEoiProvider
  {
    private const string ENGLISH = "en";
    private const string SPANISH = "sp";

    private readonly Lazy<IShopperContext> _shopperContext;
    private readonly Lazy<IManagerContext> _managerContext;
    private readonly IList<ICategoryData> _categoryList = new List<ICategoryData>();

    private string _fullLanguage;
    private string FullLanguage
    {
      get { return _fullLanguage ?? (_fullLanguage = DetermineFullLanguage()); }
    }

    private bool IsAuthenticated
    {
      get
      {
        return _shopperContext.Value.ShopperStatus == ShopperStatusType.PartiallyTrusted ||
               _shopperContext.Value.ShopperStatus == ShopperStatusType.Authenticated;
      }
    }

    public DotTypeEoiProvider(IProviderContainer container) : base(container)
    {
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
      _managerContext = new Lazy<IManagerContext>(() => Container.Resolve<IManagerContext>());
    }

    private IDotTypeProvider _dotTypeProvider;
    private IDotTypeProvider DotTypeProvider
    {
      get { return _dotTypeProvider ?? (_dotTypeProvider = Container.Resolve<IDotTypeProvider>()); }
    }

    private string DetermineFullLanguage()
    {
      string fullLanguage = ENGLISH;

      ILocalizationProvider localizationProvider;
      if (Container.TryResolve(out localizationProvider))
      {
        fullLanguage = localizationProvider.FullLanguage;
      }

      if (IsSpanishTransPerfectProxy(fullLanguage))
      {
        fullLanguage = ENGLISH;
      }

      return fullLanguage;
    }

    private bool IsSpanishTransPerfectProxy(string fullLanguage)
    {
      bool isSpanishTransPerfectProxy = false;

      IProxyContext proxyContext;
      if (Container.TryResolve(out proxyContext))
      {
        IProxyData languageProxy;
        isSpanishTransPerfectProxy = fullLanguage.ToLowerInvariant().StartsWith(SPANISH) &&
                                     proxyContext.TryGetActiveProxy(ProxyTypes.TransPerfectTranslation, out languageProxy);
      }

      return isSpanishTransPerfectProxy;
    }

    public bool GetGeneralEoi(out IGeneralEoiData generalEoiData)
    {
      var success = false;
      generalEoiData = null;

      try
      {
        var request = new GeneralEoiJsonRequestData(FullLanguage);
        var response = (GeneralEoiJsonResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEoiEngineRequests.GeneralEoiJsonRequest);

        if (response.IsSuccess && response.DotTypeEoiResponse != null)
        {
          var dotTypeEoiResponse = response.DotTypeEoiResponse;
          var displayTime = dotTypeEoiResponse.DisplayTime;

          AddGtldButtonStatus(dotTypeEoiResponse.Categories);

          generalEoiData = new GeneralEoiData(displayTime, dotTypeEoiResponse.Categories);
          success = true;
        }
      }
      catch (Exception ex)
      {
        var data = "languageCode: " + FullLanguage;
        var exception = new AtlantisException("DotTypeEoiProvider.GetGeneralEoi(languageCode)", "0", ex.Message + ex.StackTrace, data, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool GetGeneralEoi(int page, int entriesPerPage, int categoryId, out IGeneralGtldData generalGtldData)
    {
      var success = false;
      generalGtldData = null;

      try
      {
        var request = new GeneralEoiJsonRequestData(FullLanguage);
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

              AddGtldButtonStatus(gtlds);

              var totalPages = (int)Math.Ceiling(gtldList.Count / (double)entriesPerPage);
              generalGtldData = new GeneralGtldData(displayTime, gtlds, totalPages);
              success = true;
            }
          }
        }
      }
      catch (Exception ex)
      {
        var data = "page: " + page + ", entriesPerPage: " + entriesPerPage + ", categoryId: " + categoryId + ", languageCode: " + FullLanguage;
        var exception = new AtlantisException("DotTypeEoiProvider.GetGeneralEoi(page, entriesPerPage, categoryId, languageCode)", "0", ex.Message + ex.StackTrace, data, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool GetGeneralEoi(int categoryId, out IGeneralGtldData generalGtldData)
    {
      var success = false;
      generalGtldData = null;

      try
      {
        var request = new GeneralEoiJsonRequestData(FullLanguage);
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

              foreach (var gtld in category.Gtlds)
              {
                gtlds.Add(gtld);
              }

              AddGtldButtonStatus(gtlds);

              generalGtldData = new GeneralGtldData(displayTime, gtlds, 1);
              success = true;
            }
          }
        }
      }
      catch (Exception ex)
      {
        var data = "categoryId: " + categoryId + ", languageCode: " + FullLanguage;
        var exception = new AtlantisException("DotTypeEoiProvider.GetGeneralEoi(categoryId, languageCode)", "0", ex.Message + ex.StackTrace, data, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool GetGeneralEoiCategoryList(out IList<ICategoryData> categoryList)
    {
      var success = false;
      categoryList = null;

      try
      {
        var request = new GeneralEoiJsonRequestData(FullLanguage);
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

    public bool SearchEoi(string searchString, out IGeneralGtldData generalGtldData)
    {
      var success = false;
      generalGtldData = null;

      try
      {
        var request = new GeneralEoiJsonRequestData(FullLanguage);
        var response = (GeneralEoiJsonResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEoiEngineRequests.GeneralEoiJsonRequest);

        if (response.IsSuccess && response.DotTypeEoiResponse != null)
        {
          IEnumerable<IDotTypeEoiGtld> gtlds = response.DotTypeEoiResponse.AllGtlds.Values;
          IList<IDotTypeEoiGtld> matchingGtlds = new List<IDotTypeEoiGtld>(response.DotTypeEoiResponse.AllGtlds.Count);

          foreach (var gtld in gtlds)
          {
            if (gtld.Name.ToLower().Contains(searchString.ToLower()))
            {
              matchingGtlds.Add(gtld);
            }
          }

          AddGtldButtonStatus(matchingGtlds);

          generalGtldData = new GeneralGtldData(response.DotTypeEoiResponse.DisplayTime, matchingGtlds, 0);
          success = true;
        }
      }
      catch (Exception ex)
      {
        var exception = new AtlantisException("DotTypeEoiProvider.SearchEoi", "0", ex.Message + ex.StackTrace, searchString, null, null);
        Engine.Engine.LogAtlantisException(exception);
      }

      return success;
    }

    public bool GetShopperWatchList(out IShopperWatchListResponse shopperWatchListResponse)
    {
      var success = false;
      shopperWatchListResponse = null;

      if (_managerContext.Value.IsManager || IsAuthenticated)
      {
        try
        {
          string shopperId = _managerContext.Value.IsManager ? _managerContext.Value.ManagerShopperId : _shopperContext.Value.ShopperId;
          var request = new ShopperWatchListJsonRequestData(shopperId, FullLanguage);
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
      }

      return success;
    }

    public bool AddToShopperWatchList(string gtldName, out string responseMessage)
    {
      var success = false;
      responseMessage = string.Empty;

      if (_managerContext.Value.IsManager || IsAuthenticated)
      {
        try
        {
          string shopperId = _managerContext.Value.IsManager ? _managerContext.Value.ManagerShopperId : _shopperContext.Value.ShopperId;

          var gtldsToAdd = new List<IDotTypeEoiGtld>(1);
          var generalEoiRequest = new GeneralEoiJsonRequestData(FullLanguage);
          var generalEoiResponse = (GeneralEoiJsonResponseData)DataCache.DataCache.GetProcessRequest(generalEoiRequest, DotTypeEoiEngineRequests.GeneralEoiJsonRequest);

          if (generalEoiResponse.IsSuccess && generalEoiResponse.DotTypeEoiResponse != null)
          {
            IDotTypeEoiGtld gtldToAdd;
            generalEoiResponse.DotTypeEoiResponse.AllGtlds.TryGetValue(gtldName, out gtldToAdd);
            gtldsToAdd.Add(gtldToAdd);

            var addToWatchListRequest = new AddToShopperWatchListRequestData(shopperId, generalEoiResponse.DotTypeEoiResponse.DisplayTime, gtldsToAdd, FullLanguage);
            var addToWatchListResponse = (AddToShopperWatchListResponseData)Engine.Engine.ProcessRequest(addToWatchListRequest, DotTypeEoiEngineRequests.AddToShopperWatchListRequest);
            if (addToWatchListResponse.IsSuccess)
            {
              success = true;
            }

            responseMessage = addToWatchListResponse.ResponseMessage;
          }
        }
        catch (Exception ex)
        {
          var exception = new AtlantisException("DotTypeEoiProvider.AddToShopperWatchList", "0",
                                                ex.Message + ex.StackTrace, string.Empty, null, _shopperContext.Value);
          Engine.Engine.LogAtlantisException(exception);
        }
      }

      return success;
    }

    public bool RemoveFromShopperWatchList(string gtldName, out string responseMessage)
    {
      var success = false;
      responseMessage = string.Empty;

      if (_managerContext.Value.IsManager || IsAuthenticated)
      {
        try
        {
          string shopperId = _managerContext.Value.IsManager ? _managerContext.Value.ManagerShopperId : _shopperContext.Value.ShopperId;

          var gtldsToRemove = new List<IDotTypeEoiGtld>(1);
          var generalEoiRequest = new GeneralEoiJsonRequestData(FullLanguage);
          var generalEoiResponse = (GeneralEoiJsonResponseData)DataCache.DataCache.GetProcessRequest(generalEoiRequest, DotTypeEoiEngineRequests.GeneralEoiJsonRequest);

          if (generalEoiResponse.IsSuccess && generalEoiResponse.DotTypeEoiResponse != null)
          {
            IDotTypeEoiGtld gtldToRemove;
            generalEoiResponse.DotTypeEoiResponse.AllGtlds.TryGetValue(gtldName, out gtldToRemove);
            gtldsToRemove.Add(gtldToRemove);

            var removeFromWatchListRequest = new RemoveFromShopperWatchListRequestData(shopperId, gtldsToRemove);
            var removeFromWatchListResponse = (RemoveFromShopperWatchListResponseData)Engine.Engine.ProcessRequest(removeFromWatchListRequest, DotTypeEoiEngineRequests.RemoveFromShopperWatchListRequest);
            if (removeFromWatchListResponse.IsSuccess)
            {
              success = true;
            }

            responseMessage = removeFromWatchListResponse.ResponseMessage;
          }
        }
        catch (Exception ex)
        {
          var exception = new AtlantisException("DotTypeEoiProvider.RemoveFromShopperWatchList", "0",
                                                ex.Message + ex.StackTrace, string.Empty, null, _shopperContext.Value);
          Engine.Engine.LogAtlantisException(exception);
        }
      }

      return success;
    }

    private void AddGtldButtonStatus(IEnumerable<IDotTypeEoiCategory> categories)
    {
      foreach (var category in categories)
      {
        var subCategories = category.SubCategories;
        foreach (var subCategory in subCategories)
        {
          foreach (var gtld in subCategory.Gtlds)
          {
            var dotTypeInfo = DotTypeProvider.GetDotTypeInfo(gtld.Name);
            if (dotTypeInfo != null && dotTypeInfo.GetType().Name != "InvalidDotType")
            {
              var phase = dotTypeInfo.GetLaunchPhase(LaunchPhases.GeneralAvailability);
              if (phase != null)
              {
                if (phase.LivePeriod != null && phase.LivePeriod.IsActive)
                {
                  gtld.ActionButtonType = ActionButtonTypes.Register;
                }
              }
              
              if (dotTypeInfo.IsPreRegPhaseActive)
              {
                gtld.ActionButtonType = ActionButtonTypes.PreRegister;
              }
            }
            else if (_managerContext.Value.IsManager || IsAuthenticated)
            {
              IShopperWatchListResponse shopperWatchListResponse;
              if (GetShopperWatchList(out shopperWatchListResponse))
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

    private void AddGtldButtonStatus(IEnumerable<IDotTypeEoiGtld> gtlds)
    {
      foreach (var gtld in gtlds)
      {
        var dotTypeInfo = DotTypeProvider.GetDotTypeInfo(gtld.Name);
        if (dotTypeInfo != null && dotTypeInfo.GetType().Name != "InvalidDotType")
        {
          var phase = dotTypeInfo.GetLaunchPhase(LaunchPhases.GeneralAvailability);
          if (phase != null)
          {
            if (phase.LivePeriod != null && phase.LivePeriod.IsActive)
            {
              gtld.ActionButtonType = ActionButtonTypes.Register;
            }
          }
          
          if (dotTypeInfo.IsPreRegPhaseActive)
          {
            gtld.ActionButtonType = ActionButtonTypes.PreRegister;
          }
        }
        else if (_managerContext.Value.IsManager || IsAuthenticated)
        {
          IShopperWatchListResponse shopperWatchListResponse;
          if (GetShopperWatchList(out shopperWatchListResponse))
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
