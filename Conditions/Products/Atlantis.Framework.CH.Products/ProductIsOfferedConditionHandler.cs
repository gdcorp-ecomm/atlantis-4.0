using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Products;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.CH.Products
{
  public class ProductIsOfferedConditionHandler : IConditionHandler
  {
    public string ConditionName
    {
      get { return "productIsOffered"; }
    }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      var result = false;

      if (parameters.Count > 0)
      {
        int productGroupId;
        var productGroupParam = parameters[0];

        if (ProductGroups.TryGetProductId(productGroupParam, out productGroupId) || int.TryParse(productGroupParam, out productGroupId))
        {
          var productProvider = providerContainer.Resolve<IProductProvider>();
          result = productProvider.IsProductGroupOffered(productGroupId);
        }
      }
      else
      {
        throw new ArgumentException(
          "ProductIsOfferedConditionHandler condition requires one parameter - product group id.");
      }

      return result;
    }
  }
}
