using System.Runtime.Serialization;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class DotTypeEoiJsonResponse
  {
    [DataMember(Name = "eoi")]
    public DotTypeEoiResponse DotTypeEoiResponse { get; set; }
  }
}
