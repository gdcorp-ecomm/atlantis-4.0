using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCAvailDomains.Interface
{
  public class HCCAvailDomainsRequestData : RequestData
  {
    static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);
    public HCCAvailDomainsRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string accountUid,
                                  int pageSize,
                                  int page)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      PageSize = pageSize;
      Page = page;
      RequestTimeout = _requestTimeout;
    }

    public string AccountUid { get; set; }
    public int PageSize { get; set; }
    public int Page { get; set; }

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
