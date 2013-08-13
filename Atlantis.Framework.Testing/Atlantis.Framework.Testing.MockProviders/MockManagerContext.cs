using Atlantis.Framework.Interface;
using System.Collections.Specialized;

namespace Atlantis.Framework.Testing.MockProviders
{
  public class MockManagerContext : ProviderBase, IManagerContext
  {
    readonly NameValueCollection _managerQuery = new NameValueCollection();

    public MockManagerContext(IProviderContainer container)
      : base(container)
    {
    }

    public bool IsManager
    {
      get
      {
        return Container.GetData(MockManagerContextSettings.IsManager, false);
      }
    }

    public string ManagerUserId
    {
      get
      {
        return Container.GetData(MockManagerContextSettings.UserId, string.Empty);
      }
    }

    public string ManagerUserName
    {
      get
      {
        return Container.GetData(MockManagerContextSettings.UserName, string.Empty);
      }
    }

    public NameValueCollection ManagerQuery
    {
      get { return _managerQuery; }
    }

    public string ManagerShopperId
    {
      get
      {
        return Container.GetData(MockManagerContextSettings.ShopperId, string.Empty);
      }
    }

    public int ManagerPrivateLabelId
    {
      get
      {
        return Container.GetData(MockManagerContextSettings.PrivateLabelId, 1);
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
