using System.Security.Principal;
using System.Web;
using Atlantis.Framework.Interface;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using Atlantis.Framework.ManagerCategories.Interface;
using System;

namespace Atlantis.Framework.BasePages.SiteAdmin.Security
{
  public class SiteAdminSecurityProvider : ProviderBase
  {
    private Lazy<ISiteContext> _siteContext;
    private Lazy<ManagerCategoriesResponseData> _managerCategories;

    public SiteAdminSecurityProvider(IProviderContainer container)
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() =>
      {
        return Container.Resolve<ISiteContext>();
      });

      _managerCategories = new Lazy<ManagerCategoriesResponseData>(() =>
        {
          return LoadManagerCategories();
        });
    }

    private ManagerCategoriesResponseData LoadManagerCategories()
    {
      ManagerCategoriesResponseData result = null;
      try
      {
        int managerUserIdInt;
        int.TryParse(_siteContext.Value.Manager.ManagerUserId, out managerUserIdInt);
        ManagerCategoriesRequestData request = new ManagerCategoriesRequestData(string.Empty, HttpContext.Current.Request.Url.ToString(), string.Empty, string.Empty, 0, managerUserIdInt);
        result = (ManagerCategoriesResponseData)DataCache.DataCache.GetProcessRequest(request, SiteAdminBaseEngineRequests.ManagerCategories);
      }
      catch
      {
        result = ManagerCategoriesResponseData.Empty;
      }
      return result;
    }

    public bool IsCurrentUserInRole(int managerCategory)
    {
      return _managerCategories.Value.HasManagerCategory(managerCategory);
    }

    [Obsolete("Please use the method that does not require ISiteContext")]
    public bool IsCurrentUserInAnyRole(ISiteContext siteContext, params int[] managerCategories)
    {
      return _managerCategories.Value.HasAnyManagerCategories(managerCategories);
    }

    [Obsolete("Please use the method that does not require ISiteContext")]
    public bool IsCurrentUserInAllRoles(ISiteContext siteContext, params int[] managerCategories)
    {
      return _managerCategories.Value.HasAllManagerCategories(managerCategories);
    }

    public bool IsCurrentUserInAnyRole(params int[] managerCategories)
    {
      return _managerCategories.Value.HasAnyManagerCategories(managerCategories);
    }

    public bool IsCurrentUserInAllRoles(params int[] managerCategories)
    {
      return _managerCategories.Value.HasAllManagerCategories(managerCategories);
    }

  }
}
