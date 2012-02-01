using System;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HDVDGetAccountList.Interface
{
  public class HDVDGetAccountListRequestData : RequestData
  {
    private string _appId = string.Empty;

    public HDVDGetAccountListRequestData(
      string shopperId, string sourceURL, string orderId, string pathway, int pageCount,
      string productType, int pageSize, int pageNumber, string filter, string sortField, 
      string sortOrder, string appId
      ) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      ProductType = productType;
      PageSize = pageSize;
      PageNumber = pageNumber;
      Filter = filter;
      SortField = sortField;
      SortOrder = sortOrder;
      _appId = appId;
    }

    public string ProductType { get; set; }

    public int PageSize { get; set; }

    public int PageNumber { get; set; }

    public string Filter { get; set; }

    public string SortField { get; set; }

    public string SortOrder { get; set; }

    #region Overrides of RequestData
    
    private string CacheKey
    {
      get { return "HDVDGetAccountList:" +  _appId + ":" + ShopperID + ":" + ShopperID; }
    }

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
