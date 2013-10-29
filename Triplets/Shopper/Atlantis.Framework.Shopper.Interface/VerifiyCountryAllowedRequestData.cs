using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Shopper.Interface
{
  public class VerifiyCountryAllowedRequestData: RequestData
  {
  
    public VerifiyCountryAllowedRequestData()
    {
      
    }

    public override string GetCacheMD5()
    {
      return "GetBlockedCountryList";
    }
  }
}
