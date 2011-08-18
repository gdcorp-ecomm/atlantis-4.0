using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;
using System.Reflection;

namespace Atlantis.Framework.ProductFreeCreditsByResource.Interface
{
  public partial class ProductFreeCreditsByResourceResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;
    public List<ProductFreeCredit> ProductFreeCredits { get; set; }

    public bool IsSuccess
    {
      get { return _atlantisException == null; }
    }

    public ProductFreeCreditsByResourceResponseData(List<ProductFreeCredit> productFreeCredits)
    {
      ProductFreeCredits = productFreeCredits;
    }

    public ProductFreeCreditsByResourceResponseData(AtlantisException atlantisException)
    {
      _atlantisException = atlantisException;
    }

    public ProductFreeCreditsByResourceResponseData(RequestData requestData, Exception ex)
    {
      _atlantisException = new AtlantisException(requestData
        , MethodBase.GetCurrentMethod().DeclaringType.FullName
        , string.Format("ProductFreeCreditsByResource Error: {0}", ex.Message)
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
