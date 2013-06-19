using System.Collections.Generic;
using Atlantis.Framework.RegDotTypeProductIds.Interface;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public static class PhaseHelper
  {
    private static readonly Dictionary<PreRegPhases, string> _phases = new Dictionary<PreRegPhases, string>();

    static PhaseHelper()
    {
      _phases.Add(PreRegPhases.SunriseA, "SRA");
      _phases.Add(PreRegPhases.SunriseB, "SRB");
      _phases.Add(PreRegPhases.SunriseC, "SRC");
      _phases.Add(PreRegPhases.Landrush, "LR");
      _phases.Add(PreRegPhases.GeneralAvailability, "GA");
    }

    public static DotTypeProductTypes GetDotTypeProductTypes(PreRegPhases preRegPhase, bool isForApplicationFee = false)
    {
      DotTypeProductTypes dotTypeProductType;
      switch (preRegPhase)
      {
        case PreRegPhases.SunriseA:
          dotTypeProductType = !isForApplicationFee ? DotTypeProductTypes.SunriseA : DotTypeProductTypes.SunriseAApplication;
          break;
        case PreRegPhases.SunriseB:
          dotTypeProductType = !isForApplicationFee ? DotTypeProductTypes.SunriseB : DotTypeProductTypes.SunriseBApplication;
          break;
        case PreRegPhases.SunriseC:
          dotTypeProductType = !isForApplicationFee ? DotTypeProductTypes.SunriseC : DotTypeProductTypes.SunriseCApplication;
          break;
        case PreRegPhases.Landrush:
          dotTypeProductType = !isForApplicationFee ? DotTypeProductTypes.Landrush : DotTypeProductTypes.LandrushApplication;
          break;
        case PreRegPhases.GeneralAvailability:
          dotTypeProductType = DotTypeProductTypes.GeneralAvailability;
          break;
        default:
          dotTypeProductType = DotTypeProductTypes.None;
          break;
      }

      return dotTypeProductType;
    }

    public static string GetPhaseCode(PreRegPhases preRegPhase)
    {
      string result;
      _phases.TryGetValue(preRegPhase, out result);

      return result;
    }
  }
}
