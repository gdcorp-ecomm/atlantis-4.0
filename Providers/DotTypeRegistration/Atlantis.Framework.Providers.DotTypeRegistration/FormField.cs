using System.Collections.Generic;
using Atlantis.Framework.DotTypeForms.Interface;
using Atlantis.Framework.Providers.DotTypeRegistration.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Atlantis.Framework.Providers.DotTypeRegistration
{
  public class FormField : IFormField
  {
    private const string VALIDATE_FIELD_PREFIX = "tui-";

    [JsonConverter(typeof(StringEnumConverter))]
    public FormFieldTypes Type { get; set; }

    private string _name;
    public string Name
    {
      get { return _name; }
      set { _name = string.Concat(VALIDATE_FIELD_PREFIX, value); }
    }

    public string Value { get; set; }
    public string LabelText { get; set; }
    public string DescriptionText { get; set; }
    public string Required { get; set; }
    public string DefaultValue { get; set; }
    public IList<IDotTypeFormsItem> ItemCollection { get; set; }

    private IList<IDependsCollection> _dependsCollection;
    public IList<IDependsCollection> DependsCollection
    {
      get { return _dependsCollection; }
      set
      {
        _dependsCollection = value;
        foreach (var depend in _dependsCollection)
        {
          depend.FieldName = string.Concat(VALIDATE_FIELD_PREFIX, depend.FieldName);
        }
      } 
    }
  }
}