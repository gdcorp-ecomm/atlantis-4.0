using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperDataCategoryUpdate.Interface
{
  public class ShopperDataCategoryUpdateRequestData : RequestData
  {
    private int _categoryId;
    private CategoryStatus _status;

    public enum CategoryStatus
    {
      Remove = 0,
      Add = 1
    }

    public ShopperDataCategoryUpdateRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, int categoryId, CategoryStatus status)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      _categoryId = categoryId;
      _status = status;
    }

    public int CategoryID
    {
      get
      {
        return _categoryId;
      }
    }

    public CategoryStatus Status
    {
      get
      {
        return _status;
      }
    } 

    public override string GetCacheMD5()
    {
      throw new Exception("ShopperDataCategoryUpdate is not a cacheable request.");
    }

  }
}
