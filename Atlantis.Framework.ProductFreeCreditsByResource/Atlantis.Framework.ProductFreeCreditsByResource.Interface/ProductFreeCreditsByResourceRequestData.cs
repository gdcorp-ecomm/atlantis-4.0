using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductFreeCreditsByResource.Interface
{
  public class ProductFreeCreditsByResourceRequestData : RequestData
  {
    #region Properties
    
    public int BillingResourceId { get; set; }
    public int ProductTypeId { get; set; }

    #endregion Properties

    public ProductFreeCreditsByResourceRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int billingResourceId
      , int productTypeId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(2d);
      BillingResourceId = billingResourceId;
      ProductTypeId = productTypeId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in ProductFreeCreditsByResourceRequestData");
    }

  }
}
