using Atlantis.Framework.Interface;
using System.Web;
using System.Security.Principal;

namespace Atlantis.Framework.BasePages.SiteAdmin.Providers
{
  public class SiteAdminUserProvider : ProviderBase, IShopperContext
  {
    public SiteAdminUserProvider(IProviderContainer container) : base(container)
    {
    }

    #region IShopperContext Members

    public string ShopperId
    {
      get
      {
        string result = "Unknown";
        if (HttpContext.Current != null)
        {
          WindowsIdentity windowsUser = HttpContext.Current.User.Identity as WindowsIdentity;
          if (windowsUser != null)
          {
            result = windowsUser.Name;
          }
          else
          {
            result = HttpContext.Current.Request.UserHostAddress;
          }
        }
        return result;
      }
    }

    public ShopperStatusType ShopperStatus
    {
      get 
      {
        ShopperStatusType result = ShopperStatusType.Public;
        if (HttpContext.Current != null)
        {
          WindowsIdentity windowsUser = HttpContext.Current.User.Identity as WindowsIdentity;
          if (windowsUser != null)
          {
            result = ShopperStatusType.Authenticated;
          }
        }
        return result;
      }
    }

    public void ClearShopper()
    {
      return;
    }

    public bool SetLoggedInShopper(string shopperId)
    {
      return false;
    }

    public bool SetLoggedInShopperWithCookieOverride(string shopperId)
    {
      return false;
    }

    public void SetNewShopper(string shopperId)
    {
      return;
    }

    public int ShopperPriceType
    {
      get { return 0; }
    }

    #endregion
  }
}
