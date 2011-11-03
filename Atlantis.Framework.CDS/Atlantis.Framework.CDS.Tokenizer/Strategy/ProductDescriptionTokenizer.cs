using System;
using System.Collections.Generic;
using Atlantis.Framework.CDS.Tokenizer.Interfaces;
using Atlantis.Framework.CDS.Tokenizer.Tokens;
using Atlantis.Framework.Providers.Interface.Products;
using Atlantis.Framework.Providers.Interface.ProviderContainer;

namespace Atlantis.Framework.CDS.Tokenizer.Strategy
{
  public class ProductDescriptionTokenizer : ITokenizerStrategy
  {
    public string Process(List<string> tokens)
    {
      IProductProvider products = HttpProviderContainer.Instance.Resolve<IProductProvider>();

      int productId = 0;
      Int32.TryParse(tokens[ProductToken.PRODUCT_ID], out productId);

      if (productId != 0)
        return products.GetProduct(productId).Info.Name;
      else
        return "";
    }
  }
}
