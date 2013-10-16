using System;
using System.Collections.Generic;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache
{
  public static class LaunchPhaseMappings
  {
    private static IDictionary<LaunchPhases, string> _phaseMappingDictionary = new Dictionary<LaunchPhases, string> { { LaunchPhases.SunriseA , "SRA"},
                                                                                                                      { LaunchPhases.SunriseB , "SRB"},
                                                                                                                      { LaunchPhases.SunriseC , "SRC"},

                                                                                                                      { LaunchPhases.Landrush , "LR"},

                                                                                                                      { LaunchPhases.EarlyRegistration3Day , "ER3"},
                                                                                                                      { LaunchPhases.EarlyRegistration4Day , "ER4"},
                                                                                                                      { LaunchPhases.EarlyRegistration5Day , "ER5"},
                                                                                                                      { LaunchPhases.EarlyRegistration6Day , "ER6"},
                                                                                                                      { LaunchPhases.EarlyRegistration7Day , "ER7"},

                                                                                                                      { LaunchPhases.GeneralAvailability , "GA"} };

    private static IDictionary<LaunchPhaseGroupTypes, string> _phaseGroupMappingDictionary = new Dictionary<LaunchPhaseGroupTypes, string> { { LaunchPhaseGroupTypes.Sunrise , "SR"},
                                                                                                                                     { LaunchPhaseGroupTypes.Landrush , "LR"},
                                                                                                                                     { LaunchPhaseGroupTypes.EarlyRegistration , "ER"},
                                                                                                                                     { LaunchPhaseGroupTypes.GeneralAvailability , "GA"} };

    internal static string GetCodePrefix(LaunchPhaseGroupTypes launchPhaseGroupType)
    {
      string codePrefix;

      if (!_phaseGroupMappingDictionary.TryGetValue(launchPhaseGroupType, out codePrefix))
      {
        codePrefix = string.Empty;
      }

      return codePrefix;
    }

    internal static string GetCode(LaunchPhases launchPhase)
    {
      string code;

      if (!_phaseMappingDictionary.TryGetValue(launchPhase, out code))
      {
        code = string.Empty;
      }

      return code;
    }

    public static LaunchPhases GetPhase(string code)
    {
      LaunchPhases launchPhase = LaunchPhases.Invalid;

      foreach (KeyValuePair<LaunchPhases, string> phaseCodePair in _phaseMappingDictionary)
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
