using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperDataCategoryGetByCatId.Interface
{
  public class ShopperDataCategoryGetByCatIdRequestData : RequestData
  {
    private int _categoryId;

    public ShopperDataCategoryGetByCatIdRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int categoryId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _categoryId = categoryId;
    }

    public int CategoryID
    {
      get
      {
        return _categoryId;
      }
    } 


    public override string GetCacheMD5()
    {
      throw new Exception("ShopperDataCategoryGetByCatId is not a cacheable request.");
    }
  }
}
