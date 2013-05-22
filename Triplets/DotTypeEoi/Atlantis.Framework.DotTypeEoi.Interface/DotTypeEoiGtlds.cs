using System.Collections.Generic;
using System.Runtime.Serialization;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class DotTypeEoiGtlds
  {
    [DataMember(Name = "gTld")]
    public IList<IDotTypeEoiGtld> GtldList { get; set; }
  }
}
