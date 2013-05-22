using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DotTypeEoi.Interface
{
  public interface IDotTypeEoiCategory
  {
    int CategoryId { get; set; }
    string Name { get; set; }
    int DisplayOrder { get; set; }
    IList<IDotTypeEoiGtld> Gtlds { get; set; }
  }
}
