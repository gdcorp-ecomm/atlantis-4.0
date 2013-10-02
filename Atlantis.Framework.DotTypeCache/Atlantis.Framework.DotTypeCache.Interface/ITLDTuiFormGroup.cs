using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Interface
{
// ReSharper disable InconsistentNaming
  public interface ITLDTuiFormGroup
// ReSharper restore InconsistentNaming
  {
    string Type { get; }
    string Value { get; }
    IEnumerable<ITLDTuiFormGroupLaunchPhase> FormGrouplaunchPhases { get; }
  }
}
