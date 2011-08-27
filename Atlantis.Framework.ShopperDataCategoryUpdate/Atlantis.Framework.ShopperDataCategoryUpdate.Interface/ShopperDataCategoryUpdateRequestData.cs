using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperDataCategoryUpdate.Interface
{
  public class ShopperDataCategoryUpdateRequestData : RequestData
  {
    private int _categoryId;
    private int _shopperData;

    public ShopperDataCategoryUpdateRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int categoryId, int shopperdata)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _categoryId = categoryId;
      _shopperData = shopperdata;
    }

    public int CategoryID
    {
      get
      {
        return _categoryId;
      }
    }

    public int ShopperData
    {
      get
      {
        return _shopperData;
      }
    } 

    public override string GetCacheMD5()
    {
      throw new Exception("ShopperDataCategoryUpdate is not a cacheable request.");
    }

  }
}
