using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperDataCategoryUpdate.Interface
{
  public class ShopperDataCategoryUpdateResponseData : IResponseData
  {
    private AtlantisException _atlException = null;
    private bool _success = false;
    int _result = -1;

    public ShopperDataCategoryUpdateResponseData(int result)
    {
      _result = result;
      _success = (result == 0);
    }

    public ShopperDataCategoryUpdateResponseData(RequestData oRequestData, Exception ex)
    {
      _success = false;
      _atlException = new AtlantisException(oRequestData, "ShopperDataCategoryUpdateResponseData", ex.Message, string.Empty);
    }

    public bool IsSuccess
    {
      get { return _success; }
    }

    public int Result
    {
      get
      {
        return _result;
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
