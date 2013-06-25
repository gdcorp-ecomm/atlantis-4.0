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

    [DataMember(Name = "@displayName")]
    public string DisplayName { get; set; }

    [DataMember(Name = "@displayOrder")]
    public int DisplayOrder { get; set; }

    [DataMember(Name = "subCategories")]
    public DotTypeEoiSubCategories SubCategoriesObject { get; set; }

    private IList<IDotTypeEoiSubCategory> _subCategories;
    [IgnoreDataMember]
    public IList<IDotTypeEoiSubCategory> SubCategories
    {
      get
      {
        if (_subCategories == null)
        {
          if (SubCategoriesObject != null && SubCategoriesObject.SubCategoryListObject != null && SubCategoriesObject.SubCategoryListObject.Count > 0)
          {
            _subCategories = new List<IDotTypeEoiSubCategory>(SubCategoriesObject.SubCategoryListObject.Count);
            foreach (var subCategory in SubCategoriesObject.SubCategoryListObject)
            {
              _subCategories.Add(subCategory);
            }
          }
          else
          {
            _subCategories = new List<IDotTypeEoiSubCategory>(0);
          }
        }

        return _subCategories;
      }
    }

    private IList<IDotTypeEoiGtld> _gtlds;
    [IgnoreDataMember]
    public IList<IDotTypeEoiGtld> Gtlds
    {
      get
      {
        if (_gtlds == null)
        {
          if (SubCategoriesObject != null && SubCategoriesObject.SubCategoryListObject != null && SubCategoriesObject.SubCategoryListObject.Count > 0)
          {
            _gtlds = new List<IDotTypeEoiGtld>(1024);
            foreach (var subCategory in SubCategoriesObject.SubCategoryListObject)
            {
              if (subCategory.GtldsObject != null && subCategory.GtldsObject.GtldList != null && subCategory.GtldsObject.GtldList.Count > 0)
              {
                foreach (var gtld in subCategory.GtldsObject.GtldList)
                {
                  _gtlds.Add(new DotTypeEoiGtld(gtld, subCategory));
                }
              }
            }
          }
          else
          {
            _gtlds = new List<IDotTypeEoiGtld>(0);
          }
        }

        return _gtlds;
      }
    }
  }
}
