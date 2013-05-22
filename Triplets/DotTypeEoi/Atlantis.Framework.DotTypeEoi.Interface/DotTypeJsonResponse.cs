using System.Runtime.Serialization;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class DotTypeJsonResponse
  {
    [DataMember(Name = "eoi")]
    public IDotTypeEoiResponse DotTypeEoiResponse { get; set; }
  }
}
