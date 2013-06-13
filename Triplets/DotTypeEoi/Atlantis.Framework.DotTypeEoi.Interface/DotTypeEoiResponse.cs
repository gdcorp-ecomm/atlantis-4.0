using System.Collections.Generic;
using System.Runtime.Serialization;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  [DataContract]
  public class DotTypeEoiResponse : IDotTypeEoiResponse
  {
    [DataMember(Name = "@displayTime")]
    public string DisplayTime { get; set; }

    [DataMember(Name = "categories")]
    public DotTypeEoiCategories CategoriesObject { get; set; }

    [IgnoreDataMember()]
    public IList<IDotTypeEoiCategory> Categories
    {
      get
      {
        IList<IDotTypeEoiCategory> categories;
        if (CategoriesObject != null && CategoriesObject.CategoryList != null && CategoriesObject.CategoryList.Count > 0)
        {
          categories = new List<IDotTypeEoiCategory>(CategoriesObject.CategoryList.Count);
          foreach (var category in CategoriesObject.CategoryList)
          {
            categories.Add(category);
          }
        }
        else
        {
          categories = new List<IDotTypeEoiCategory>();
        }

        return categories;
      }
    }
  }
}
