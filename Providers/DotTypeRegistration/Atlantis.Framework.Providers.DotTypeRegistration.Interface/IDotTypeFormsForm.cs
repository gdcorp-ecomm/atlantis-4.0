using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IDotTypeFormsForm
  {
    string FormName { get; set; }
    string FormDescription { get; set; }
    string FormGetMethod { get; set; }
    string FormSetMethod { get; set; }
    IList<IDotTypeFormsValidationRule> ValidationRuleCollection { get; set; }
    IList<IDotTypeFormsField> FieldCollection { get; set; }
  }
}
