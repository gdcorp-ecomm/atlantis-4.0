using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorStatus.Interface
{
  public class AuthTwoFactorStatusRequestData : RequestData
  {

    public AuthTwoFactorStatusRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(6);
    }


    #region RequestData  Members

    public override string GetCacheMD5()
    {
      throw new Exception("AuthTwoFactorStatus is not a cacheable request.");
    }

    public override string ToXML()
    {
      return string.Empty;
    }

    #endregion

  }
}
