using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Interface
{
// ReSharper disable InconsistentNaming
  public interface ITLDLaunchPhase
// ReSharper restore InconsistentNaming
  {
    string Code { get; }
    string Type { get; }
    string Value { get; }
    IEnumerable<ITLDLaunchPhasePeriod> Periods { get; }
    IEnumerable<string> Duplicates { get; }
    bool UpdatesEnabled { get; }
    bool RefundsEnabled { get; }
    bool PrivacyEnabled { get; }
  }
}
