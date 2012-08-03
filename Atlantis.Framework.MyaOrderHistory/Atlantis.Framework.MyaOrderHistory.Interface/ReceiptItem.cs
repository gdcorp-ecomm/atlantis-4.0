using System;
using System.Collections.Generic;
using System.Xml;

namespace Atlantis.Framework.MyaOrderHistory.Interface
{
  public class ReceiptItem
  {
    public string ReceiptId { get; set; }
    public DateTime ReceiptDate { get; set; }
    public string TransactionCurrency { get; set; }
    public int TransactionTotal { get; set; }
    public bool IsRefunded { get; set; }
    public string OrderSource { get; set; }
    public List<string> ReceiptDetails { get; set; }

    public ReceiptItem(string receiptId, DateTime receiptDate, string transactionCurrency, int transactionTotal, bool isRefunded, string orderSource, string detailsXml)
    {
      ReceiptId = receiptId;
      ReceiptDate = receiptDate;
      TransactionCurrency = transactionCurrency;
      TransactionTotal = transactionTotal;
      IsRefunded = isRefunded;
      OrderSource = orderSource;
      ReceiptDetails = ReceiptDescriptionList(detailsXml);
    }

    private List<string> ReceiptDescriptionList(string xml)
    {
      List<string> _items = new List<string>(5);
      if (xml.Length > 0)
      {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        XmlNodeList nodeList = xmlDoc.SelectNodes("items/item");
        if (nodeList != null && nodeList.Count > 0)
        {
          foreach (XmlNode node in nodeList)
          {
            string description = node.Attributes["detail"].Value;
            if (description.Length > 0)
            {
              _items.Add(description);
            }
          }
        }
      }

      return _items;
    }

  }
}
