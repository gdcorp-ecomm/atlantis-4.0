using System;
using System.Data;
using System.Security.Principal;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Manager.Interface;
using Atlantis.Framework.Manager.Interface.ManagerUser;
using Atlantis.Framework.Providers.Manager.Interface;

namespace Atlantis.Framework.Providers.Manager
{
  public class ManagerProvider : ProviderBase, IManagerProvider
  {
    #region Properties
    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<IShopperContext> _shopperContext; 
    private readonly Lazy<ManagerCategoriesResponseData> _managerCategories;
    private readonly Lazy<ManagerUserLookupResponseData> _managerUserLookup;
    #endregion 

    public ManagerProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() => Container.Resolve<ISiteContext>());
      _shopperContext = new Lazy<IShopperContext>(() => Container.Resolve<IShopperContext>());
      _managerCategories = new Lazy<ManagerCategoriesResponseData>(LoadManagerCategories);
      _managerUserLookup = new Lazy<ManagerUserLookupResponseData>(LoadManagerUser);
    }

    #region Public ManagerCategories Methods
    public bool IsCurrentUserInAnyRole(params int[] managerCategories)
    {
      return _managerCategories.Value.HasAnyManagerCategories(managerCategories);
    }

    public bool IsCurrentUserInAllRoles(params int[] managerCategories)
    {
      return _managerCategories.Value.HasAllManagerCategories(managerCategories);
    }

    public bool HasManagerCategory(int category)
    {
      return _managerCategories.Value.HasManagerCategory(category);
    }

    public bool HasManagerAttribute(string key)
    {
      return _managerCategories.Value.HasManagerAttribute(key);
    }

    public bool TryGetManagerAttribute(string key, out string value)
    {
      return _managerCategories.Value.TryGetManagerAttribute(key, out value);
    }
    #endregion

    #region Public ManagerUserLookup Methods
    public ManagerUserData GetManagerUserData()
    {
      return _managerUserLookup.Value.ManagerUser;
    }
    public bool HasManagerUserData()
    {
      return _managerUserLookup.Value != null;
    }
    #endregion

    #region Public Manager Product Detail Methods
    public DataTable GetProductCatalogDetails(decimal pfid, int adminFlag)
    {
      var dt = new DataTable();
      ManagerGetProductDetailResponseData response;
      try
      {
        var userId = HasManagerUserData() ? Convert.ToInt32(GetManagerUserData().UserId) : 0;
        var request = new ManagerGetProductDetailRequestData(_shopperContext.Value.ShopperId
          , HttpContext.Current.Request.Url.ToString()
          , string.Empty
          , _siteContext.Value.Pathway
          , _siteContext.Value.PageCount
          , pfid
          , _siteContext.Value.PrivateLabelId
          , adminFlag
          , userId);
        response = Engine.Engine.ProcessRequest(request, ManagerProviderBaseEngineRequests.ManagerProductDetail) as ManagerGetProductDetailResponseData;
      }
      catch (Exception ex)
      {
        var managerException = new AtlantisException("ManagerProvider.LoadManagerProductDetails", "0", string.Concat("Cannot load Product Catalog Details: ", ex.Message), string.Empty, _siteContext.Value, _shopperContext.Value);
        Engine.Engine.LogAtlantisException(managerException);
        response = new ManagerGetProductDetailResponseData();
      }
      if (response.IsSuccess)
      {
        dt = response.ProductCatalogDetails;
      }
      return dt;
    }
    #endregion

    #region Private Methods

    private ManagerUserLookupResponseData LoadManagerUser()
    {
      ManagerUserLookupResponseData response;
      try
      {
        string userId;
        if (GetWindowsUserId(out userId))
        {
          var request = new ManagerUserLookupRequestData(_shopperContext.Value.ShopperId
            , HttpContext.Current.Request.Url.ToString()
            , string.Empty
            , _siteContext.Value.Pathway
            , _siteContext.Value.PageCount
            , userId);
          response = DataCache.DataCache.GetProcessRequest(request, ManagerProviderBaseEngineRequests.ManagerUserLookup) as ManagerUserLookupResponseData;
        }
        else
        {
          throw new Exception("ManagerProvider.GetManagerUserData() -- Windows identity cannot be determined.");
        }
      }
      catch (Exception ex)
      {
        var managerException = new AtlantisException("ManagerProvider.GetManagerUserData()", "403", string.Concat("Error Looking up Manager User. ", ex.Message), string.Empty, _siteContext.Value, _shopperContext.Value);
        Engine.Engine.LogAtlantisException(managerException);
        response = ManagerUserLookupResponseData.Empty;
      }
      return response;
    }

    private ManagerCategoriesResponseData LoadManagerCategories()
    {
      ManagerCategoriesResponseData result;
      try
      {
        int managerUserIdInt;
        int.TryParse(_siteContext.Value.Manager.ManagerUserId, out managerUserIdInt);
        var request = new ManagerCategoriesRequestData(_shopperContext.Value.ShopperId
          , HttpContext.Current.Request.Url.ToString()
          , string.Empty
          , _siteContext.Value.Pathway
          , _siteContext.Value.PageCount
          , managerUserIdInt);
        result = DataCache.DataCache.GetProcessRequest(request, ManagerProviderBaseEngineRequests.ManagerCategories) as ManagerCategoriesResponseData;
      }
      catch
      {
        result = ManagerCategoriesResponseData.Empty;
      }
      return result;
    }

    private bool GetWindowsUserId(out string userId)
    {
      var result = false;
      userId = null;

      var windowsIdentity = HttpContext.Current.User.Identity as WindowsIdentity;
      if ((windowsIdentity != null) && (windowsIdentity.IsAuthenticated))
      {
        var nameParts = windowsIdentity.Name.Split('\\');
        if (nameParts.Length == 2)
        {
          userId = nameParts[1];
          result = true;
        }
        else
        {
          var managerException = new AtlantisException("ManagerProvider.GetWindowsUserId", "403", "Windows identity cannot be determined.", string.Empty, _siteContext.Value, _shopperContext.Value);
          Engine.Engine.LogAtlantisException(managerException);
        }
      }

      if (!result)
      {
        var managerException = new AtlantisException("ManagerProvider.GetWindowsUserId", "403", "Windows identity cannot be determined - Windows authentication issue.", string.Empty, _siteContext.Value, _shopperContext.Value);
        Engine.Engine.LogAtlantisException(managerException);
      }

      return result;
    }
    #endregion 
  }
}
