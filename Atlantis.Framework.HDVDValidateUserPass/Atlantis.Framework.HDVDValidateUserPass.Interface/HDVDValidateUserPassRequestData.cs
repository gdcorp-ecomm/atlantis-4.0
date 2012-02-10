using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDValidateUserPass.Interface
{
  public class HDVDValidateUserPassRequestData : RequestData
  {
    private readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(15);
    
    public HDVDValidateUserPassRequestData(
      string shopperId, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount,
      string accountGuid,
      string hostName,
      string userName,
      string password,
      bool validateFtp,
      string ftpUserName,
      string ftpPassword,
      string firewallPassword
      ) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountGuid = accountGuid;
      HostName = hostName;
      UserName = userName;
      Password = password;
      ValidateFtp = validateFtp;
      FtpUserName = ftpUserName;
      FtpPassword = ftpPassword;
      FirewallPassword = firewallPassword;
      RequestTimeout = _requestTimeout;
    }

    public string AccountGuid { get; set; }

    public string HostName { get; set; }
    
    public string UserName { get; set; }
    
    public string Password { get; set; }
    
    public bool ValidateFtp { get; set; }
    
    public string FtpUserName { get; set; }
    
    public string FtpPassword { get; set; }
    
    public string FirewallPassword { get; set; }

    
    #region Overrides of RequestData
    
    public override string GetCacheMD5()
    {
      throw new Exception("HDVDValidateUserPassRequest is not a cacheable request.");
    }

    #endregion

    
  }
}
