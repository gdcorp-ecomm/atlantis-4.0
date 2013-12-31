using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface ITLDTuiFormGroup
  {
    string Type { get; }

    IEnumerable<ITLDTuiFormGroupLaunchPhase> FormGrouplaunchPhases { get; }
  }
}
