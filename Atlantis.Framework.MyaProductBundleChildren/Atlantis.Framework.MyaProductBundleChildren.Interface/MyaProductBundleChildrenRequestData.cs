using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaProductBundleChildren.Interface
{
  public class MyaProductBundleChildrenRequestData : RequestData
  {
    #region Properties

    public int BillingResourceId { get; private set; }

    #endregion

    public MyaProductBundleChildrenRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int billingResourceId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5d);
      BillingResourceId = billingResourceId;
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes(string.Format("{0}-{1}", ShopperID, BillingResourceId));
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
