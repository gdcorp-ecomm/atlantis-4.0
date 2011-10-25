
namespace Atlantis.Framework.PurchaseBasket.Interface
{
  public class PaymentUseGiftCard : PaymentElement
  {
    public override string ElementName
    {
      get { return "GiftCardPayment"; }
    }

    public string AccountNumber
    {
      get { return GetStringProperty("account_number", string.Empty); }
      set { this["account_number"] = value; }
    }

    public int Amount
    {
      get { return GetIntProperty("amount", 0); }
      set { this["amount"] = value.ToString(); }
    }
  }
}
