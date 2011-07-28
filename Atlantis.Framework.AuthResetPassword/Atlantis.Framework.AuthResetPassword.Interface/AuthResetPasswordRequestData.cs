using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthResetPassword.Interface
{
  public class AuthResetPasswordRequestData : RequestData
  {
    public AuthResetPasswordRequestData( string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int privateLabelId, string ipAddress, string newPassword, string newHint, string authToken )
      : base( shopperId, sourceUrl, orderId, pathway, pageCount )
    {
      PrivateLabelId = privateLabelId;
      IpAddress = ipAddress;
      NewPassword = newPassword;
      NewHint = newHint;
      AuthToken = authToken;
    }

    public int PrivateLabelId { get; private set; }

    public string IpAddress { get; private set; }

    public string NewPassword { get; private set; }

    public string NewHint { get; private set; }

    public string AuthToken { get; private set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException( "Auth Reset Password is not a cacheable request." );
    }
  }
}
