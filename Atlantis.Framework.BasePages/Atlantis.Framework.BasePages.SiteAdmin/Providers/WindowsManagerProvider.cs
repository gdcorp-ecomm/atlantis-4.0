using Atlantis.Framework.Interface;
using System;
using Atlantis.Framework.ManagerUser.Interface;
using System.Security.Principal;

namespace Atlantis.Framework.BasePages.SiteAdmin.Providers
{
  public class WindowsManagerProvider : ProviderBase, IManagerContext
  {
    Lazy<IShopperContext> _shopperContext;
    Lazy<ISiteContext> _siteContext;

    public WindowsManagerProvider(IProviderContainer container) : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(() =>
      {
        return Container.Resolve<ISiteContext>();
      });

      _shopperContext = new Lazy<IShopperContext>(() =>
      {
        return Container.Resolve<IShopperContext>();
      });
    }

    #region IManagerContext Members

    public bool IsManager
    {
      get { return true; }
    }

    public string ManagerUserId
    {
      get { return string.Empty; }
    }

    public string ManagerUserName
    {
      get { return string.Empty; }
    }

    public System.Collections.Specialized.NameValueCollection ManagerQuery
    {
      get { return new System.Collections.Specialized.NameValueCollection(); }
    }

    public string ManagerShopperId
    {
      get { return _shopperContext.Value.ShopperId; }
    }

    public int ManagerPrivateLabelId
    {
      get { return 0; }
    }

    public int ManagerContextId
    {
      get { return ContextIds.SiteAdmin; }
    }

    #endregion

    private bool GetWindowsUserAndDomain(out string domain, out string userId)
    {
      bool result = false;
      domain = null;
      userId = null;

      string[] nameParts = _shopperContext.Value.ShopperId.Split('\\');
      if (nameParts.Length == 2)
      {
        domain = nameParts[0];
        userId = nameParts[1];
        result = true;
      }
      else
      {
        AtlantisException managerException = new AtlantisException(
          "WindowsManagerProvider.GetWindowsUserAndDomain", "403", "Windows identity cannot be determined.", _shopperContext.Value.ShopperId, _siteContext.Value, _shopperContext.Value);
        Engine.Engine.LogAtlantisException(managerException);
      }

      return result;
    }

  }
}
