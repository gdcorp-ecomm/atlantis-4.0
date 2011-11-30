using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.EcommInvoices.Impl.BasketWs;
using Atlantis.Framework.EcommInvoices.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommInvoices.Impl
{
  class EcommInvoicesRequest : IRequest
  {
    private string _xmlInvoices { get; set; }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {

      //TODO: WS doesn't page/filter yet so because we're getting all records we're just going to return them all to the markup.
      //TODO: Once/if the ws pages/filters then uncomment filter and paging functionality.

      EcommInvoicesResponseData response = null;
      string xmlError = string.Empty;
      try
      {
        EcommInvoicesRequestData request = (EcommInvoicesRequestData)requestData;

        using (WscgdBasketService basketWs = new WscgdBasketService())
        {
          basketWs.Url = ((WsConfigElement)config).WSURL;
          basketWs.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

          _xmlInvoices = basketWs.GetInvoicesForShopperDisplay(request.ShopperID, request.RetrievalAttributes.DaysBack, out xmlError);

          if (string.IsNullOrEmpty(xmlError))
          {
            int totalRecords = -1;

            List<Invoice> invoices =  FetchInvoiceList(request.RetrievalAttributes.Filter, out totalRecords);
            int totalPages = 1;

            response = new EcommInvoicesResponseData(invoices, totalPages, totalRecords);
          }
          else
          {
            response = new EcommInvoicesResponseData(requestData, new Exception(xmlError));
          }
        }
      }
      catch (Exception ex)
      {
        response = new EcommInvoicesResponseData(requestData, ex);
      }

      return response;

    }

    private List<Invoice> FetchInvoiceList(int filter, out int totalRecords)
    {
      List<Invoice> invoices = new List<Invoice>();

      XDocument xml = XDocument.Parse(_xmlInvoices);
      if (xml.Root.HasElements)
      {
        IEnumerable<XElement> descendants = (IEnumerable<XElement>)xml.Descendants("INVOICE");
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

      //TODO: remove default sort when sorting comes from ws
      return invoices.OrderBy(i => i.Status).ToList(); 
    }

    private Invoice CreateInvoice(XElement element)
    {
      Invoice invoice = new Invoice(
        element.Attribute(InvoiceFieldNames.UID).Value, element.Attribute(InvoiceFieldNames.OrderId).Value, Convert.ToInt32(element.Attribute(InvoiceFieldNames.Status).Value)
        , Convert.ToInt32(element.Attribute(InvoiceFieldNames.ProcessorStatus).Value), Convert.ToDateTime(element.Attribute(InvoiceFieldNames.CreateDate).Value)
        , Convert. ToDateTime(element.Attribute(InvoiceFieldNames.LastModifiedDate).Value), element.Attribute(InvoiceFieldNames.ProcessorType).Value
        , element.Attribute(InvoiceFieldNames.Currency).Value, Convert.ToInt32(element.Attribute(InvoiceFieldNames.Amount).Value)
      );

      return invoice;
    }
  }
}
