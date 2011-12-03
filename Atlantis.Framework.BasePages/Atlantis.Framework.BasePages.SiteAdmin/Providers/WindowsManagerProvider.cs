using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BasePages.SiteAdmin.Providers
{
  public class NoManagerProvider : ProviderBase, IManagerContext
  {
    public NoManagerProvider(IProviderContainer container) : base(container)
    {
    }

    #region IManagerContext Members

    public bool IsManager
    {
      get { return false; }
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
      get { return string.Empty; }
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
  }
}
