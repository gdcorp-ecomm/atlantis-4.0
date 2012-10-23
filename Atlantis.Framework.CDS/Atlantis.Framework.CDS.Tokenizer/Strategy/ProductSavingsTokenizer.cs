using System;
using System.Collections.Generic;
using Atlantis.Framework.CDS.Tokenizer.Interfaces;
using Atlantis.Framework.CDS.Tokenizer.Tokens;
using Atlantis.Framework.Providers.Interface.Currency;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.CDS.Tokenizer.Strategy
{
  public class ProductSavingsTokenizer : ITokenizerStrategy
  {
    public string Process(List<string> tokens)
    {
      IProductProvider products = HttpProviderContainer.Instance.Resolve<IProductProvider>();
      ICurrencyProvider currency = HttpProviderContainer.Instance.Resolve<ICurrencyProvider>();

      int productId = 0;
      Int32.TryParse(tokens[ProductToken.PRODUCT_ID], out productId);

      IProductView view = products.NewProductView(products.GetProduct(productId));

      IProduct product = products.GetProduct(productId);
      IProductView pView = products.NewProductView(product);
      pView.CalculateSavings(product);
      return pView.SavingsPercentage.ToString();

    }
  }
}
