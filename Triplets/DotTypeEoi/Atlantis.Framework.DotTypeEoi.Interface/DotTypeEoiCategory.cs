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

    [IgnoreDataMember()]
    public IList<IDotTypeEoiSubCategory> SubCategories
    {
      get
      {
        IList<IDotTypeEoiSubCategory> subCategories;
        if (SubCategoriesObject != null && SubCategoriesObject.SubCategoryListObject != null && SubCategoriesObject.SubCategoryListObject.Count > 0)
        {
          subCategories = new List<IDotTypeEoiSubCategory>(SubCategoriesObject.SubCategoryListObject.Count);
          foreach (var subCategory in SubCategoriesObject.SubCategoryListObject)
          {
            subCategories.Add(subCategory);
          }
        }
        else
        {
          subCategories = new List<IDotTypeEoiSubCategory>();
        }

        return subCategories;
      }
    }

    [IgnoreDataMember()]
    public IList<IDotTypeEoiGtld> Gtlds
    {
      get
      {
        IList<IDotTypeEoiGtld> gtlds = new List<IDotTypeEoiGtld>();
        if (SubCategoriesObject != null && SubCategoriesObject.SubCategoryListObject != null && SubCategoriesObject.SubCategoryListObject.Count > 0)
        {
          foreach (var subCategory in SubCategoriesObject.SubCategoryListObject)
          {
            if (subCategory.GtldsObject != null && subCategory.GtldsObject.GtldList != null && subCategory.GtldsObject.GtldList.Count > 0)
            {
              bool isFeatured = subCategory.Name.ToLower() == "featured";
              IList<IDotTypeEoiGtld> tempGtldList = new List<IDotTypeEoiGtld>(subCategory.GtldsObject.GtldList.Count);
              foreach (var gtld in subCategory.GtldsObject.GtldList)
              {
                gtld.IsFeatured = isFeatured;
                tempGtldList.Add(gtld);
              }
              foreach (var tempGtld in tempGtldList)
              {
                gtlds.Add(tempGtld);
              }
            }
          }
        }

        return gtlds;
      }
    }
  }
}
