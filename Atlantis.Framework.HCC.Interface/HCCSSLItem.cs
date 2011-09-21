using System.Runtime.Serialization;

namespace Atlantis.Framework.HCC.Interface
{
  [DataContract(Name = "hccsslitem")]
  public class HCCSSLItem
  {
    [DataMember(Name = "val")]
    public string Value { get; set; }
    
    [DataMember(Name = "txt")]
    public string DisplayText {get; set; }    
  }
    
}
