using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class DotTypeEoiSubCategory
  {
    [DataMember(Name = "@id")]
    public int SubCategoryId { get; set; }

    [DataMember(Name = "@name")]
    public string Name { get; set; }

    [DataMember(Name = "@displayOrder")]
    public int DisplayOrder { get; set; }

    [DataMember(Name = "@showSelectAll")]
    public int ShowSelectAll { get; set; }

    [DataMember(Name = "gTlds")]
    public DotTypeEoiGtlds Gtlds { get; set; }
  }
}
