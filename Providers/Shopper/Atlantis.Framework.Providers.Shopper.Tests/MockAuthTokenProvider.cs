using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.SsoAuth.Interface;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Shopper.Tests
{
  internal class MockAuthTokenProvider : ProviderBase, IAuthTokenProvider
  {
    private class MockAuthToken : IAuthToken
    {
      private class MockPayload : IAuthTokenPayload
      {
        public IDictionary<string, DateTime> AuthenticationFactors
        {
          get { throw new NotImplementedException(); }
        }

        public DateTime ExpiresAt
        {
          get { throw new NotImplementedException(); }
        }

        public string FirstName
        {
          get { throw new NotImplementedException(); }
        }

        public DateTime IssuedAt
        {
          get { throw new NotImplementedException(); }
        }

        public int PrivateLabelId
        {
          get { throw new NotImplementedException(); }
        }

        public string ShopperId
        {
          get { return "bogusshopperid"; }
        }

        public string TokenId
        {
          get { throw new NotImplementedException(); }
        }

        public string TokenType
        {
          get { throw new NotImplementedException(); }
        }
      }

      public IAuthTokenHeader Header
      {
        get { throw new NotImplementedException(); }
      }

      public bool IsEncrypted
      {
        get { throw new NotImplementedException(); }
      }

      public IAuthTokenPayload Payload
      {
        get { return new MockPayload(); }
      }

      public string RawValue
      {
        get { throw new NotImplementedException(); }
      }

      public string Signature
      {
        get { throw new NotImplementedException(); }
      }

      public bool Validate()
      {
        return IsMockTokenvalid;
      }


      public TimeSpan RequestTimeout
      {
        get
        {
          return TimeSpan.FromSeconds(5);
        }
        set
        {
          throw new NotImplementedException();
        }
      }
    }

    public MockAuthTokenProvider(IProviderContainer container) : base(container) { }

    public static bool IsMockTokenvalid { get; set; }

    public IAuthToken AuthToken
    {
      get { return new MockAuthToken(); }
    }
  }
}
