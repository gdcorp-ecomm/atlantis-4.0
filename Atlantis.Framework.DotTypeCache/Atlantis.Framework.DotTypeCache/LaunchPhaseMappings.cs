using System;
using System.Collections.Generic;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache
{
  public static class LaunchPhaseMappings
  {
    private static readonly IDictionary<LaunchPhaseGroupTypes, string> PhaseGroupMappingDictionary = new Dictionary<LaunchPhaseGroupTypes, string> { { LaunchPhaseGroupTypes.Sunrise , "SR"},
                                                                                                                                     { LaunchPhaseGroupTypes.Landrush , "LR"},
                                                                                                                                     { LaunchPhaseGroupTypes.EarlyRegistration , "ER"},
                                                                                                                                     { LaunchPhaseGroupTypes.GeneralAvailability , "GA"} };

    internal static string GetCodePrefix(LaunchPhaseGroupTypes launchPhaseGroupType)
    {
      string codePrefix;

      if (!PhaseGroupMappingDictionary.TryGetValue(launchPhaseGroupType, out codePrefix))
      {
        codePrefix = string.Empty;
      }

      return codePrefix;
    }

    public static string GetCode(LaunchPhases launchPhase)
    {
      string code;

      if (!PhaseMappingsDictionary.PhaseMappings.TryGetValue(launchPhase, out code))
      {
        code = string.Empty;
      }

      return code;
    }

    public static LaunchPhases GetPhase(string code)
    {
      var launchPhase = LaunchPhases.Invalid;

      foreach (var phaseCodePair in PhaseMappingsDictionary.PhaseMappings)
      {
        if (phaseCodePair.Value.Equals(code, StringComparison.OrdinalIgnoreCase))
        {
          launchPhase = phaseCodePair.Key;
          break;
        }
      }

      return launchPhase;
    }
  }
}
