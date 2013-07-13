using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IFormField
  {
    FormFieldTypes Type { get; set; }
    string Name { get; set; }
    string Value { get; set; }
    IList<IDotTypeFormsItem> ItemCollection { get; set; }
  }
}
