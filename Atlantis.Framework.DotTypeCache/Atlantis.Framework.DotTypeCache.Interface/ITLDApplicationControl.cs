using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeCache.Interface
{
  public interface ITLDApplicationControl
  {
    string DotTypeDescription { get; }

    string LandingPageUrl { get; }

    bool IsMultiRegistry { get; }

    Dictionary<string, ITLDTuiFormGroup> TuiFormGroups { get; }
  }
}
