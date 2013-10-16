using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface ITLDTuiFormGroup
  {
    string Type { get; }

    string Value { get; }

    IEnumerable<ITLDTuiFormGroupLaunchPhase> FormGrouplaunchPhases { get; }
  }
}
