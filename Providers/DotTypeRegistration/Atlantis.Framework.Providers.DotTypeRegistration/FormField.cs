using System.Collections.Generic;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration
{
  public class FormField : IFormField
  {
    public FormFieldTypes Type { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public IList<IDotTypeFormsItem> ItemCollection { get; set; }
  }
}
