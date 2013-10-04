using System;
using System.Collections.Generic;
using System.IO;
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
    public List<int> NonUnifiedReceiptProductIds { get; set; }

    public ReceiptItem(string receiptId, DateTime receiptDate, string transactionCurrency, int transactionTotal, bool isRefunded, string orderSource, string detailsXml)
    {
      ReceiptId = receiptId;
      ReceiptDate = receiptDate;
      TransactionCurrency = transactionCurrency;
      TransactionTotal = transactionTotal;
      IsRefunded = isRefunded;
      OrderSource = orderSource;
      FillReceiptLists(detailsXml);
    }

    private void FillReceiptLists(string xml)
    {
      ReceiptDetails = new List<string>(5);
      NonUnifiedReceiptProductIds = new List<int>(5);

      if (xml.Length > 0)
      {
        using (XmlReader reader = new XmlTextReader(new StringReader(xml)))
        {
          while (reader.Read())
          {
            if (reader.IsStartElement() && string.Equals(reader.Name, "item"))
            {
              var detail = reader["detail"];
              if (!string.IsNullOrEmpty(detail))
              {
                ReceiptDetails.Add(detail);
              }

              var pf_id = reader["pf_id"];
              int productId;
              if (int.TryParse(pf_id, out productId))
              {
                NonUnifiedReceiptProductIds.Add(productId);
              }
            }
          }
        }
      }
    }
  }
}
