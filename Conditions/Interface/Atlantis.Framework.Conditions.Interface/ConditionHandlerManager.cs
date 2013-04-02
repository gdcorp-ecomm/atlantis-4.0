using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Conditions.Interface
{
  public static class ConditionHandlerManager
  {
    private static readonly Dictionary<string, IConditionHandler> _conditionHandlers;
    private static readonly Dictionary<string, ConditionHandlerStats> _conditionHandlersStats;

    static ConditionHandlerManager()
    {
      _conditionHandlers = new Dictionary<string, IConditionHandler>(StringComparer.OrdinalIgnoreCase);
      _conditionHandlersStats = new Dictionary<string, ConditionHandlerStats>(StringComparer.OrdinalIgnoreCase);
    }

    private static void LogException(string message, string sourceFunction, string condtionName)
    {
      AtlantisException aex = new AtlantisException(sourceFunction,
                                                    "0",
                                                    message,
                                                    "ConditionName: " + condtionName,
                                                    null,
                                                    null);

      Engine.Engine.LogAtlantisException(aex);
    }

    private static void AddConditionHandler(IConditionHandler conditionHandler)
    {
      if (!_conditionHandlers.ContainsKey(conditionHandler.ConditionName))
      {
        _conditionHandlers[conditionHandler.ConditionName] = conditionHandler;
        _conditionHandlersStats[conditionHandler.ConditionName] = new ConditionHandlerStats();
      }
      else
      {
        LogException(string.Format("Attempted to add duplicate IConditionHandler ConditionName. ConditionName: \"{0}\", Type: \"{1}\"", conditionHandler.ConditionName, conditionHandler.GetType()), 
                     "ConditionHandlerManager.AddConditionHandler()", 
                     "ConditionName: " + conditionHandler.ConditionName);
      }
    }

    public static void AutoRegisterConditionHandlers(params Assembly[] additionalAssemblies)
    {
      using (DynamicConditionHandlerLoader conditionHandlerLoader = new DynamicConditionHandlerLoader(additionalAssemblies))
      {
        foreach (var lazyHandler in conditionHandlerLoader.ConditionHandlersFound)
        {
          RegisterConditionHandler(lazyHandler.Value);
        }
      }
    }

    public static void RegisterConditionHandler(IConditionHandler conditionHandler)
    {
      if (conditionHandler == null)
      {
        throw new ArgumentException("ConditionHandler cannot be null.");
      }

      if (string.IsNullOrEmpty(conditionHandler.ConditionName))
      {
        throw new ArgumentException("ConditionHandler does not have valid ConditionName.");
      }

      AddConditionHandler(conditionHandler);
    }

    public static bool EvaluateCondition(string conditionName, IEnumerable<string> parameters, IProviderContainer providerContainer)
    {
      bool conditionResult = false;
      Stopwatch callTimer = null;
      ConditionHandlerStats stats;

      bool isException = false;

      IConditionHandler conditionHandler;
      if (!_conditionHandlers.TryGetValue(conditionName, out conditionHandler))
      {
        conditionHandler = new NullConditionHandler();
      }

      try
      {
        callTimer = Stopwatch.StartNew();
        conditionResult = conditionHandler.EvaluateCondition(conditionName, parameters, providerContainer);
        callTimer.Stop();
      }
      catch (Exception ex)
      {
        isException = true;

        if (callTimer != null)
        {
          callTimer.Stop();
        }

        if (_conditionHandlersStats.TryGetValue(conditionHandler.ConditionName, out stats))
        {
          stats.LogFailure(callTimer);
        }

        LogException(string.Format("Unhandled IConditionHandler Exception: " + ex.Message),
                     "EvaluateCondition()",
                     conditionName);
      }

      if (!isException && _conditionHandlersStats.TryGetValue(conditionHandler.ConditionName, out stats))
      {
        stats.LogSuccess(callTimer);
      }

      return conditionResult;
    }
  }
}