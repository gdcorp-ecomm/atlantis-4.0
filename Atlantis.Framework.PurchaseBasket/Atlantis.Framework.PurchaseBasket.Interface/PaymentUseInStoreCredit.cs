
namespace Atlantis.Framework.PurchaseBasket.Interface
{
  public class PaymentUseInStoreCredit : PaymentElement
  {
    public override string ElementName
    {
      get { return "ISCPayment"; }
    }
    
    public int Amount
    {
      get { return GetIntProperty("amount", 0); }
      set { this["amount"] = value.ToString(); }
    }
  }
}
