﻿using System.Collections.Generic;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormsForm : IDotTypeFormsForm
  {
    public string FormName { get; set; }
    public string FormLabel { get; set; }
    public string FormDescription { get; set; }
    public string FormType { get; set; }
    public string ValidationLevel { get; set; }
    public IList<IDotTypeFormsField> FieldCollection { get; set; }
    public IFormFieldCollection FormFieldCollection { get; set; }
  }
}
