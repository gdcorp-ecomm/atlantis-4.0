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
    private readonly HashSet<string> _blockedCountryCodes;

    public VerifyCountryAllowedResponseData(AtlantisException exceptionOccured)
    {
      _exception = exceptionOccured;
      _blockedCountryCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

    public VerifyCountryAllowedResponseData(HashSet<string> blockedCountryList)
    {
      _blockedCountryCodes = blockedCountryList;
    }

    public bool IsCountryAllowed(string countryCode)
    {
      return !_blockedCountryCodes.Contains(countryCode);
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

      foreach (string country in _blockedCountryCodes)
      {
        fieldsElement.Add(new XElement("Country", new XAttribute("Code", country)));
      }

      return requestElement.ToString(SaveOptions.DisableFormatting);
    }
  }
}
