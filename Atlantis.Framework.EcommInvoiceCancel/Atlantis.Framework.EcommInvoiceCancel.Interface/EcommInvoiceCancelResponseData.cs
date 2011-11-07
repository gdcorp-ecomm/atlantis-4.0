using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommInvoiceCancel.Interface
{
 public class EcommInvoiceCancelResponseData: IResponseData
  {
    AtlantisException _atlException = null;
    public bool IsSuccess { get; set; }

    public EcommInvoiceCancelResponseData(short success) 
    {
      IsSuccess = success != 0;
    }

    public EcommInvoiceCancelResponseData(RequestData request, Exception ex)
    {
      IsSuccess = false;
      _atlException = new AtlantisException(request, "EcommInvoiceCancelResponseData", ex.Message, string.Empty);
    }

    public string ToXML()
    {
      throw new NotImplementedException();
    }

    public AtlantisException GetException()
    {
      return _atlException;
    }
  }
}
