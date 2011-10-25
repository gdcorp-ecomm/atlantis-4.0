using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaProduct.Interface;

namespace Atlantis.Framework.MyaProductWST.Interface
{
  public class MyaProductWSTResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;

    public IList<WSTProduct> WSTProducts { get; private set; }

    public IPagingResult PagingResult { get; private set; }

    public bool IsSuccess
    {
      get
      {
        return _atlantisException == null;
      }
    }

    public MyaProductWSTResponseData(IList<WSTProduct> wstProducts, int totalRecord, int totalPages)
    {
      WSTProducts = wstProducts;
      PagingResult = new WSTPagingResult(totalRecord, totalPages);
    }

    public MyaProductWSTResponseData(MyaProductWSTRequestData requestData, Exception ex)
    {
      WSTProducts = new List<WSTProduct>(1);
      PagingResult = new WSTPagingResult(0, 0);
      _atlantisException = new AtlantisException(requestData,
                                                 MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                 string.Format("MyaProductWSTRequest Error: {0}", ex.Message),
                                                 ex.Data.ToString(),
                                                 ex);
    }

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _atlantisException;
    }
  }
}
