using System;
using System.Collections.Generic;
using System.Data;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaOrderHistory.Interface
{
  public class MyaOrderHistoryResponseData : IResponseData
  {
    private DataSet _ds = null;
    private AtlantisException _atlException = null;

    public MyaOrderHistoryResponseData(DataSet ds)
    {
      _ds = ds;
      _isSuccess = true;
    }

    public MyaOrderHistoryResponseData(AtlantisException exAtlantis)
    {
      _atlException = exAtlantis;
    }

    public MyaOrderHistoryResponseData(DataSet ds, RequestData oRequestData, Exception ex)
    {
      _ds = ds;
      _atlException = new AtlantisException(oRequestData, "MyaOrderHistoryResponseData", ex.Message, string.Empty);
    }

    private bool _isSuccess = false;
    public bool IsSuccess
    {
      get
      {
        return _isSuccess;
      }
    }

    public DataSet OrderHistoryListSet
    {
      get
      {
        return _ds;
      }
    }

    public int TotalRecords
    {
      get
      {
        int records = 0;
        if (_ds.Tables.Count > 0 && _ds.Tables[0].Rows != null && !int.TryParse(_ds.Tables[0].Rows[0]["Number_of_Records"].ToString(), out records))
        {
          records = 0;
        }
        return records;
      }
    }

    public int TotalPages
    {
      get
      {
        int pages = 0;
        if (_ds.Tables.Count > 0 && _ds.Tables[1].Rows != null && !int.TryParse(_ds.Tables[1].Rows[0]["Number_of_pages"].ToString(), out pages))
        {
          pages = 0;
        }
        return pages;
      }
    }

    public List<ReceiptItem> GetRecords
    {
      get
      {
        List<ReceiptItem> receiptList = new List<ReceiptItem>(5);

        if (_ds.Tables.Count > 0 && _ds.Tables[2].Rows != null)
        {
          foreach (DataRow row in _ds.Tables[2].Rows)
          {
            string  receiptId;
            DateTime receiptDate = DateTime.Now;
            string transactionCurrency = string.Empty;
            int transactionTotal = 0;
            bool isRefunded = false;
            string orderSource = string.Empty;
            string detailsXML = string.Empty;

            receiptId = row["order_id"].ToString();

            if (!DateTime.TryParse(row["date_entered"].ToString(), out receiptDate))
            {
              receiptDate = new DateTime(0, 0, 0);
            }
            
            if (!int.TryParse(row["transactionTotal"].ToString(), out transactionTotal))
            {
              transactionTotal = 0;
            }

            if (!row.IsNull("isRefunded"))
            {
              isRefunded = true;
            }

            if (!row.IsNull("order_source"))
            {
              orderSource = row["order_source"].ToString();
            }

            transactionCurrency = row["transactionCurrency"].ToString();
            detailsXML = row["detail"].ToString();

            ReceiptItem item = new ReceiptItem(receiptId, receiptDate, transactionCurrency, transactionTotal, isRefunded, orderSource, detailsXML);
            receiptList.Add(item);

          }
        }

        return receiptList;
      }
    }

    #region IResponseData Members

    public AtlantisException GetException()
    {
      return _atlException;
    }

    public string ToXML()
    {
      return string.Empty;
    }

    #endregion
  }
}
