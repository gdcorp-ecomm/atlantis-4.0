using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Products;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Atlantis.Framework.CH.Products
{
  public class ProductHasSavingsMoreThanConditionHandler : IConditionHandler
  {
    public string ConditionName
    {
      get { return "ProductHasSavingsMoreThan"; }
    }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      var evaluationResult = false;

      if (parameters.Count == 3)
      {
        try
        {
          var productId = parameters[0];
          var baseProductId = parameters[1];
          var percent = parameters[2];

          int intProductId, intBaseProductId, intPercent;

          if (int.TryParse(productId, out intProductId) && int.TryParse(baseProductId, out intBaseProductId) && int.TryParse(percent, out intPercent))
          {
            var productProvider = providerContainer.Resolve<IProductProvider>();

            var product = productProvider.GetProduct(intProductId);
            var baseProduct = productProvider.GetProduct(intBaseProductId);

            if (product != null && baseProduct != null)
            {
              var pv = productProvider.NewProductView(product);
              pv.CalculateSavings(baseProduct);
              evaluationResult = pv.SavingsPercentage > intPercent;
            }
          }
        }
        catch (Exception ex)
        {
          LogError(GetType().Name, ex);
        }
      }

      return evaluationResult;
    }

    private static void LogError(string methodName, Exception ex)
    {
      try
      {
        if (ex.GetType() != typeof(ThreadAbortException))
        {
          var message = ex.Message + Environment.NewLine + ex.StackTrace;
          var source = methodName;
          var aex = new AtlantisException(source, "0", message, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(aex);
        }
      }
      catch (Exception) { }
    }
  }
}
