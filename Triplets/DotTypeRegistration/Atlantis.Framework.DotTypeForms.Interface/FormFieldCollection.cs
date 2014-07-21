using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeForms.Interface
{
 public class FormFieldCollection:IFormFieldCollection
  {
    public string Label { get; set; }
    public string ToggleText { get; set; }
    public string ToggleValue { get; set; }

    public IList<IDotTypeFormsField> FieldCollection { get; set; }
  }
}
