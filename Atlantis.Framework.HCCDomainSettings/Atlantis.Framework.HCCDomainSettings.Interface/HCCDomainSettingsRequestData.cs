using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCDomainSettings.Interface
{
  public class HCCDomainSettingsRequestData : RequestData
  {
    static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public HCCDomainSettingsRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string accountUid,
                                  string domain)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      Domain = domain;
      RequestTimeout = _requestTimeout;
    }

    public string AccountUid { get; set; }
    public string Domain { get; set; }

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();

      byte[] data = Encoding.UTF8.GetBytes(string.Format("{0}||{1}||{2}", base.ShopperID, AccountUid, Domain));

      byte[] hash = md5.ComputeHash(data);
      string result = Encoding.UTF8.GetString(hash);
      return result;
    }


  }
}
