using System.Runtime.Serialization;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  [DataContract(Name = "dataSourceOption")]
  internal class DataSourceOption
  {
    [DataMember(Name = "key")]
    public string Key { get; set; }

    [DataMember(Name = "value")]
    public string Value { get; set; }
  }
}
