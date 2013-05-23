using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class DotTypeEoiCategories
  {
    [DataMember(Name = "category")]
    public IList<DotTypeEoiCategory> CategoryList { get; set; }
  }
}
