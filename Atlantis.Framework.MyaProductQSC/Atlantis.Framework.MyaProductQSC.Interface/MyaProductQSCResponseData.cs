using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaProduct.Interface;

namespace Atlantis.Framework.MyaProductQSC.Interface
{
  public class MyaProductQSCResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;

    public IList<QSCProduct> QSCProducts { get; private set; }

    public IPagingResult PagingResult { get; private set; }

    public bool IsSuccess
    {
      get
      {
        return _atlantisException == null;
      }
    }

    public MyaProductQSCResponseData(IList<QSCProduct> qscProducts, int totalRecord, int totalPages)
    {
      QSCProducts = qscProducts;
      PagingResult = new QSCPagingResult(totalRecord, totalPages);
    }

    public MyaProductQSCResponseData(MyaProductQSCRequestData requestData, Exception ex)
    {
      QSCProducts = new List<QSCProduct>(1);
      PagingResult = new QSCPagingResult(0, 0);
      _atlantisException = new AtlantisException(requestData,
                                                 MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                 string.Format("MyaProductQSCRequest Error: {0}", ex.Message),
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
