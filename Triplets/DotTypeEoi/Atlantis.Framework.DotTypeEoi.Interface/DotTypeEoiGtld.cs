using System.Runtime.Serialization;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class DotTypeEoiGtld : IDotTypeEoiGtld
  {
    [DataMember(Name = "@id")]
    public int SubCategoryId { get; set; }

    [DataMember(Name = "@name")]
    public string Name { get; set; }

    [DataMember(Name = "@isIdn")]
    public int IsIdn { get; set; }

    [DataMember(Name = "@idnScript")]
    public string IdnScript { get; set; }

    [DataMember(Name = "@englishMeaning")]
    public string EnglishMeaning { get; set; }

    [DataMember(Name = "@aLabel")]
    public string ALabel { get; set; }

    [DataMember(Name = "@gTldSubCategoryId")]
    public int GtldSubCategoryId { get; set; }

    [DataMember(Name = "@displayOrder")]
    public int DisplayOrder { get; set; }
  }
}
