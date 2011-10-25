
namespace Atlantis.Framework.PurchaseBasket.Interface
{
  public class PaymentUseProfile : PaymentElement
  {
    public override string ElementName
    {
      get { return "Profile"; }
    }

    public string ShopperPaymentProfileId
    {
      get { return GetStringProperty("pp_shopperProfileID", string.Empty); }
      set { this["pp_shopperProfileID"] = value; }
    }

    public int Amount
    {
      get { return GetIntProperty("amount", 0); }
      set { this["amount"] = value.ToString(); }
    }
  }
}
