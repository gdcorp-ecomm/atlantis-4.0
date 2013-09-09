using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Providers.Shopper.Tests
{
  public class MockManagerContextException : ProviderBase, IManagerContext
  {
    public MockManagerContextException(IProviderContainer container)
      : base(container)
    {
    }

    public bool IsManager
    {
      get { return true; }
    }

    public string ManagerUserId
    {
      get { throw new NotImplementedException(); }
    }

    public string ManagerUserName
    {
      get { throw new NotImplementedException(); }
    }

    public System.Collections.Specialized.NameValueCollection ManagerQuery
    {
      get { throw new NotImplementedException(); }
    }

    public string ManagerShopperId
    {
      get { throw new NotImplementedException(); }
    }

    public int ManagerPrivateLabelId
    {
      get { throw new NotImplementedException(); }
    }

    public int ManagerContextId
    {
      get { throw new NotImplementedException(); }
    }
  }
}
