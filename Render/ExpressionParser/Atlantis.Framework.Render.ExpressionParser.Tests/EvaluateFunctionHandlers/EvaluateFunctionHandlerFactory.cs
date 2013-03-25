using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Render.ExpressionParser.Tests.EvaluateFunctionHandlers
{
  public static class EvaluateFunctionHandlerFactory
  {
    public static bool EvaluateFunctionHandler(string functionName, IEnumerable<string> parameters, IProviderContainer providerContainer)
    {
      bool result = false;

      switch (functionName)
      {
        case "dataCenter":
          result = new DataCenterEvaluateFunctionHandler().EvaluateFunction(functionName, parameters);
          break;
        case "countrySiteContext":
          result = new CountrySiteContextEvaluateFunctionHandler().EvaluateFunction(functionName, parameters);
          break;
      }

      return result;
    }
  }
}
