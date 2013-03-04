using System.Collections.Generic;

namespace Atlantis.Framework.Render.MarkupParser.Tests.EvaluateFunctionHandlers
{
  public class AprimoMessageIdEvaluateFunctionHandler
  {
    private const string ACTUAL_VALUE = "1234";

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
