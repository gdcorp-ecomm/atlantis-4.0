using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Products;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.CH.Products
{
  public class ProductHasDiscountMoreThanConditionHandler : IConditionHandler
  {
    public string ConditionName
    {
      get { return "ProductHasDiscountMoreThan"; }
    }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      var evaluationResult = false;

      if (parameters.Count == 2)
      {
        try
        {
          var productId = parameters[0];
          var percent = parameters[1];

          int intProductId, intPercent;

          if (int.TryParse(productId, out intProductId) && int.TryParse(percent, out intPercent))
          {
            var productProvider = providerContainer.Resolve<IProductProvider>();

            var product = productProvider.GetProduct(intProductId);

            if (product != null)
            {
              double currentPrice = product.CurrentPrice.Price;
              double listPrice = product.ListPrice.Price;

              var savings = (int) ((listPrice - currentPrice)/listPrice*100);
              evaluationResult = savings > intPercent;
            }
          }
        }
        catch (Exception ex)
        {
          var aex = new AtlantisException("ProductHasDiscountMoreThan EvaluateCondition", "0", ex.Message + Environment.NewLine + ex.StackTrace, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(aex);
        }
      }
      else
      {
        throw new ArgumentException("ProductHasDiscountMoreThan condition was called with the improper amount of parameters");
      }

      return evaluationResult;
    }
  }
}
