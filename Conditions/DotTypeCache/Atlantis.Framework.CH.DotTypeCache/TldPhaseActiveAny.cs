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
            DotTypeActivePhase = dotType.GetActiveClientRequestPhases();

            foreach (var phases in parameters)
            {
              switch (phases.ToUpperInvariant())
              {
                case TokenPreReg.GeneralAvailability:
                  result = IsPhaseActive(PreRegPhases.GeneralAvailability);
                  break;
                case TokenPreReg.EarlyAccess:
                  result = IsPhaseActive(PreRegPhases.Landrush);
                  break;
                case TokenPreReg.Landrush:
                  result = IsPhaseActive(PreRegPhases.Landrush);
                  break;
                case TokenPreReg.Sunrise:
                  result = IsPhaseActive(PreRegPhases.SunriseA);
                  result = IsPhaseActive(PreRegPhases.SunriseB);
                  result = IsPhaseActive(PreRegPhases.SunriseC);
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

    private Dictionary<string, ITLDLaunchPhase> DotTypeActivePhase { get; set; }

    private PreRegPhases GetPreRegPhase(ITLDLaunchPhase tldLaunchPhase)
    {
      PreRegPhases result;

      if (Enum.TryParse(tldLaunchPhase.Type, out result))
      {
        return result;
      }

      return PreRegPhases.Invalid;
    }

    private bool IsPhaseActive(PreRegPhases phase)
    {
      var isPhaseActive = false;

      foreach (var tldDict in DotTypeActivePhase)
      {
        var preRegPhase = GetPreRegPhase(tldDict.Value);

        switch (preRegPhase)
        {
          case PreRegPhases.Landrush:
            if (preRegPhase == phase)
            {
              isPhaseActive = true;
            }
            break;
          case PreRegPhases.SunriseA:
            if (preRegPhase == phase)
            {
              isPhaseActive = true;
            }
            break;
          case PreRegPhases.SunriseB:
            if (preRegPhase == phase)
            {
              isPhaseActive = true;
            }
            break;
          case PreRegPhases.SunriseC:
            if (preRegPhase == phase)
            {
              isPhaseActive = true;
            }
            break;
          case PreRegPhases.GeneralAvailability:
            if (preRegPhase == phase)
            {
              isPhaseActive = true;
            }
            break;
        }
      }
      return isPhaseActive;
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