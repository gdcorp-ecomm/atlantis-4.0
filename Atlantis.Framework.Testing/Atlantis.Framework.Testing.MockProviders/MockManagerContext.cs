using Atlantis.Framework.Interface;
using System.Collections.Specialized;
using System.Web;

namespace Atlantis.Framework.Testing.MockProviders
{
  public class MockManagerContext : ProviderBase, IManagerContext
  {
    NameValueCollection _managerQuery = new NameValueCollection();

    public MockManagerContext(IProviderContainer container)
      : base(container)
    {
    }

    public bool IsManager
    {
      get 
      {
        bool result = false;
        if (HttpContext.Current.Items[MockManagerContextSettings.IsManager] != null)
        {
          result = (bool)HttpContext.Current.Items[MockManagerContextSettings.IsManager];
        }
        return result;
      }
    }

    public string ManagerUserId
    {
      get
      {
        string result = string.Empty;
        if (HttpContext.Current.Items[MockManagerContextSettings.UserId] != null)
        {
          result = (string)HttpContext.Current.Items[MockManagerContextSettings.UserId];
        }
        return result;
      }
    }

    public string ManagerUserName
    {
      get
      {
        string result = string.Empty;
        if (HttpContext.Current.Items[MockManagerContextSettings.UserName] != null)
        {
          result = (string)HttpContext.Current.Items[MockManagerContextSettings.UserName];
        }
        return result;
      }
    }

    public System.Collections.Specialized.NameValueCollection ManagerQuery
    {
      get { return _managerQuery; }
    }

    public string ManagerShopperId
    {
      get
      {
        string result = string.Empty;
        if (HttpContext.Current.Items[MockManagerContextSettings.ShopperId] != null)
        {
          result = (string)HttpContext.Current.Items[MockManagerContextSettings.ShopperId];
        }
        return result;
      }
    }

    public int ManagerPrivateLabelId
    {
      get
      {
        int result = 0;
        if (HttpContext.Current.Items[MockManagerContextSettings.PrivateLabelId] != null)
        {
          result = (int)HttpContext.Current.Items[MockManagerContextSettings.PrivateLabelId];
        }
        return result;
      }
    }

    public int ManagerContextId
    {
      get
      {
        return KnownPrivateLabelIds.GetContextId(ManagerPrivateLabelId);
      }
    }
  }
}
