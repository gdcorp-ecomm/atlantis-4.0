using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IFieldCollection
  {
    string Label { get; set; }
    string ToggleText { get; set; }
    string ToggleValue { get; set; }

    IList<IFormField> Fields { get; set; }
  }
}
