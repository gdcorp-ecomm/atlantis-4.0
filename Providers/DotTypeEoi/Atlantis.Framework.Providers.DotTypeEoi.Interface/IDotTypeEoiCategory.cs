using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IDotTypeEoiCategory
  {
    int CategoryId { get; set; }

    string Name { get; set; }

    string DisplayName { get; set; }

    int DisplayOrder { get; set; }

    IList<IDotTypeEoiSubCategory> SubCategories { get; }

    IList<IDotTypeEoiGtld> Gtlds { get; }
  }
}
