using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Render.MarkupParser.Tests.ConditionHandlers
{
  internal class ParameterWhitespaceValidatorConditionHandler : IConditionHandler
  {
    public string ConditionName { get { return "parameterWhitespaceValidator"; } }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool success = true;

      foreach (string parameter in parameters)
      {
        if (!parameter.Equals(parameter.Trim()))
        {
          success = false;
          break;
        }
      }

      return success;
    }
  }
}
