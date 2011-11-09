using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommInvoiceToBasket.Interface
{
  public class EcommInvoiceToBasketResponseData: IResponseData
  {
    private AtlantisException _atlException = null;
    public bool IsSuccess { get; set; }
    public int TransferCount { get; set; }

    public EcommInvoiceToBasketResponseData(int transferItemCount)
    {
      IsSuccess = transferItemCount > 0;
      TransferCount = transferItemCount;
    }
    public EcommInvoiceToBasketResponseData(RequestData request, Exception ex)
    { 
      IsSuccess = false;
      _atlException = new AtlantisException(request, "EcommInvoiceToBasketResponseData", ex.Message, string.Empty);
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
