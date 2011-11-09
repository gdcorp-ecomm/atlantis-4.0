using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommInvoiceDetails.Interface
{
  public class EcommInvoiceDetailsResponseData: IResponseData
  {
    private AtlantisException _atlException = null;

    public string InvoiceDetails = null;
    public bool IsSuccess { get; set; }

    public EcommInvoiceDetailsResponseData(string invoiceDetails)
    {
      InvoiceDetails = invoiceDetails;
      IsSuccess = !string.IsNullOrEmpty(invoiceDetails);
    }

    public EcommInvoiceDetailsResponseData(RequestData request, Exception ex)
    {
      IsSuccess = false;
      _atlException = new AtlantisException(request, "EcommInvoiceDetailsResponseData", ex.Message, string.Empty);
    }
    public string ToXML()
    {
      return InvoiceDetails;
    }

    public AtlantisException GetException()
    {
      return _atlException;
    }
  }
}
