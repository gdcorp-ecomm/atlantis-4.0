using System.Runtime.Serialization;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class DotTypeEoiGtld : IDotTypeEoiGtld
  {
    [DataMember(Name = "@id")]
    public int Id { get; set; }

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

    [DataMember(Name = "@comments")]
    public string Comments { get; set; }

    [DataMember(Name = "@hasLeafPage")]
    public bool HasLeafPage { get; set; }

    [IgnoreDataMember]
    public bool IsFeatured { get; private set; }

    [IgnoreDataMember]
    public ActionButtonTypes ActionButtonType { get; set; }

    public DotTypeEoiGtld()
    {
    }

    public DotTypeEoiGtld(IDotTypeEoiGtld gtld, IDotTypeEoiSubCategory subCategory)
    {
      Id = gtld.Id;
      Name = gtld.Name;
      IsIdn = gtld.IsIdn;
      IdnScript = gtld.IdnScript;
      EnglishMeaning = gtld.EnglishMeaning;
      ALabel = gtld.ALabel;
      GtldSubCategoryId = gtld.GtldSubCategoryId;
      DisplayOrder = gtld.DisplayOrder;
      Comments = gtld.Comments;
      HasLeafPage = gtld.HasLeafPage;

      IsFeatured = subCategory.Name.ToLower() == "featured";
    }
  }
}
