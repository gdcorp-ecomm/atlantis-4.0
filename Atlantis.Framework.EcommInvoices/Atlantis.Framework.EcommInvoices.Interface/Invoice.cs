using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.EcommInvoices.Interface
{
  [Serializable]
  public class Invoice
  {

    #region Properties

    private string _orderNumber { get; set; }
    private int _status { get; set; }

    public int ProcessorStatus { get; private set; }
    public DateTime OrderDate { get; private set; }
    public DateTime LastModifiedDate { get; private set; }
    public string PaymentType { get; private set; }
    public string Currency { get; private set; }
    public int Amount { get; private set; }

    public string Status
    {
      get
      {
        string statusText = string.Empty;


        if (OrderDate > ExpiresDate)
        {
          switch (_status)
          {
            case InvoiceStatus.Cancelled:
              statusText = "Cancelled";
              break;
            case InvoiceStatus.Completed:
              statusText = "Paid";
              break;
            case InvoiceStatus.Created:
              statusText = "Pending";
              break;
            case InvoiceStatus.Failed:
              statusText = "Failed";
              break;
            case InvoiceStatus.Active:
              statusText = "Active";
              break;
          }
        }
        else
        {
          statusText = "Expired";
        }

        return statusText;
      }
    }

    public string InvoiceNumber
    {
      get
      {
        if (_status != InvoiceStatus.Created)  //if status is created then the order number is now a receipt number.
        {
          return _orderNumber;
        }
        else
        {
          return "Paid";
        }

      }
    }

    public string ReceiptNumber
    {
      get
      {
        if (_status != InvoiceStatus.Created)  //if status is created then the order number is now a receipt number.
        {
          return "N/A";
        }
        else
        {
          return _orderNumber;
        }
      }
    }

    private DateTime _expiresDate;
    public DateTime ExpiresDate
    {
      get
      {
        if (_status != InvoiceStatus.Cancelled && _status != InvoiceStatus.Completed) //set the expires date to a really low date because they display "N/A" if in either of these statuses
        {
          return _expiresDate;
        }
        else
        {
          return Convert.ToDateTime("1/1/1980");
        }
      }
      private set
      {
        _expiresDate = value;
      }
    }

    private string _expiresText;
    public string ExpiresText
    {
      get
      {
        if (string.IsNullOrEmpty(_expiresText))
        {
          if (_status != InvoiceStatus.Cancelled && _status != InvoiceStatus.Completed)
          {
            _expiresText = OrderDate.AddDays(30).ToString("d");
          }
          else
          {
            _expiresText = "N/A";
          }
        }

        return _expiresText;
      }
    }

    #endregion

    public Invoice(string orderNumber, int status, int processorStatus, DateTime orderDate, DateTime lastModifiedDate, string paymentType, string currency, int amount)
    {
      _orderNumber = orderNumber;
      _status = status;
      ExpiresDate = orderDate.AddDays(30);

      ProcessorStatus = processorStatus;
      OrderDate = orderDate;
      LastModifiedDate = lastModifiedDate;
      PaymentType = paymentType;
      Currency = currency;
      Amount = amount;
    }
  }
}
