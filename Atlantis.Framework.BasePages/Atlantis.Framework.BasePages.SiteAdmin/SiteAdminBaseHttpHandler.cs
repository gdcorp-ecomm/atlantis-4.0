using System.Web;
using System.Web.SessionState;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.BasePages.SiteAdmin
{
  public abstract class SiteAdminBaseHttpHandler : IHttpHandler, IRequiresSessionState
  {
    protected abstract void HandleHttpRequest(HttpContext context);

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

    #region IHttpHandler Members

    public void ProcessRequest(HttpContext context)
    {
      HandleHttpRequest(context);
    }

    public virtual bool IsReusable
    {
      get { return false; }
    }

    #endregion
  }
}
