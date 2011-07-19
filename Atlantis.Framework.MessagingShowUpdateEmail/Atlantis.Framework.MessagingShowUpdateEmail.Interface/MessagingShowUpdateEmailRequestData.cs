using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MessagingShowUpdateEmail.Interface
{
  public class MessagingShowUpdateEmailRequestData : RequestData
  {
    #region Properties

    public string ShopperEmail { get; set; }
    public int PrivateLabelId { get; set; }

    #endregion

    public MessagingShowUpdateEmailRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string shopperEmail,
                                  int privateLabelId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ShopperEmail = shopperEmail;
      PrivateLabelId = privateLabelId;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}-{1}-{2}", ShopperID, ShopperEmail, PrivateLabelId));
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
