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

    public VerifyCountryAllowedResponseData(AtlantisException exceptionOccured)
    {
      _exception = exceptionOccured;
      _countryCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

    public VerifyCountryAllowedResponseData(HashSet<string> countryList)
    {
      _countryCodes = countryList;
    }

    public bool IsCountryAllowed(string countryCode)
    {
      return _countryCodes.Contains(countryCode);
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
