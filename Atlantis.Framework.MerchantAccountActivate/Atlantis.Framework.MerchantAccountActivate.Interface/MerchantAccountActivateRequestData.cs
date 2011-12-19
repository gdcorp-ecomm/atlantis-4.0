using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MerchantAccountActivate.Interface
{
  public class MerchantAccountActivateRequestData : RequestData
  {
    #region Properties
    public int MerchantAccountId { get; private set; }
    #endregion

    public MerchantAccountActivateRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int merchantAccountId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      MerchantAccountId = merchantAccountId;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes(string.Format("{0}-{1}", MerchantAccountId, ShopperID));
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
