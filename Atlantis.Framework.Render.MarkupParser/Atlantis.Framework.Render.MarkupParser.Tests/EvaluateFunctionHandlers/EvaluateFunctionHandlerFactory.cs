using System.Collections.Generic;
using Atlantis.Framework.Render.MarkupParser.Tests.EvaluateFunctionHandlers;

namespace Atlantis.Framework.ExpressionParser.Tests.EvaluateFunctionHandlers
{
  public static class EvaluateFunctionHandlerFactory
  {
    public static bool EvaluateFunctionHandler(string functionName, IEnumerable<string> parameters)
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
        case "aprimoMessageId":
          result = new AprimoMessageIdEvaluateFunctionHandler().EvaluateFunction(functionName, parameters);
          break;
      }

      return result;
    }
  }
}
