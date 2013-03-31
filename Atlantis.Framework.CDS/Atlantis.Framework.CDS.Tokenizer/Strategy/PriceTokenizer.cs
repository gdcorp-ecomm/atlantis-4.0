using System;
using System.Collections.Generic;
using Atlantis.Framework.CDS.Tokenizer.Interfaces;
using Atlantis.Framework.CDS.Tokenizer.Tokens;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.CDS.Tokenizer.Strategy
{
  public class PriceTokenizer : ITokenizerStrategy
  {
    public string Process(List<string> tokens)
    {
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

      ICurrencyPrice price = currency.NewCurrencyPrice(Int32.Parse(tokens[PriceToken.AMOUNT]), currency.GetCurrencyInfo(tokens[PriceToken.CURRENCY_TYPE]), CurrencyPriceType.Transactional);
      PriceFormatOptions formatOptions = PriceFormatOptions.None;
      if (tokens[PriceToken.DROP_DECIMAL] == "dropdecimal")
      {
        formatOptions = PriceFormatOptions.DropDecimal;
      }
      return currency.PriceText(price, PriceTextOptions.None, formatOptions);
    }
  }
}
