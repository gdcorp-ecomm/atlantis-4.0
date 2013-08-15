
namespace Atlantis.Framework.PurchaseBasket.Interface
{
  public abstract class PaymentElement : ElementBase
  {
    public PaymentElement Clone()
    {
      return this.MemberwiseClone() as PaymentElement;
    }
  }
}
