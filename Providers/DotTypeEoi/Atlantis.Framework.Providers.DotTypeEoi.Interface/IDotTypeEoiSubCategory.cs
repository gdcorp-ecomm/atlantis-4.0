using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IDotTypeEoiSubCategory
  {
    int SubCategoryId { get; set; }

    string Name { get; set; }

    string DisplayName { get; set; }

    int DisplayOrder { get; set; }

    int ShowSelectAll { get; set; }

    IList<IDotTypeEoiGtld> Gtlds { get; }
  }
}
