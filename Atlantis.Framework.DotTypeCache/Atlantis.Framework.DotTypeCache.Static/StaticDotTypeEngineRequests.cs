using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.DotTypeCache.Static
{
  public static class StaticDotTypeEngineRequests
  {
    public static int ExtendedTLDData { get; set; }

    static StaticDotTypeEngineRequests()
    {
      ExtendedTLDData = 668;
    }
  }
}
