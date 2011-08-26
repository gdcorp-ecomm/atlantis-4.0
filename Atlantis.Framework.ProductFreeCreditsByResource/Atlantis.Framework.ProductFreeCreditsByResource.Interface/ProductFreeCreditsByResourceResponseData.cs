using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ProductFreeCreditsByProductId.Interface.Types;

namespace Atlantis.Framework.ProductFreeCreditsByResource.Interface
{
  public class ProductFreeCreditsByResourceResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;
    public Dictionary<int, List<IProductFreeCredit>> ResourceFreeCredits { get; set; }

    public bool IsSuccess
    {
      get { return _atlantisException == null; }
    }

    public ProductFreeCreditsByResourceResponseData(Dictionary<int, List<IProductFreeCredit>> resourceFreeCredits)
    {
      ResourceFreeCredits = resourceFreeCredits;
    }

    public ProductFreeCreditsByResourceResponseData(AtlantisException atlantisException)
    {
      _atlantisException = atlantisException;
    }

    public ProductFreeCreditsByResourceResponseData(RequestData requestData, Exception ex)
    {
      _atlantisException = new AtlantisException(requestData,
                                                 MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                 string.Format("ProductFreeCreditsByResource Error: {0}", ex.Message),
                                                 ex.Data.ToString(),
                                                 ex);
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
