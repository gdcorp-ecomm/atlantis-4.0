using System;
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
      ICurrencyPrice price = view.MonthlyCurrentPrice;

      if (tokens[ProductToken.TERM_LABEL] == "yearly")
        price = view.YearlyCurrentPrice;

      return currency.PriceText(price, false, tokens[ProductToken.DROP_DECIMAL] == "dropdecimal");
    }
  }
}
