using System;
using Atlantis.Framework.Interface;
using System.Collections.Specialized;

namespace Atlantis.Framework.Testing.MockProviders
{
  public class MockManagerContext : MockProviderBase, IManagerContext
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

        object isManager = GetMockSetting(MockManagerContextSettings.IsManager);
        if (isManager != null)
        {
          result = Convert.ToBoolean(isManager);
        }

        return result;
      }
    }

    public string ManagerUserId
    {
      get
      {
        string result = string.Empty;

        string userId = GetMockSetting(MockManagerContextSettings.UserId) as string;
        if (userId != null)
        {
          result = userId;
        }

        return result;
      }
    }

    public string ManagerUserName
    {
      get
      {
        string result = string.Empty;

        string userName = GetMockSetting(MockManagerContextSettings.UserName) as string;
        if (userName != null)
        {
          result = userName;
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

        string shopperId = GetMockSetting(MockManagerContextSettings.ShopperId) as string;
        if (shopperId != null)
        {
          result = shopperId;
        }

        return result;
      }
    }

    public int ManagerPrivateLabelId
    {
      get
      {
        int result = 0;

        object privateLabelId = GetMockSetting(MockManagerContextSettings.PrivateLabelId);
        if (privateLabelId != null)
        {
          result = Convert.ToInt32(privateLabelId);
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
