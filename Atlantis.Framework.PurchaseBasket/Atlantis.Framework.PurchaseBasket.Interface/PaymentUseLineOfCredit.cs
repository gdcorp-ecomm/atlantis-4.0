
namespace Atlantis.Framework.PurchaseBasket.Interface
{
  public class PaymentUseLineOfCredit:PaymentElement
  {

    public override string ElementName
    {
      get { return "CreditLinePayment"; }
    }

    public string AccountNumber
    {
      get { return GetStringProperty("account_number", string.Empty); }
      set { this["account_number"] = value; }
    }

  }
}
