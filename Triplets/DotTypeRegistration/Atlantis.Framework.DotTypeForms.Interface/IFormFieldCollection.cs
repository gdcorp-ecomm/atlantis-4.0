using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public interface IFormFieldCollection
  {
    string Label { get; set; }
    string ToggleText { get; set; }
    string ToggleValue { get; set; }

    IList<IDotTypeFormsField> FieldCollection { get; set; }
  }
}
