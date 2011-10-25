
namespace Atlantis.Framework.PurchaseBasket.Interface
{
  public class PaymentUseACHBusiness : PaymentElement
  {
    public override string ElementName
    {
      get { return "ACHBusinessPayment"; }
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

    public string RoutingNumber
    {
      get { return GetStringProperty("routing_number", string.Empty); }
      set { this["routing_number"] = value; }
    }

    public string BankName
    {
      get { return GetStringProperty("bank_name", string.Empty); }
      set { this["bank_name"] = value; }
    }

    public string BankCity
    {
      get { return GetStringProperty("bank_city", string.Empty); }
      set { this["bank_city"] = value; }
    }

    public string BankState
    {
      get { return GetStringProperty("bank_state", string.Empty); }
      set { this["bank_state"] = value; }
    }

    public string BankZip
    {
      get { return GetStringProperty("bank_zip", string.Empty); }
      set { this["bank_zip"] = value; }
    }

  }
}
