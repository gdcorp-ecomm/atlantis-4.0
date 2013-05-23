using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class DotTypeEoiGtlds
  {
    [DataMember(Name = "gTld")]
    public IList<DotTypeEoiGtld> GtldList { get; set; }
  }
}
