using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Currency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.TH.Products
{
  internal static class ICurrencyProviderExtensions
  {
    public static ICurrencyPrice GetCurrentPrice(this ICurrencyProvider currency, IProviderContainer container, int unifiedProductId, ICurrencyInfo overrideCurrencyType)
    {
      ICurrencyPrice result = null;
      
      if (currency.SelectedTransactionalCurrencyInfo == overrideCurrencyType)
      {
        result = currency.GetCurrentPrice(unifiedProductId);
      }
      else 
      {
        int priceInOverriddenCurrency = 0;
      }

      return result;
      
    }
  }
}
