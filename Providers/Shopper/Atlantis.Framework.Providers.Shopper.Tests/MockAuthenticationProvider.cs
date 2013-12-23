using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.SsoAuth.Interface;
using System;

namespace Atlantis.Framework.Providers.Shopper.Tests
{
  internal class MockAuthenticationProvider : ProviderBase, IAuthenticationProvider
  {
    public static bool DeauthenticateWasCalled { get; private set; }

    public MockAuthenticationProvider(IProviderContainer container) : base(container)
    {
    }

    public static void ClearHistory()
    {
      DeauthenticateWasCalled = false;
    }

    public IAuthenticationResponse Authenticate(string username, string password, string clientIp, string appName)
    {
      throw new NotImplementedException();
    }

    public void Deauthenticate()
    {
      DeauthenticateWasCalled = true;
    }

    public TimeSpan RequestTimeout
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public IAuthenticationResponse ValidateTwoFactorCode(string rawToken, string twoFactorCode, string appName)
    {
      throw new NotImplementedException();
    }
  }
}
