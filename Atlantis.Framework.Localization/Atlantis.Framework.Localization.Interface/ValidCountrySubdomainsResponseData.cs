using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Localization.Interface
{
  public class ValidCountrySubdomainsResponseData : IResponseData
  {
    public static ValidCountrySubdomainsResponseData FromDelimitedSetting(string settingValue)
    {
      HashSet<string> countrySubdomains = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

      if (!string.IsNullOrEmpty(settingValue))
      {
        string[] countryArray = settingValue.Split('|');
        foreach (string country in countryArray)
        {
          if (!string.IsNullOrEmpty(country))
          {
            countrySubdomains.Add(country);
          }
        }
      }

      return new ValidCountrySubdomainsResponseData(countrySubdomains);
    }

    private HashSet<string> _validCountrySubdomains;

    private ValidCountrySubdomainsResponseData(HashSet<string> countrySubdomains)
    {
      _validCountrySubdomains = countrySubdomains;
    }

    /// <summary>
    /// An enumerable list of the valid country subdomains. If doing single checks, please use IsValidCountrySubdomain for better performance
    /// </summary>
    public IEnumerable<string> ValidCountrySubdomains
    {
      get { return _validCountrySubdomains; }
    }

    /// <summary>
    /// Total number of valid country subdomains
    /// </summary>
    public int Count
    {
      get { return _validCountrySubdomains.Count; }
    }

    /// <summary>
    /// Provides a case-insenstive lookup against the valid country subdomains
    /// </summary>
    /// <param name="country">country code</param>
    /// <returns>true if the country code is a valid country subdomain</returns>
    public bool IsValidCountrySubdomain(string country)
    {
      return _validCountrySubdomains.Contains(country);
    }

    public string ToXML()
    {
      XElement element = new XElement("validcountrysubdomains");

      foreach (string country in _validCountrySubdomains)
      {
        element.Add(new XElement("country", new XText(country)));
      }

      return element.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
