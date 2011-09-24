using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCSetUpHostingAcct.Interface
{
  public class HCCSetUpHostingAcctRequestData : RequestData
  {
    static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);
    public HCCSetUpHostingAcctRequestData(string accountUid,
                                        string userName,
                                        string password,
                                        string domain,                                  
                                        int iisVersion,
                                        int dotNetVersion,
                                        int phpVersion,
                                        int pipeLineMode,
                                        string sslCertificateUid,
                                        bool enableGoogleWMT,
                                        bool enablePreviewDNS,
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
      IISVersion = iisVersion;
      DotNetVersion = dotNetVersion;
      PHPVersion = phpVersion;
      PipeLineMode = pipeLineMode;
      SSLCertificateUid = sslCertificateUid;
      EnableGoogleWMT = enableGoogleWMT;
      EnablePreviewDNS = enablePreviewDNS;
      RequestTimeout = _requestTimeout;
    }

    public string AccountUid { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Domain { get; set; }
    public int IISVersion { get; set; }
    public int DotNetVersion { get; set; }
    public int PHPVersion { get; set; }
    public int PipeLineMode { get; set; }
    public string SSLCertificateUid { get; set; }
    public bool EnableGoogleWMT { get; set; }
    public bool EnablePreviewDNS { get; set; }
    
    public override string GetCacheMD5()
    {
      throw new System.NotImplementedException();
    }
  }
}
