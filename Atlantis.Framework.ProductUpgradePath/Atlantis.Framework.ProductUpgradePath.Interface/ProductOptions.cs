using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.ProductUpgradePath.Interface
{
  public enum DurationUnit
  {
    Month,
    Quarterly,
    Year
  }
  public class ProductOptions
  {
    public int Duration { get; set; }
    public DurationUnit DurationUnit { get; set; }

    public ProductOptions(int duration, DurationUnit durationUnit)
    {
      Duration = duration;
      DurationUnit = durationUnit;
    }
  }
}
