using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class DotTypeEoiSubCategories
  {
    [DataMember(Name = "subCategory")]
    public IList<DotTypeEoiSubCategory> SubCategoryListObject { get; set; }
  }
}
