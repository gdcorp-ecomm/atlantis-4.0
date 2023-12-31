﻿using System;
using System.Collections.Generic;
using Atlantis.Framework.CDS.Tokenizer.Interfaces;
using Atlantis.Framework.CDS.Tokenizer.Tokens;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.CDS.Tokenizer.Strategy
{
  public class ProductPriceTokenizer : ITokenizerStrategy
  {
    public string Process(List<string> tokens)
    {
      IProductProvider products = HttpProviderContainer.Instance.Resolve<IProductProvider>();
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

      int productId = 0;
      Int32.TryParse(tokens[ProductToken.PRODUCT_ID], out productId);

      IProductView view = products.NewProductView(products.GetProduct(productId));
      ICurrencyPrice price;

      // CURRENT PRICE
      if (tokens[ProductToken.OPERATOR] == "price" || tokens[ProductToken.OPERATOR] == "price_current")
      {
        if (tokens[ProductToken.TERM_LABEL] == "yearly")
        {
          price = view.YearlyCurrentPrice;
        }
        else
        {
        price = view.MonthlyCurrentPrice;
        }
      }
      else // LIST PRICE
      {
        if (tokens[ProductToken.TERM_LABEL] == "yearly")
        {
          price = view.YearlyListPrice;
        }
        else
        {
        price = view.MonthlyListPrice;
        }
      }

      PriceFormatOptions formatOptions = PriceFormatOptions.None;
      if (tokens[PriceToken.DROP_DECIMAL] == "dropdecimal")
      {
        formatOptions = PriceFormatOptions.DropDecimal;
      }
      return currency.PriceText(price, PriceTextOptions.None, formatOptions);
    }
  }
}
