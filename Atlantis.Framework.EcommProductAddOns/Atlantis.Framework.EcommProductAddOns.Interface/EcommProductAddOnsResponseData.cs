using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommProductAddOns.Interface
{
  public class EcommProductAddOnsResponseData : IResponseData
  {
    private readonly AtlantisException _exception = null;
    private readonly string _resultXml = string.Empty;
    public bool HasAddOns { get; private set; }
    public bool HasRenewableAddOns { get; private set; }
    public List<AddOnProduct> AddOnProducts { get; set; } 

    public bool IsSuccess
    {
      get { return _exception == null; }
    }

    public EcommProductAddOnsResponseData(List<AddOnProduct> addOnProducts, bool hasRenewableAddOns)
    {
      HasAddOns = addOnProducts.Count > 0;
      HasRenewableAddOns = hasRenewableAddOns;
      AddOnProducts = addOnProducts;
    }

     public EcommProductAddOnsResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public EcommProductAddOnsResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData
        , "EcommProductAddOnsResponseData"
        , exception.Message
        , requestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _resultXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
    #endregion
  }
}
