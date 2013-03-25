using System.Collections.Generic;

namespace Atlantis.Framework.Render.ExpressionParser.Tests.EvaluateFunctionHandlers
{
  public class DataCenterEvaluateFunctionHandler
  {
    private const string ACTUAL_VALUE = "AP";

    public bool EvaluateFunction(string functionName, IEnumerable<string> parameters)
    {
      bool evaluationResult = false;

      foreach (string expectedValue in parameters)
      {
        if (expectedValue == ACTUAL_VALUE)
        {
          evaluationResult = true;
          break;
        }
      }

      return evaluationResult;
    }
  }
}
