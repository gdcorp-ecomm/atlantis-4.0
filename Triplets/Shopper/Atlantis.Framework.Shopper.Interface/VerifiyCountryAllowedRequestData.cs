using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Shopper.Interface
{
  public class VerifiyCountryAllowedRequestData: RequestData
  {
    private string _countryCode;
    public string CountryCode
    {
      get
      {
        return _countryCode;
      }

      protected set
      {
        if (!string.IsNullOrEmpty(value))
        {
          _countryCode = value;
        }
      }
    }

    public VerifiyCountryAllowedRequestData(string countryCode)
    {
      CountryCode = countryCode;
    }

    public override string GetCacheMD5()
    {
      return "GetBlockedCountryList";
    }
  }
}
