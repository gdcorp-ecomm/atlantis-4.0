using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;

namespace Atlantis.Framework.QSCGetOrders.Interface
{
  public class QSCGetOrdersRequestData : RequestData
  {
    public QSCGetOrdersRequestData(string shopperId, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount,
      string accountUid,
      int pageNumber,
      int pageSize) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      PageNumber = pageNumber;
      PageSize = pageSize;

      OrderSearchFields = new List<orderSearchField>();
    }

    public string AccountUid { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public List<orderSearchField> OrderSearchFields { get; set; }

    private string CacheKey
    {
      get { return "QSCGetAccounts:" + ShopperID + ":" + AccountUid; }
    }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      MD5 oMd5 = new MD5CryptoServiceProvider();
      oMd5.Initialize();
      byte[] stringBytes = Encoding.ASCII.GetBytes(CacheKey);
      byte[] md5Bytes = oMd5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", string.Empty);
    }

    #endregion
  }
}
