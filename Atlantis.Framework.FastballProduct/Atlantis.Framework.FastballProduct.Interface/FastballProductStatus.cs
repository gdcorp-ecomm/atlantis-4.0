using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.FastballProduct.Interface
{
  public static class FastballProductStatus
  {
    public const int AppSettingOff = 0;
    public const int DelayedOff = 1;
    public const int Valid = 2;
    public const int ValidCached = 3;
    public const int Failed = 4;
    public const int FailedCached = 5;
  }
}
