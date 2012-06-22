using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthResetPassword.Interface
{
  public class AuthResetPasswordRequestData : RequestData
  {
    public int PrivateLabelId { get; private set; }

    public string IpAddress { get; private set; }

    public string HostName { get; private set; }

    public string NewPassword { get; private set; }

    public string NewHint { get; private set; }

    public string EmailAuthToken { get; private set; }

    public string TwoFactorAuthToken { get; private set; }

    public AuthResetPasswordRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int privateLabelId, string ipAddress, string hostName, string newPassword, string newHint, string emailAuthToken)
                                        : this(shopperId, sourceUrl, orderId, pathway, pageCount, privateLabelId, ipAddress, hostName, newPassword, newHint, emailAuthToken, string.Empty)
    {
    }

    public AuthResetPasswordRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int privateLabelId, string ipAddress, string hostName, string newPassword, string newHint, string emailAuthToken, string twoFactorAuthToken)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      IpAddress = ipAddress;
      HostName = hostName;
      NewPassword = newPassword;
      NewHint = newHint;
      EmailAuthToken = emailAuthToken;
      TwoFactorAuthToken = twoFactorAuthToken;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("Auth Reset Password is not a cacheable request.");
    }
  }
}
