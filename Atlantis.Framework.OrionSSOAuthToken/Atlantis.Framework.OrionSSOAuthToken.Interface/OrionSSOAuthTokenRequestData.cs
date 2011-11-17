using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OrionSSOAuthToken.Interface
{
  public class OrionSSOAuthTokenRequestData : RequestData
  {
    public int PrivateLabelId { get; private set; }
    public string OrionProductName { get; private set; }

    public OrionSSOAuthTokenRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int privateLabelId
      , string orionProductName)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      OrionProductName = orionProductName;
      RequestTimeout = TimeSpan.FromSeconds(10);
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes(string.Format("{0}-{1}-{2}", ShopperID, PrivateLabelId, OrionProductName));
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");    
    }
  }
}
