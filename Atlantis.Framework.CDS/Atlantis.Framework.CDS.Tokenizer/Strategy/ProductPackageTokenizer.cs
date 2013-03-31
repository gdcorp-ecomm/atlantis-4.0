using System;
using System.Collections.Generic;
using Atlantis.Framework.CDS.Tokenizer.Interfaces;
using Atlantis.Framework.CDS.Tokenizer.Tokens;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.CDS.Tokenizer.Strategy
{
  public class ProductPackageTokenizer : ITokenizerStrategy
  {
    public string Process(List<string> tokens)
    {
      IProductProvider products = HttpProviderContainer.Instance.Resolve<IProductProvider>();
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

      List<string> productIds = new List<string>(tokens[ProductPackageToken.PRODUCT_IDS].Split('|'));

      IProductView pv;
      ICurrencyPrice price;
      int totalPrice = 0;

      foreach (string pfid in productIds)
      {
        pv = products.NewProductView(products.GetProduct(Convert.ToInt32(pfid)));
        if (tokens[ProductPackageToken.OPERATOR] == "price" || tokens[ProductPackageToken.OPERATOR] == "price_current")
        {
          totalPrice = totalPrice + (tokens[ProductPackageToken.TERM_LABEL] == "monthly" ? pv.MonthlyCurrentPrice.Price : pv.YearlyCurrentPrice.Price);
        }
        else // list
        {
          totalPrice = totalPrice + (tokens[ProductPackageToken.TERM_LABEL] == "monthly" ? pv.MonthlyListPrice.Price : pv.YearlyListPrice.Price);
        }
      }
      price = currency.NewCurrencyPrice(totalPrice, currency.SelectedTransactionalCurrencyInfo, CurrencyPriceType.Transactional);
      PriceFormatOptions formatOptions = PriceFormatOptions.None;
      if (tokens[PriceToken.DROP_DECIMAL] == "dropdecimal")
      {
        formatOptions = PriceFormatOptions.DropDecimal;
      }
      return currency.PriceText(price, PriceTextOptions.None, formatOptions);
    }
  }
}
