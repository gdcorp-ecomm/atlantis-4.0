using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Conditions.Interface
{
  public static class ConditionHandlerManager
  {
    private static Dictionary<string, IConditionHandler> _conditionHandlers;

    static ConditionHandlerManager()
    {
      _conditionHandlers = new Dictionary<string, IConditionHandler>(StringComparer.OrdinalIgnoreCase);
    }

    private static void AddConditionHandler(IConditionHandler conditionHandler)
    {
      if (!_conditionHandlers.ContainsKey(conditionHandler.ConditionName))
      {
        _conditionHandlers[conditionHandler.ConditionName] = conditionHandler;
      }
      else
      {
        AtlantisException aex = new AtlantisException("ConditionHandlerManager.AddConditionHandler()", 
                                                      "0", 
                                                      string.Format("Attempted to add duplicate IConditionHandler ConditionName. ConditionName: \"{0}\", Type: \"{1}\"", conditionHandler.ConditionName, conditionHandler.GetType()), 
                                                      conditionHandler.ConditionName, 
                                                      null, 
                                                      null);

        Engine.Engine.LogAtlantisException(aex);
      }
    }

    public static void AutoRegisterConditionHandlers()
    {
      AutoRegisterConditionHandlers(null);
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
      IConditionHandler conditionHandler;
      if (!_conditionHandlers.TryGetValue(conditionName, out conditionHandler))
      {
        conditionHandler = new NullConditionHandler();
      }

      return conditionHandler.EvaluateCondition(conditionName, parameters, providerContainer);
    }
  }
}