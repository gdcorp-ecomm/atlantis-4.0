using System.Collections.Generic;
using System.Runtime.Serialization;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class DotTypeEoiCategory : IDotTypeEoiCategory
  {
    [DataMember(Name = "@id")]
    public int CategoryId { get; set; }

    [DataMember(Name = "@name")]
    public string Name { get; set; }

    [DataMember(Name = "@displayOrder")]
    public int DisplayOrder { get; set; }

    [DataMember(Name = "subCategories")]
    public DotTypeEoiSubCategories SubCategoriesObject { get; set; }

    [IgnoreDataMember()]
    public IList<IDotTypeEoiGtld> Gtlds
    {
      get
      {
        IList<IDotTypeEoiGtld> gtlds;
        if (SubCategoriesObject != null &&
            SubCategoriesObject.SubCategoryObject != null &&
            SubCategoriesObject.SubCategoryObject.GtldsObject != null)
        {
          gtlds = (IList<IDotTypeEoiGtld>)SubCategoriesObject.SubCategoryObject.GtldsObject.GtldList;
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
