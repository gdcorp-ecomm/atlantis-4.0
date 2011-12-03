using System.Web.UI;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.BasePages.SiteAdmin
{
  public class SiteAdminBaseMaster : MasterPage
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
  }
}
