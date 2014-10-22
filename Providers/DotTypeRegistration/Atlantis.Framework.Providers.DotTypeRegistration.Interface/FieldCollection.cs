using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeRegistration.Interface
{
  public class FieldCollection : IFieldCollection
  {
    public string Label { get; set; }
    public string ToggleText { get; set; }
    public string ToggleValue { get; set; }

    public IList<IFormField> Fields { get; set; }
  }
}
