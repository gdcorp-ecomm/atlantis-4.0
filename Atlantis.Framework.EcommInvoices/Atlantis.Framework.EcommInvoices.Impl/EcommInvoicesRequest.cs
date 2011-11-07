using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.EcommInvoices.Impl.BasketWs;
using Atlantis.Framework.EcommInvoices.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.EcommInvoices.Impl
{
  class EcommInvoicesRequest : IRequest
  {
    private string _xmlInvoices { get; set; }
    private EcommInvoicesResponseData _response { get; set; }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      _response = null;
      string xmlError = string.Empty;
      try
      {
        EcommInvoicesRequestData request = (EcommInvoicesRequestData)requestData;

        using (WscgdBasketService basketWs = new WscgdBasketService())
        {
          basketWs.Url = ((WsConfigElement)config).WSURL;
          basketWs.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

          _xmlInvoices = basketWs.GetInvoicesForShopper(request.ShopperID, out xmlError);

          if (string.IsNullOrEmpty(xmlError))
          {
            int totalRecords = -1;

            List<Invoice> invoices =  FetchInvoiceList(request.RetrievalAttributes.Filter, out totalRecords);
            ParseInvoices(request.RetrievalAttributes, ref invoices);

            int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalRecords) / Convert.ToDecimal(request.RetrievalAttributes.PageSize)));

            _response = new EcommInvoicesResponseData(invoices, totalPages, totalRecords);
          }
          else
          {
            _response = new EcommInvoicesResponseData(requestData, new Exception(xmlError));
          }
        }
      }
      catch (Exception ex)
      {
        _response = new EcommInvoicesResponseData(requestData, ex);
      }

      return _response;

    }

    private List<Invoice> FetchInvoiceList(int filter, out int totalRecords)
    {
      List<Invoice> invoices = new List<Invoice>();

      XDocument xml = XDocument.Parse(_xmlInvoices);
      if (xml.Root.HasElements)
      {
        IEnumerable<XElement> descendants = (IEnumerable<XElement>)xml.Descendants("INVOICE");

        if (filter != InvoiceStatus.All)
        {
          descendants = descendants.Where(f => Convert.ToInt32(f.Attribute(InvoiceFieldNames.Status).Value) == filter);
        }

        totalRecords = descendants.Count();

        foreach (XElement element in descendants)
        {
          invoices.Add(CreateInvoice(element));
        }
      }
      else
      {
        totalRecords = 0;
      }

      return invoices;
    }

    private Invoice CreateInvoice(XElement element)
    {
      Invoice invoice = new Invoice(
        element.Attribute(InvoiceFieldNames.OrderId).Value, Convert.ToInt32(element.Attribute(InvoiceFieldNames.Status).Value)
        , Convert.ToInt32(element.Attribute(InvoiceFieldNames.ProcessorStatus).Value), Convert.ToDateTime(element.Attribute(InvoiceFieldNames.CreateDate).Value)
        , Convert.ToDateTime(element.Attribute(InvoiceFieldNames.LastModifiedDate).Value), element.Attribute(InvoiceFieldNames.ProcessorType).Value
        , element.Attribute(InvoiceFieldNames.Currency).Value, Convert.ToInt32(element.Attribute(InvoiceFieldNames.Amount).Value)
      );

      return invoice;
    }

    private void ParseInvoices(RetrievalAttributes retAttr, ref List<Invoice> invoices)
    {
     int skip = (retAttr.CurrentPage - 1) * retAttr.PageSize;

      Func<Invoice, object> sortDelegate;

      switch (retAttr.SortColumn)
      {
        case "status":
          sortDelegate = i => i.Status;
          break;
        case "payment":
          sortDelegate = i => i.PaymentType;
          break;
        case "expireDate":
          sortDelegate = i => i.ExpiresDate;
          break;
        case "amount":
          sortDelegate = i => i.Amount;
          break;
        default:  // "orderDate"
          sortDelegate = i => i.OrderDate;
          break;
      }

      if (retAttr.SortDirection == "asc")
      {
       invoices = invoices.OrderBy(sortDelegate).Skip(skip).Take(retAttr.PageSize).ToList();
      }
      else
      {
       invoices = invoices.OrderByDescending(sortDelegate).Skip(skip).Take(retAttr.PageSize).ToList();
      }
    }
  }
}
