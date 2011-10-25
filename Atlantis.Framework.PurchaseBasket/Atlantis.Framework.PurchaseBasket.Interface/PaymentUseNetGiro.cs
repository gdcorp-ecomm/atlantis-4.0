
namespace Atlantis.Framework.PurchaseBasket.Interface
{
  public class PaymentUseNetGiro : PaymentElement
  {

    public override string ElementName
    {
      get { return "NetGiroPayment"; }
    }

    public string PaymentType
    {
      get { return GetStringProperty("type", "AliPay"); }
      set { this["type"] = value; }
    }

    public string Status
    {
      get { return GetStringProperty("status", "PENDING"); }
      set { this["status"] = value; }
    }

    public int Amount
    {
      get { return GetIntProperty("amount", 0); }
      set { this["amount"] = value.ToString(); }
    }

    public string OrderID
    {
      get { return GetStringProperty("order_id", string.Empty); }
      set { this["order_id"] = value; }
    }

    public int TransactionID
    {
      get { return GetIntProperty("transaction_id", 0); }
      set { this["transaction_id"] = value.ToString(); }
    }

    public string TimeStamp
    {
      get { return GetStringProperty("timestamp", string.Empty); }
      set { this["timestamp"] = value; }
    }

    public int NetGiroAmount
    {
      get { return GetIntProperty("netgiro_amount", 0); }
      set { this["netgiro_amount"] = value.ToString(); }
    }

    public int OrderSequence
    {
      get { return GetIntProperty("order_sequence", 0); }
      set { this["order_sequence"] = value.ToString(); }
    }

  }
}
