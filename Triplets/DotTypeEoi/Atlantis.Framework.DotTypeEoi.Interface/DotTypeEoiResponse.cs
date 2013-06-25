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

    private IList<IDotTypeEoiCategory> _categories;
    [IgnoreDataMember]
    public IList<IDotTypeEoiCategory> Categories
    {
      get
      {
        if (_categories == null)
        {
          if (CategoriesObject != null && CategoriesObject.CategoryList != null && CategoriesObject.CategoryList.Count > 0)
          {
            _categories = new List<IDotTypeEoiCategory>(CategoriesObject.CategoryList.Count);
            foreach (var category in CategoriesObject.CategoryList)
            {
              _categories.Add(category);
            }
          }
          else
          {
            _categories = new List<IDotTypeEoiCategory>(0);
          }
        }

        return _categories;
      }
    }

    private IDictionary<int, IDotTypeEoiGtld> _allGtlds;
    [IgnoreDataMember]
    public IDictionary<int, IDotTypeEoiGtld> AllGtlds
    {
      get
      {
        if (_allGtlds == null)
        {
          if (Categories != null)
          {
            _allGtlds = new Dictionary<int, IDotTypeEoiGtld>(1024);
            foreach (IDotTypeEoiCategory dotTypeEoiCategory in Categories)
            {
              if (dotTypeEoiCategory.SubCategories != null)
              {
                foreach (IDotTypeEoiSubCategory dotTypeEoiSubCategory in dotTypeEoiCategory.SubCategories)
                {
                  foreach (IDotTypeEoiGtld dotTypeEoiGtld in dotTypeEoiCategory.Gtlds)
                  {
                    _allGtlds[dotTypeEoiGtld.Id] = new DotTypeEoiGtld(dotTypeEoiGtld, dotTypeEoiSubCategory);
                  }
                }
              }
            }
          }
          else
          {
            _allGtlds = new Dictionary<int, IDotTypeEoiGtld>(0);
          }
        }
        return _allGtlds;
      }
    }
  }
}
