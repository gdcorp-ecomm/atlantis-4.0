using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaRecentNamespaces.Interface
{
  public class MyaRecentNamespacesRequestData : RequestData
  {
    private static TimeSpan _DEFAULTIMEOUT = TimeSpan.FromSeconds(10.0);
    public DateTime FromDate { get; private set; }

    public MyaRecentNamespacesRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, DateTime fromDate)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = _DEFAULTIMEOUT;
      FromDate = fromDate;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}-{1}", ShopperID, FromDate));
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
