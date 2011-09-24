using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCGet404ErrorBehavior.Interface
{
  public class HCCGet404ErrorBehaviorRequestData : RequestData
  {
    static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public string AccountUid { get; set; }

    public HCCGet404ErrorBehaviorRequestData(string shopperId, 
                                          string sourceURL, 
                                          string orderId, 
                                          string pathway, 
                                          int pageCount,
                                          string accountUid) 
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      RequestTimeout = _requestTimeout;
    }

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();

      byte[] data = Encoding.UTF8.GetBytes(string.Format("{0}||{1}", base.ShopperID, AccountUid));

      byte[] hash = md5.ComputeHash(data);
      string result = Encoding.UTF8.GetString(hash);
      return result;
    }
  }
}
