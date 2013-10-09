using System;
using System.Collections.Generic;
using Atlantis.Framework.RegDotTypeProductIds.Interface;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public static class PhaseHelper
  {
    private static readonly Dictionary<LaunchPhases, string> _phases = new Dictionary<LaunchPhases, string>();

    static PhaseHelper()
    {
      _phases.Add(LaunchPhases.SunriseA, "SRA");
      _phases.Add(LaunchPhases.SunriseB, "SRB");
      _phases.Add(LaunchPhases.SunriseC, "SRC");
      _phases.Add(LaunchPhases.SunriseD, "SRD");
      _phases.Add(LaunchPhases.SunriseE, "SRE");
      _phases.Add(LaunchPhases.SunriseF, "SRF");
      _phases.Add(LaunchPhases.SunriseG, "SRG");
      _phases.Add(LaunchPhases.SunriseH, "SRH");
      _phases.Add(LaunchPhases.SunriseI, "SRI");
      _phases.Add(LaunchPhases.SunriseJ, "SRJ");
      _phases.Add(LaunchPhases.GeneralAvailability, "GA");
      _phases.Add(LaunchPhases.GeneralAvailabilityB, "GAB");
      _phases.Add(LaunchPhases.GeneralAvailabilityC, "GAC");
      _phases.Add(LaunchPhases.GeneralAvailabilityD, "GAD");
      _phases.Add(LaunchPhases.GeneralAvailabilityE, "GAE");
      _phases.Add(LaunchPhases.GeneralAvailabilityF, "GAF");
      _phases.Add(LaunchPhases.GeneralAvailabilityG, "GAG");
      _phases.Add(LaunchPhases.GeneralAvailabilityH, "GAH");
      _phases.Add(LaunchPhases.GeneralAvailabilityI, "GAI");
      _phases.Add(LaunchPhases.GeneralAvailabilityJ, "GAJ");
      _phases.Add(LaunchPhases.Landrush, "LR");
      _phases.Add(LaunchPhases.LandrushB, "LRB");
      _phases.Add(LaunchPhases.LandrushC, "LRC");
      _phases.Add(LaunchPhases.LandrushD, "LRD");
      _phases.Add(LaunchPhases.LandrushE, "LRE");
      _phases.Add(LaunchPhases.LandrushF, "LRF");
      _phases.Add(LaunchPhases.LandrushG, "LRG");
      _phases.Add(LaunchPhases.LandrushH, "LRH");
      _phases.Add(LaunchPhases.LandrushI, "LRI");
      _phases.Add(LaunchPhases.LandrushJ, "LRJ");
      _phases.Add(LaunchPhases.ThreeDayEarlyRegistration, "ER3");
      _phases.Add(LaunchPhases.FourDayEarlyRegistration, "ER4");
      _phases.Add(LaunchPhases.FiveDayEarlyRegistration, "ER5");
      _phases.Add(LaunchPhases.SixDayEarlyRegistration, "ER6");
      _phases.Add(LaunchPhases.SevenDayEarlyRegistration, "ER7");
      _phases.Add(LaunchPhases.RegularRegistration, "REGREG");
    }

    [Obsolete("Please dont use this method. This is provided for backward compatibility. This method will be removed in a future version.")]
    public static DotTypeProductTypes GetDotTypeProductTypes(LaunchPhases phase)
    {
      DotTypeProductTypes dotTypeProductType;
      switch (phase)
      {
        case LaunchPhases.SunriseA:
          dotTypeProductType = DotTypeProductTypes.SunriseA;
          break;
        case LaunchPhases.SunriseB:
          dotTypeProductType = DotTypeProductTypes.SunriseB;
          break;
        case LaunchPhases.SunriseC:
          dotTypeProductType = DotTypeProductTypes.SunriseC;
          break;
        case LaunchPhases.Landrush:
          dotTypeProductType = DotTypeProductTypes.Landrush;
          break;
        case LaunchPhases.GeneralAvailability:
          dotTypeProductType = DotTypeProductTypes.GeneralAvailability;
          break;
        default:
          dotTypeProductType = DotTypeProductTypes.None;
          break;
      }

      return dotTypeProductType;
    }

    public static string GetPhaseCode(LaunchPhases phase)
    {
      string result;
      _phases.TryGetValue(phase, out result);

      return result;
    }

    public static LaunchPhases GetLaunchPhase(string phaseCode)
    {
      var phase = LaunchPhases.Invalid;
      foreach (var pair in _phases)
      {
        if (phaseCode.Equals(pair.Value, StringComparison.OrdinalIgnoreCase))
        {
          phase = pair.Key;
          break;
        }
      }
      return phase;
    }
  }
}