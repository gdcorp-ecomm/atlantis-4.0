using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthValidatePassword.Interface
{
  public class AuthValidatePasswordRequestData: RequestData
  {
    public string Password { get; set; }

    public AuthValidatePasswordRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string password)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      Password = password;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
