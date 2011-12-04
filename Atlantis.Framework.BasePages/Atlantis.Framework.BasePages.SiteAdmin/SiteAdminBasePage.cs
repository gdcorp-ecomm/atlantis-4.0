using Atlantis.Framework.Interface;
using System.Web.UI;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.BasePages.SiteAdmin.Security;

namespace Atlantis.Framework.BasePages.SiteAdmin
{
  public abstract class SiteAdminBasePage : Page
  {
    private ISiteContext _siteContext = null;
    protected virtual ISiteContext SiteContext
    {
      get
      {
        if (_siteContext == null)
        {
          _siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
        }
        return _siteContext;
      }
    }

    private IShopperContext _userContext = null;
    protected virtual IShopperContext UserContext
    {
      get
      {
        if (_userContext == null)
        {
          _userContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
        }
        return _userContext;
      }
    }

    private SiteAdminSecurityProvider _securityContext;
    protected SiteAdminSecurityProvider SecurityContext
    {
      get
      {
        if (_securityContext == null)
        {
          _securityContext = HttpProviderContainer.Instance.Resolve<SiteAdminSecurityProvider>();
        }
        return _securityContext;
      }
    }
  }
}
