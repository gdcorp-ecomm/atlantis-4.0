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
      _phases.Add(LaunchPhases.Landrush, "LR");
      _phases.Add(LaunchPhases.GeneralAvailability, "GA");
    }

    public static DotTypeProductTypes GetDotTypeProductTypes(LaunchPhases phase, bool isForApplicationFee = false)
    {
      DotTypeProductTypes dotTypeProductType;
      switch (phase)
      {
        case LaunchPhases.SunriseA:
          dotTypeProductType = !isForApplicationFee ? DotTypeProductTypes.SunriseA : DotTypeProductTypes.SunriseAApplication;
          break;
        case LaunchPhases.SunriseB:
          dotTypeProductType = !isForApplicationFee ? DotTypeProductTypes.SunriseB : DotTypeProductTypes.SunriseBApplication;
          break;
        case LaunchPhases.SunriseC:
          dotTypeProductType = !isForApplicationFee ? DotTypeProductTypes.SunriseC : DotTypeProductTypes.SunriseCApplication;
          break;
        case LaunchPhases.Landrush:
          dotTypeProductType = !isForApplicationFee ? DotTypeProductTypes.Landrush : DotTypeProductTypes.LandrushApplication;
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