
namespace Atlantis.Framework.PurchaseBasket.Interface
{
  public class PaymentUsePaypal : PaymentElement
  {
    public override string ElementName
    {
      get { return "PaypalPayment"; }
    }

    public int Amount
    {
      get { return GetIntProperty("amount", 0); }
      set { this["amount"] = value.ToString(); }
    }

    public string AccountName
    {
      get { return GetStringProperty("account_name", string.Empty); }
      set { this["account_name"] = value; }
    }

    public string AccountNumber
    {
      get { return GetStringProperty("account_number", string.Empty); }
      set { this["account_number"] = value; }
    }

    public string ExpressToken
    {
      get { return GetStringProperty("express_token", string.Empty); }
      set { this["express_token"] = value; }
    }
  }
}
