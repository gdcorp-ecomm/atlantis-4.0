using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.Providers.DotTypeEoi
{
  class CategoryData : ICategoryData
  {
    public CategoryData(IDotTypeEoiCategory category)
    {
      CategoryId = category.CategoryId;
      Name = category.Name;
    }

    public int CategoryId { get; set; }
    public string Name { get; set; }
  }
}
