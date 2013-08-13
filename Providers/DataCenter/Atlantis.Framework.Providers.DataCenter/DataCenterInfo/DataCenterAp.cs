using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DataCenter.DataCenterInfo
{
  internal class DataCenterAp : IDataCenterInfo
  {
    private readonly static HashSet<string> _asiaPacificCountries;

    static DataCenterAp()
    {
      _asiaPacificCountries = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { 
          "af", "aq", "am", "au", "az", "bh", "bd", "bt", "bn", "kh", "cn", "cx", "cc", "ck", "cy", "fm", "fj", "ge", "hm", "hk", 
          "in", "id", "iq", "il", "jp", "jo", "kz", "ki", "kp", "kr", "kw", "kg", "la", "lb", "mo", "my", "mv", "mh", "mn", "mm", 
          "nr", "np", "nz", "nu", "nf", "om", "pk", "pw", "ps", "pg", "ph", "qa", "sa", "sg", "sb", "lk", "tw", "tj", "th", "tl", 
          "tk", "to", "tr", "tm", "tv", "ae", "uz", "vu", "vn", "ws", "ye", "io", "ir", "sy", "as", "pf", "gu", "nc", "mp", "pn", 
          "um", "wf" };
    }

    public string Code
    {
      get { return "ap"; }
    }

    public bool IsValidForCountryCode(string countryCode)
    {
      return _asiaPacificCountries.Contains(countryCode);
    }
  }
}
