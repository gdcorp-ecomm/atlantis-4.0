using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCSet404ErrorBehavior.Interface
{
  public class HCCSet404ErrorBehaviorRequestData : RequestData
  {
    static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public string AccountUid { get; set; }
    public int ErrorPageType { get; set; }
    public string ErrorPagePath { get; set; }
    public string Filename { get; set; }

    public HCCSet404ErrorBehaviorRequestData(string shopperId, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount,
      string accountId,
      int errorPageType,
      string errorPagePath,
      string filename) 
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountId;
      ErrorPageType = errorPageType;
      ErrorPagePath = errorPagePath;
      Filename = filename;
      RequestTimeout = _requestTimeout;
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();

      byte[] data = Encoding.UTF8.GetBytes(string.Format("{0}||{1}||{2}||{3}||{4}", base.ShopperID, AccountUid, ErrorPageType, ErrorPagePath, Filename));

      byte[] hash = md5.ComputeHash(data);
      string result = Encoding.UTF8.GetString(hash);
      return result;
    }

    #endregion
  }
}
