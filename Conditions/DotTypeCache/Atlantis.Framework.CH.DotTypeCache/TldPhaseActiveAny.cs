using System;
using System.Collections.Generic;
using Atlantis.Framework.Conditions.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CH.DotTypeCache
{
  public class TldPhaseActiveAny : IConditionHandler
  {
    public string ConditionName { get { return "tldPhaseActiveAny"; } }

    public bool EvaluateCondition(string conditionName, IList<string> parameters, IProviderContainer providerContainer)
    {
      bool result = false;

      if (parameters.Count >= 2)
      {
        try
        {
          var dotTypeProvider = providerContainer.Resolve<IDotTypeProvider>();
          var formatDotType = parameters[0].Replace("'", string.Empty).ToUpperInvariant();
          var dotType = dotTypeProvider.GetDotTypeInfo(formatDotType);

          if (!dotType.DotType.Equals("INVALID"))
          {
            foreach (var phases in parameters)
            {
              switch (phases.ToUpperInvariant())
              {
                case TokenPreReg.GeneralAvailability:
                  result = dotType.IsLivePhase(LaunchPhases.GeneralAvailability);
                  break;
                case TokenPreReg.EarlyAccess:
                  result = dotType.IsLivePhase(LaunchPhases.Landrush);
                  break;
                case TokenPreReg.Landrush:
                  result = dotType.IsLivePhase(LaunchPhases.Landrush);
                  break;
                case TokenPreReg.Sunrise:
                  result = dotType.IsLivePhase(LaunchPhases.SunriseA);
                  result = dotType.IsLivePhase(LaunchPhases.SunriseB);
                  result = dotType.IsLivePhase(LaunchPhases.SunriseC);
                  break;
              }

              if (result)
              {
                break;
              }

            }
          }
          else
          {
            var aex = new AtlantisException("tldPhaseActiveAny EvaluateCondition", "0", "first parameter must be a TLD", string.Empty, null, null);
            Engine.Engine.LogAtlantisException(aex);
          }
        }
        catch (Exception ex)
        {
          var aex = new AtlantisException("tldPhaseActiveAny EvaluateCondition", "0", ex.Message + Environment.NewLine + ex.StackTrace, string.Empty, null, null);
          Engine.Engine.LogAtlantisException(aex);
        }
      }
      else
      {
        var aex = new AtlantisException("tldPhaseActiveAny EvaluateCondition", "0", "must have at least two parameters", string.Empty, null, null);
        Engine.Engine.LogAtlantisException(aex);
      }

      return result;
    }
  }
}

public static class TokenPreReg
{
  public const string Sunrise = "SR";
  public const string EarlyAccess = "EA";
  public const string Landrush = "LR";
  public const string GeneralAvailability = "GA";
}