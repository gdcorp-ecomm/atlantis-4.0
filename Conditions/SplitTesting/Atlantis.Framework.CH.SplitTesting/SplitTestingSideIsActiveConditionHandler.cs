using System;
using System.Collections.Generic;
using System.Threading;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.SplitTesting.Interface;

namespace Atlantis.Framework.CH.SplitTesting
{
  public class SplitTestingSideIsActiveConditionHandler : IConditionHandler
  {
    public string ConditionName
    {
      get { return "SplitTestingSideIsActive"; }
    }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      var evaluationResult = false;

      if (parameters.Count == 2)
      {
        try
        {
          var splitTestId = parameters[0];
          var splitTestSide = parameters[1];

          int result;
          if (int.TryParse(splitTestId, out result))
          {
            var splitTestingProvider = providerContainer.Resolve<ISplitTestingProvider>();
            var activeSplitTestSide = splitTestingProvider.GetSplitTestingSide(result);

            if (activeSplitTestSide != null && !string.IsNullOrEmpty(activeSplitTestSide.Name))
            {
              evaluationResult = string.Equals(splitTestSide, activeSplitTestSide.Name, StringComparison.OrdinalIgnoreCase);
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
