using System;
using System.Collections.Generic;
using System.Threading;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.SplitTesting.Interface;

namespace Atlantis.Framework.CH.SplitTesting
{
  public class IsActiveSplitTestingSideConditionHandler : IConditionHandler
  {
    public string ConditionName
    {
      get { return "isActiveSplitTestingSide"; }
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

          var splitTestingProvider = providerContainer.Resolve<ISplitTestingProvider>();
          var side = splitTestingProvider.GetSplitTestingSide(Convert.ToInt32(splitTestId));

          if (!string.IsNullOrEmpty(side))
          {
            evaluationResult = string.Equals(splitTestSide, side, StringComparison.OrdinalIgnoreCase);
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
