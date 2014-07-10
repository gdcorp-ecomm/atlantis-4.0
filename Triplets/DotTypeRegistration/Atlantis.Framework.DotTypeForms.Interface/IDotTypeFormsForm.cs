using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public interface IDotTypeFormsForm
  {
    string FormName { get; set; }
    string FormDescription { get; set; }
    string FormType { get; set; }
    IList<IDotTypeFormsField> FieldCollection { get; set; }
  }
}
