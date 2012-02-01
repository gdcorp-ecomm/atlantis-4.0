using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;


namespace Atlantis.Framework.HDVDGetAccountSummary.Interface
{
  public class HDVDGetAccountSummaryRequestData : RequestData
  {
    private readonly string _appId = string.Empty;

    public HDVDGetAccountSummaryRequestData(string shopperId, string sourceURL, string orderId, string pathway, 
                                            int pageCount, string appId, Guid accountGuid) 
                                            : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _appId = appId;
      AccountGuid = accountGuid;
    }

    public TimeSpan RequestTimeout { get; set; }
    
    private string CacheKey
    {
      get { return "HDVDGetAccountSummary:" + _appId + ":" + ShopperID + ":" + AccountGuid; }
    }

    public Guid AccountGuid { get; set; }

    #region Overrides of RequestData
   

    public override string GetCacheMD5()
    {
      MD5 oMd5 = new MD5CryptoServiceProvider();
      oMd5.Initialize();
      byte[] stringBytes = Encoding.ASCII.GetBytes(CacheKey);
      byte[] md5Bytes = oMd5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }
    #endregion
  }
}
