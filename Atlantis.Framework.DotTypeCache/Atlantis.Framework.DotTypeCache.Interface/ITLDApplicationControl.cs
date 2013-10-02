using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Interface
{
// ReSharper disable InconsistentNaming
  public interface ITLDApplicationControl
// ReSharper restore InconsistentNaming
  {
    string DotTypeDescription { get; }
    string LandingPageUrl { get; }
    bool IsMultiRegistry { get; }
    Dictionary<string, ITLDTuiFormGroup> TuiFormGroups { get; }
  }
}
