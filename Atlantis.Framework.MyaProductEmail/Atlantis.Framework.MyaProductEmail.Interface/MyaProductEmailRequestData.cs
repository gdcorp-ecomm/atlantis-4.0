using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaProduct.Interface;
using Atlantis.Framework.MyaProduct.Interface.Enums;

namespace Atlantis.Framework.MyaProductEmail.Interface
{
  public class MyaProductEmailRequestData : RequestData
  {
    public enum SortColumnType
    {
      AccountExpirationDate,
      CommonName,
    }

    public int PrivateLabelId { get; set; }

    /// <summary>
    /// Default of Expiration Date
    /// </summary>
    public SortColumnType SortColumn { get; set; }
    
    /// <summary>
    /// Default of Ascending
    /// </summary>
    public SortDirectionType SortDirection { get; set; }

    public IPagingInfo PagingInfo { get; set; }
    
    public MyaProductEmailRequestData(string sShopperID,
                                      int privateLabelId,
                                      string sSourceURL, 
                                      string sOrderID, 
                                      string sPathway, 
                                      int iPageCount) : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      PrivateLabelId = privateLabelId;
      SortColumn = SortColumnType.AccountExpirationDate;
      SortDirection = SortDirectionType.Ascending;
      RequestTimeout = TimeSpan.FromSeconds(4);
      PagingInfo = new EmailPagingInfo();
    }

    public override string GetCacheMD5()
    {
      throw new Exception("MyaSharedHosting is not a cacheable request");
    }
  }
}
