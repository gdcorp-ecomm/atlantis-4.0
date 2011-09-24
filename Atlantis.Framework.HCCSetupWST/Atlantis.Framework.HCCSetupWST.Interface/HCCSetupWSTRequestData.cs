using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCSetupWST.Interface
{
  public class HCCSetupWSTRequestData : RequestData
  {
    static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);
    public HCCSetupWSTRequestData(string accountUid,
                                        string userName,
                                        string password,
                                        string domain,
                                        string sslCertificateUid,
                                        bool enableGoogleWMT,
                                        string shopperId,
								                        string sourceUrl,
								                        string orderId,
								                        string pathway,
								                        int pageCount) : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      UserName = userName;
      Password = password;
      Domain = domain;
      SslCertificateUid = sslCertificateUid;
      EnableGoogleWmt = enableGoogleWMT;
      RequestTimeout = _requestTimeout;
    }

    public string AccountUid { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Domain { get; set; }
    public string SslCertificateUid { get; set; }
    public bool EnableGoogleWmt { get; set; }
    
    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}
