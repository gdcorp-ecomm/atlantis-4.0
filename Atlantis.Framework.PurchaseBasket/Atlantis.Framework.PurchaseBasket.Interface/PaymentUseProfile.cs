
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

    public string Cvv
    {
      get { return GetStringProperty("cvv", string.Empty); }
      set { this["cvv"] = value; }
    }

    public string TaxId
    {
      get { return GetStringProperty("tax_id", string.Empty); }
      set { this["tax_id"] = value; }
    }
  }
}
