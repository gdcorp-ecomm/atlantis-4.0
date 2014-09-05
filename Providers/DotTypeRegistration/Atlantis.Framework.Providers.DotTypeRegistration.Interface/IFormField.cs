using System.Collections.Generic;
using Atlantis.Framework.DotTypeForms.Interface;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public interface IFormField
  {
    FormFieldTypes Type { get; set; }
    string Name { get; set; }
    string Value { get; set; }
    string LabelText { get; set; }
    string DescriptionText { get; set; }
    string Required { get; set; }
    string DefaultValue { get; set; }
    IList<IDotTypeFormsItem> ItemCollection { get; set; }
    IList<IDependsCollection> DependsCollection { get; set; }
  }
}
