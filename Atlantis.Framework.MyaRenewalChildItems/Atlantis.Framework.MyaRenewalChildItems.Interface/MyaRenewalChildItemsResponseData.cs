using System;
using System.Collections.Generic;
using System.Data;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MYAGetExpiringProductsDetail.Interface;

namespace Atlantis.Framework.MyaRenewalChildItems.Interface
{
  public class MyaRenewalChildItemsResponseData : IResponseData
  {
    private AtlantisException _atlException;

    public MyaRenewalChildItemsResponseData(List<RenewalChildItem> productList)
    {      
      _isSuccess = true;     
      _childItemList = productList;
    }

    public MyaRenewalChildItemsResponseData(AtlantisException exAtlantis)
    {
      _atlException = exAtlantis;
    }

    public MyaRenewalChildItemsResponseData(DataSet ds, RequestData oRequestData, Exception ex)
    {      
      _atlException = new AtlantisException(oRequestData, "MyaRenewalChildItemsResponseData", ex.Message, string.Empty);
    }

    private bool _isSuccess;
    public bool IsSuccess
    {
      get
      {
        return _isSuccess;
      }
    }

    private List<RenewalChildItem> _childItemList;
    public List<RenewalChildItem> ChildItemList
    {
      get
      {
        return _childItemList;
      }
    }  

    public AtlantisException GetException()
    {
      return _atlException;
    }

    public string ToXML()
    {
      return string.Empty;
    }
  }
}
