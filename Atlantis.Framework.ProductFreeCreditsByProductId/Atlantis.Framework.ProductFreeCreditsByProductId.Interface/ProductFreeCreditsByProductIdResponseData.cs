using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ProductFreeCreditsByProductId.Interface
{
  public class ProductFreeCreditsByProductIdResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;
    public List<ProductFreeCredit> ProductFreeCredits { get; set; }

    public bool IsSuccess
    {
      get { return _atlantisException == null; }
    }

    public ProductFreeCreditsByProductIdResponseData(List<ProductFreeCredit> productFreeCredits)
    {
      ProductFreeCredits = productFreeCredits;
    }

    public ProductFreeCreditsByProductIdResponseData(AtlantisException atlantisException)
    {
      _atlantisException = atlantisException;
    }

    public ProductFreeCreditsByProductIdResponseData(RequestData requestData, Exception ex)
    {
      _atlantisException = new AtlantisException(requestData
        , MethodBase.GetCurrentMethod().DeclaringType.FullName
        , string.Format("ProductFreeCreditsByProductId Error: {0}", ex.Message)
        , ex.Data.ToString()
        , ex);
    }

    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }

    #endregion

  }
}
