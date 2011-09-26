using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuctionGetMemberInfo.Interface
{
  public class AuctionGetMemberInfoRequestData : RequestData
  {
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    public AuctionGetMemberInfoRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RequestTimeout = _requestTimeout;
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();

      byte[] data = Encoding.UTF8.GetBytes("AuctionGetMemberInfoRequestData::" + ShopperID);

      byte[] hash = md5.ComputeHash(data);
      string result = Encoding.UTF8.GetString(hash);
      return result;
    }

    #endregion
  }
}
