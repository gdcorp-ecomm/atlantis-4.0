using System.Collections.Generic;
using System.Runtime.Serialization;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class DotTypeEoiCategories
  {
    [DataMember(Name = "category")]
    public IList<IDotTypeEoiCategory> CategoryList { get; set; }
  }
}
