
namespace Atlantis.Framework.EcommValidPaymentType.Interface
{
  public class PaymentTypeInfo
  {
    public string PaymentType { get; private set; }
  
    public string Name { get; private set; }

    public PaymentTypeInfo(string paymentType, string name)
    {
      PaymentType = paymentType;
      Name = name;
    }
  }
}
