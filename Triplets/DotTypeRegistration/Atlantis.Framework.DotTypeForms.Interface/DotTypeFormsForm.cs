using System.Collections.Generic;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;

namespace Atlantis.Framework.DotTypeForms.Interface
{
  public class DotTypeFormsForm : IDotTypeFormsForm
  {
    public string FormName { get; set; }
    public string FormDescription { get; set; }
    public string FormGetMethod { get; set; }
    public string FormSetMethod { get; set; }
    public IList<IDotTypeFormsValidationRule> ValidationRuleCollection { get; set; }
    public IList<IDotTypeFormsField> FieldCollection { get; set; }
  }
}
