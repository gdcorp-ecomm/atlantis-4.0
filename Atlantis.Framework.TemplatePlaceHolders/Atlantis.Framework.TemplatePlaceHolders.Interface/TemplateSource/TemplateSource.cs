using System.Runtime.Serialization;

namespace Atlantis.Framework.TemplatePlaceHolders.Interface
{
  [DataContract(Name = "templateSource")]
  internal class TemplateSource : ITemplateSource
  {
    [DataMember(Name = "format")]
    public string Format { get; set; }

    [DataMember(Name = "source")]
    public string Source { get; set; }

    [DataMember(Name = "sourceAssembly", IsRequired = false)]
    public string SourceAssembly { get; set; }

    [DataMember(Name = "requestKey")]
    public string RequestKey { get; set; }
  }
}
