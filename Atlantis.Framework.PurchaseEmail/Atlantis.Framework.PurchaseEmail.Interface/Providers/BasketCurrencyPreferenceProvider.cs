using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Providers.Currency.Interface;

namespace Atlantis.Framework.PurchaseEmail.Interface.Providers
{
  public class BasketCurrencyPreferenceProvider : ICurrencyPreferenceProvider
  {
    public string CurrencyPreference { get; set; }
  }
}
