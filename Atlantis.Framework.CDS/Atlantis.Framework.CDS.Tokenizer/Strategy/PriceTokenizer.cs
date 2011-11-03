using System;
using System.Collections.Generic;
using Atlantis.Framework.CDS.Tokenizer.Interfaces;
using Atlantis.Framework.CDS.Tokenizer.Tokens;
using Atlantis.Framework.Providers.Currency;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.ProviderContainer;


namespace Atlantis.Framework.CDS.Tokenizer.Strategy
{
  public class PriceTokenizer : ITokenizerStrategy
  {
    public string Process(List<string> tokens)
    {
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

      ICurrencyPrice price = new CurrencyPrice(Int32.Parse(tokens[PriceToken.AMOUNT]), CurrencyData.GetCurrencyInfo(tokens[PriceToken.CURRENCY_TYPE]), CurrencyPriceType.Transactional);
      return currency.PriceText(price, false, tokens[PriceToken.DROP_DECIMAL] == "dropdecimal");
    }
  }
}
