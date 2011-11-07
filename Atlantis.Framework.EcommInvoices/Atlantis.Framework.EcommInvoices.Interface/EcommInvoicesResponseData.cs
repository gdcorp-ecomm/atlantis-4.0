using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using System.Xml.Serialization;
using System.IO;

namespace Atlantis.Framework.EcommInvoices.Interface
{
  public class EcommInvoicesResponseData : IResponseData
  {
    private AtlantisException _atlException = null;

    private List<Invoice> _invoices = null;
    public List<Invoice> Invoices
    {
      get { return _invoices; }
      set { _invoices = value; }
    }    

    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    
    public EcommInvoicesResponseData(List<Invoice> invoiceList, int totalPages = 0, int totalRecords = 0)
    {
      _invoices = invoiceList;
      TotalPages = totalPages;
      TotalRecords = totalRecords;
      IsSuccess = invoiceList != null;
    }

    public bool IsSuccess { get; set; }
    public EcommInvoicesResponseData(RequestData oRequestData, Exception ex)
    {
      IsSuccess = false;
      _atlException = new AtlantisException(oRequestData, "EcommInvoicesResponseData", ex.Message, string.Empty);
    }

    public string ToXML()
    {
      XmlSerializer serializer = new XmlSerializer(typeof(Invoice));
      StringWriter writer = new StringWriter();

      serializer.Serialize(writer, _invoices);

      return writer.ToString();
    }

    public AtlantisException GetException()
    {
      return _atlException;
    }
  }
}

