using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.CDS
{
  public static class CDSProviderEngineRequests
  {
    private static int _cdsRequestType = 424;

    public static int CDSRequestType
    {
      get { return _cdsRequestType; }
      set { _cdsRequestType = value; }
    }
  }
}
