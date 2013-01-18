using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.PurchaseBasket.Interface
{
  public class PaymentUseThirdPartyCard : PaymentElement
  {
    public override string ElementName
    {
      get { return "ThirdPartyCardPayment"; }
    }

    public int Amount
    {
      get { return GetIntProperty("amount", 0); }
      set { this["amount"] = value.ToString(); }
    }

    public string Type
    {
      get { return GetStringProperty("type", string.Empty); }
      set { this["type"] = value; }
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

    public int ExpirationMonth
    {
      get { return GetIntProperty("expmonth", 0); }
      set { this["expmonth"] = value.ToString(); }
    }

    public int ExpirationYear
    {
      get { return GetIntProperty("expyear", 0); }
      set { this["expyear"] = value.ToString(); }
    }

    public string CVV
    {
      get { return GetStringProperty("cvv", string.Empty); }
      set { this["cvv"] = value; }
    }

  }
}
