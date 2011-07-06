using System.Web;
using System.Web.SessionState;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.BasePages
{
  public abstract class AtlantisContextBaseHttpHandler : IHttpHandler, IRequiresSessionState
  {
    private ISiteContext _siteContext;
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

    private IShopperContext _shopperContext;
    protected virtual IShopperContext ShopperContext
    {
      get
      {
        if (_shopperContext == null)
        {
          _shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
        }
        return _shopperContext;
      }
    }

    protected abstract void HandleHttpRequest(HttpContext context);

    public void ProcessRequest(HttpContext context)
    {
      HandleHttpRequest(context);
    }

    public virtual bool IsReusable
    {
      get { return false; }
    }
  }
}
