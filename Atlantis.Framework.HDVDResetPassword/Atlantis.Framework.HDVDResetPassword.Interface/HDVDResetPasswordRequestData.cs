using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDResetPassword.Interface
{
  public class HDVDResetPasswordRequestData : RequestData
  {
    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(15);

    public HDVDResetPasswordRequestData(
      string shopperId, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount,
      string accountGuid,
      string newPassword) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountGuid = accountGuid;
      NewPassword = newPassword;
      RequestTimeout = _requestTimeout;
    }

    public string AccountGuid { get; set; }

    public string NewPassword { get; set; }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("HDVDSubmitResetPassword is not a cacheable request.");
    }

    #endregion
  }
}
