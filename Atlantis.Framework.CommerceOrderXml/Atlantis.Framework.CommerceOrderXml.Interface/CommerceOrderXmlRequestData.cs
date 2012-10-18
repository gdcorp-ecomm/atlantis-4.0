using System;
using Atlantis.Framework.Interface;
using System.Security.Cryptography;

namespace Atlantis.Framework.CommerceOrderXml.Interface
{
  public class CommerceOrderXmlRequestData : RequestData
  {
    #region Properties
    private string _recentOrderId;

    public string RecentOrderId
    {
      get { return _recentOrderId; }
    }

    #endregion
    public CommerceOrderXmlRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string recentOrderId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _recentOrderId = recentOrderId;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();

      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(string.Concat("OrderID:",RecentOrderId, ":", ShopperID));

      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);

      string sValue = BitConverter.ToString(md5Bytes, 0);

      return sValue.Replace("-", "");
    }
  }
}
