using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorGetPhones.Interface
{
  public class AuthTwoFactorGetPhonesRequestData : RequestData
  {
    private static readonly TimeSpan _defaultRequestTimeout = TimeSpan.FromSeconds(6);

    public AuthTwoFactorGetPhonesRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = _defaultRequestTimeout;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("AuthTwoFactorGetPhones is not a cacheable request");
    }
  }
}
