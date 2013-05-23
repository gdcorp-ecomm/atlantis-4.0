using System.Runtime.Serialization;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class DotTypeEoiSubCategories
  {
    [DataMember(Name = "subCategory")]
    public DotTypeEoiSubCategory SubCategoryObject { get; set; }
  }
}
