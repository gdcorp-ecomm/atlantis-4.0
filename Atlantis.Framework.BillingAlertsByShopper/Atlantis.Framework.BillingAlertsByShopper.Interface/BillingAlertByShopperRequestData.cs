using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BillingAlertsByShopper.Interface
{
  public class BillingAlertsByShopperRequestData : RequestData
  {
    #region Properties
    private List<int> _productGroupIds;
    private int _totalAlertsThreshold = BillingAlertThreshold.All;

    public List<int> ProductGroupIds
    {
      get { return _productGroupIds; }
    }  

    public int TotalAlertsThreshold
    {
      get { return _totalAlertsThreshold; }
      set { _totalAlertsThreshold = value; }
    }

    #endregion

    public BillingAlertsByShopperRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  IEnumerable<int> productGroupIds)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _productGroupIds = new List<int>(productGroupIds);
      RequestTimeout = TimeSpan.FromSeconds(5);      
    }

    private string ProductGroupCombinedString
    {
      get
      {
        List<string> stringList = _productGroupIds.ConvertAll<string>(Convert.ToString);
        string result = string.Join("|", stringList.ToArray());
        return result;
      }
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}-{1}-{2}", ShopperID, TotalAlertsThreshold.ToString(), ProductGroupCombinedString));
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}
