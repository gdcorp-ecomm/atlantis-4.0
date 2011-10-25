using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaProduct.Interface;

namespace Atlantis.Framework.MyaProductSharedHosting.Interface
{
  public class MyaProductSharedHostingResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;

    public IList<SharedHostingProduct> SharedHostingProducts { get; private set; }

    public IPagingResult PagingResult { get; private set; }

    public bool IsSuccess
    {
      get
      {
        return _atlantisException == null;
      }
    }

    public MyaProductSharedHostingResponseData(IList<SharedHostingProduct> sharedHostingProducts, int totalRecord, int totalPages)
    {
      SharedHostingProducts = sharedHostingProducts;
      PagingResult = new SharedHostingPagingResult(totalRecord, totalPages);
    }

    public MyaProductSharedHostingResponseData(MyaProductSharedHostingRequestData requestData, Exception ex)
    {
      SharedHostingProducts = new List<SharedHostingProduct>(1);
      PagingResult = new SharedHostingPagingResult(0, 0);
      _atlantisException = new AtlantisException(requestData, 
                                                 MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                 string.Format("MyaProductSharedHostingRequest Error: {0}", ex.Message),
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
