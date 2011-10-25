using System;
using System.Collections.Generic;
using System.Reflection;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaProduct.Interface;

namespace Atlantis.Framework.MyaProductEmail.Interface
{
  public class MyaProductEmailResponseData : IResponseData
  {
    private readonly AtlantisException _atlantisException;

    public IList<EmailProduct> EmailProducts { get; private set; }

    public IPagingResult PagingResult { get; private set; }

    public bool IsSuccess
    {
      get
      {
        return _atlantisException == null;
      }
    }

    public MyaProductEmailResponseData(IList<EmailProduct> emailProducts, int totalRecord, int totalPages)
    {
      EmailProducts = emailProducts;
      PagingResult = new EmailPagingResult(totalRecord, totalPages);
    }

    public MyaProductEmailResponseData(MyaProductEmailRequestData requestData, Exception ex)
    {
      EmailProducts = new List<EmailProduct>(1);
      PagingResult = new EmailPagingResult(0, 0);
      _atlantisException = new AtlantisException(requestData, 
                                                 MethodBase.GetCurrentMethod().DeclaringType.FullName,
                                                 string.Format("MyaProductEmailRequest Error: {0}", ex.Message),
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
