using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommOrderItemCommonName.Interface
{
  public class EcommOrderItemCommonNameRequestData : RequestData
  {
    #region Properties
    public int ProductTypeId { get; private set; }
    public int RowId { get; private set; }
    #endregion

    public EcommOrderItemCommonNameRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int productTypeId
      , int rowId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ProductTypeId = productTypeId;
      RowId = rowId;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in EcommOrderItemCommonNameRequestData");     
    }
  }
}
