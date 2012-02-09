using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDSubmitSyncPasswords.Interface
{
  public class HDVDSubmitSyncPasswordsRequestData : RequestData
  {
    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(15);
    
    public HDVDSubmitSyncPasswordsRequestData(string shopperId, 
                                              string sourceURL, 
                                              string orderId, 
                                              string pathway, 
                                              int pageCount,
                                              string accountGuid,
                                              string userName,
                                              string password) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountGuid = accountGuid;
      UserName = userName;
      UserPassword = password;
      RootPassword = password;
      FirewallPassword = password;
      RequestTimeout = _requestTimeout;
    }

    public HDVDSubmitSyncPasswordsRequestData(string shopperId,
                                              string sourceURL,
                                              string orderId,
                                              string pathway,
                                              int pageCount,
                                              string accountGuid,
                                              string userName,
                                              string userPassword,
                                              string rootPassword,
                                              string firewallPassword)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountGuid = accountGuid;
      UserName = userName;
      UserPassword = userPassword;
      RootPassword = rootPassword;
      FirewallPassword = firewallPassword;
      RequestTimeout = _requestTimeout;
    }

    public string AccountGuid { get; set; }

    public string UserName { get; set; }

    public string UserPassword { get; set; }

    public string  RootPassword { get; set; }

    public string FirewallPassword { get; set; }


    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("HDVDSubmitSyncPasswords is not a cacheable request.");
    }

    #endregion
  }
}
