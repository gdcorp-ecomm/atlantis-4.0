using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Atlantis.Framework.Shopper.Interface
{
  public class VerifyCountryAllowedResponseData : IResponseData
  {
    AtlantisException _exception;
    private readonly HashSet<string> _countryCodes;
    private HashSet<string> CountryCodes
    {
      get
      {
        return _countryCodes;
      }
    }

    public bool IsAllowed { get; set; }

    public VerifyCountryAllowedResponseData(AtlantisException exceptionOccured)
    {
      _exception = exceptionOccured;
      _countryCodes=new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
    }

    public VerifyCountryAllowedResponseData(HashSet<string> countryList,string countryCode)
    {
      _countryCodes = countryList;
      IsAllowed = !_countryCodes.Contains(countryCode);
    }

    public bool IsCountryAllowed(string countryCode)
    {
      return CountryCodes.Contains(countryCode);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    public string ToXML()
    {
      XElement requestElement = new XElement("BlockedCountries");
      XElement fieldsElement = new XElement("Countries");
      requestElement.Add(fieldsElement);

      foreach (string country in _countryCodes)
      {
        fieldsElement.Add(new XElement("Country", new XAttribute("Code", country)));
      }

      return requestElement.ToString(SaveOptions.DisableFormatting);
    }
  }
}
