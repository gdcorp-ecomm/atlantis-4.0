using System;

namespace Atlantis.Framework.MYARecentOrders.Interface
{
  public class RecentOrder
  {
    private DateTime _dateEntered;
    private bool _isRefund;
    private string _orderId;
    private string _orderSource;
    private string _shopperId;
    private int _totalAmount;
    private string _transactionCurrency = "USD";

    public DateTime DateEntered
    {
      get { return _dateEntered; }
      set { _dateEntered = value; }
    }

    public bool IsRefund
    {
      get { return _isRefund; }
      set { _isRefund = value; }
    }

    public string OrderId
    {
      get { return _orderId; }
      set { _orderId = value; }
    }

    public string OrderSource
    {
      get { return _orderSource; }
      set { _orderSource = value; }
    }

    public string ShopperId
    {
      get { return _shopperId; }
      set { _shopperId = value; }
    }

    public int TransactionTotal
    {
      get { return _totalAmount; }
      set { _totalAmount = value; }
    }

    public string TransactionCurrency
    {
      get { return _transactionCurrency; }
      set { _transactionCurrency = value; }
    }
  }
}
