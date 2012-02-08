using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.HDVD.Interface.Aries;

namespace Atlantis.Framework.HDVD.Interface.Helpers
{
  public static class SerivceHelper
  {
    public static HCCAPIServiceAries GetServiceReference(string wsUrl)
    {
      return new HCCAPIServiceAries() { Url = wsUrl }; 
    }
  }
}
