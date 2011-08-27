using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperDataCategoryGetByCatId.Interface
{
  public class ShopperDataCategoryGetByCatIdResponseData : IResponseData
  {

    private AtlantisException _atlException = null;
    private bool _success = false;
    int _data = 0;
    string _categoryName = string.Empty;

    public ShopperDataCategoryGetByCatIdResponseData(int data, string categoryName)
    {
      _data = data;
      _categoryName = categoryName;
      _success = true;
    }

    public ShopperDataCategoryGetByCatIdResponseData(RequestData oRequestData, Exception ex)
    {
      _success = false;
      _atlException = new AtlantisException(oRequestData, "ShopperDataCategoryGetByCatIdResponseData", ex.Message, string.Empty);
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public int Data
    {
      get
      {
        return _data;
      }
    }

    public string CategoryName
    {
      get
      {
        return _categoryName;
      }
    }


    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    #endregion

    public AtlantisException GetException()
    {
      return _atlException;
    }

  }
}
