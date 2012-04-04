using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MYARecentOrders.Interface
{
  public class MYARecentOrdersRequestData : RequestData
  {
    #region Properties

    public int OrderCount { get; set; }
    public int DaysToSearch { get; set; }

    #endregion

    public MYARecentOrdersRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  int orderCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(2);
      OrderCount = orderCount;
      DaysToSearch = 90;
    }

    public MYARecentOrdersRequestData(string shopperId,
                              string sourceUrl,
                              string orderId,
                              string pathway,
                              int pageCount,
                              int orderCount,
                              int daysToCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(2);
      OrderCount = orderCount;
      DaysToSearch = daysToCount;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}:{2}", ShopperID, OrderCount, DaysToSearch));
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
