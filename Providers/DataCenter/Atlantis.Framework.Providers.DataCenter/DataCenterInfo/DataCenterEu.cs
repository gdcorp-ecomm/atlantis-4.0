using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.DataCenter.DataCenterInfo
{
  internal class DataCenterEu : IDataCenterInfo
  {
    private readonly static HashSet<string> _europeanCountries;

    static DataCenterEu()
    {
      _europeanCountries = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { 
          "at", "be", "bg", "cy", "cz", "dk", "ee", "fi", "fr", "ge", "gr", "hu", "ie", "it", "lv", "lt", "lu", "mt", "nl", "pl", 
          "pt", "ro", "sk", "si", "es", "se", "uk", "al", "ad", "by", "ba", "hr", "fo", "de", "gi", "gg", "is", "im", "je", "li",
          "mk", "md", "mc", "me", "no", "sm", "rs", "sj", "ch", "ua", "va"};  
    }
    
    public string Code
    {
      get { return "eu"; }
    }

    public bool IsValidForCountryCode(string countryCode)
    {
      return _europeanCountries.Contains(countryCode);
    }
  }
}
