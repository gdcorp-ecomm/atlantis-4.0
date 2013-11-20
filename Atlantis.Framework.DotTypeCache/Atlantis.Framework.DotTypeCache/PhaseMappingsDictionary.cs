using System.Collections.Generic;
using Atlantis.Framework.DotTypeCache.Interface;

namespace Atlantis.Framework.DotTypeCache
{
  internal class PhaseMappingsDictionary
  {
    private static readonly IDictionary<LaunchPhases, string> Sunrise = new Dictionary<LaunchPhases, string>
    {
      {LaunchPhases.SunriseA, "SRA"},
      {LaunchPhases.SunriseB, "SRB"},
      {LaunchPhases.SunriseC, "SRC"},
      {LaunchPhases.SunriseD, "SRD"},
      {LaunchPhases.SunriseE, "SRE"},
      {LaunchPhases.SunriseF, "SRF"},
      {LaunchPhases.SunriseG, "SRG"},
      {LaunchPhases.SunriseH, "SRH"},
      {LaunchPhases.SunriseI, "SRI"},
      {LaunchPhases.SunriseJ, "SRJ"}
    };

    private static readonly IDictionary<LaunchPhases, string> Landrush = new Dictionary<LaunchPhases, string>
    {
      {LaunchPhases.Landrush, "LR"},
      {LaunchPhases.LandrushB, "LRB"},
      {LaunchPhases.LandrushC, "LRC"},
      {LaunchPhases.LandrushD, "LRD"},
      {LaunchPhases.LandrushE, "LRE"},
      {LaunchPhases.LandrushF, "LRF"},
      {LaunchPhases.LandrushG, "LRG"},
      {LaunchPhases.LandrushH, "LRH"},
      {LaunchPhases.LandrushI, "LRI"},
      {LaunchPhases.LandrushJ, "LRJ"}
    };

    private static readonly IDictionary<LaunchPhases, string> EarlyRegistration = new Dictionary<LaunchPhases, string>
    {
      {LaunchPhases.EarlyRegistration1Day, "ER1"},
      {LaunchPhases.EarlyRegistration2Day, "ER2"},
      {LaunchPhases.EarlyRegistration3Day, "ER3"},
      {LaunchPhases.EarlyRegistration4Day, "ER4"},
      {LaunchPhases.EarlyRegistration5Day, "ER5"},
      {LaunchPhases.EarlyRegistration6Day, "ER6"},
      {LaunchPhases.EarlyRegistration7Day, "ER7"}
    };

    private static readonly IDictionary<LaunchPhases, string> GeneralAvailability = new Dictionary<LaunchPhases, string>
    {
      {LaunchPhases.GeneralAvailability, "GA"},
      {LaunchPhases.GeneralAvailabilityB, "GAB"},
      {LaunchPhases.GeneralAvailabilityC, "GAC"},
      {LaunchPhases.GeneralAvailabilityD, "GAD"},
      {LaunchPhases.GeneralAvailabilityE, "GAE"},
      {LaunchPhases.GeneralAvailabilityF, "GAF"},
      {LaunchPhases.GeneralAvailabilityG, "GAG"},
      {LaunchPhases.GeneralAvailabilityH, "GAH"},
      {LaunchPhases.GeneralAvailabilityI, "GAI"},
      {LaunchPhases.GeneralAvailabilityJ, "GAJ"}
    };

    private static void AddRange(ICollection<KeyValuePair<LaunchPhases, string>> target, IEnumerable<KeyValuePair<LaunchPhases, string>> source)
    {
      foreach (var element in source)
      {
        target.Add(element);
      }
    }

    static IDictionary<LaunchPhases, string> _phaseMappings;
    internal static IDictionary<LaunchPhases, string> PhaseMappings
    {
      get
      {
        if (_phaseMappings == null)
        {
          _phaseMappings = new Dictionary<LaunchPhases, string>(36);
          AddRange(_phaseMappings, Sunrise);
          AddRange(_phaseMappings, Landrush);
          AddRange(_phaseMappings, EarlyRegistration);
          AddRange(_phaseMappings, GeneralAvailability);
        }

        return _phaseMappings;
      }
    }
  }
}
