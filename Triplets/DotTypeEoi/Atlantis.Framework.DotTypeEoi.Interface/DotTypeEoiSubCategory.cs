using System.Collections.Generic;
using System.Runtime.Serialization;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class DotTypeEoiSubCategory : IDotTypeEoiSubCategory
  {
    [DataMember(Name = "@id")]
    public int SubCategoryId { get; set; }

    [DataMember(Name = "@name")]
    public string Name { get; set; }

    [DataMember(Name = "@displayName")]
    public string DisplayName { get; set; }

    [DataMember(Name = "@displayOrder")]
    public int DisplayOrder { get; set; }

    [DataMember(Name = "@showSelectAll")]
    public int ShowSelectAll { get; set; }

    [DataMember(Name = "gTlds")]
    public DotTypeEoiGtlds GtldsObject { get; set; }

    [IgnoreDataMember()]
    public IList<IDotTypeEoiGtld> Gtlds
    {
      get
      {
        IList<IDotTypeEoiGtld> gtlds;
        if (GtldsObject != null && GtldsObject.GtldList != null && GtldsObject.GtldList.Count > 0)
        {
          gtlds = new List<IDotTypeEoiGtld>(GtldsObject.GtldList.Count);
          foreach (var gtld in GtldsObject.GtldList)
          {
            var gtldTemp = gtld;
            gtldTemp.IsFeatured = Name.ToLower() == "featured";
            gtlds.Add(gtld);
          }
        }
        else
        {
          gtlds = new List<IDotTypeEoiGtld>();
        }

        return gtlds;
      }
    }
  }
}
