using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballProduct.Interface
{
  public class FastballProductRequestData : RequestData
  {
    public string Placement { get; set; }
    static TimeSpan _twoSeconds = TimeSpan.FromSeconds(2.0);

    public FastballProductRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, string placement)
      :base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Placement = placement;
      RequestTimeout = _twoSeconds;
    }

    /// <summary>
    /// This is a very unique caching rule.  To avoid a situation where someone is somehow getting new guids on every request
    /// we will only key the data by the placement.  So if there is some kind of visit guid generation issue, only 1
    /// placement will be available for the users session.
    /// </summary>
    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(Placement);
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
