using Atlantis.Framework.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.Geo.Interface
{
  public class IPCountryLookupResponseData : IResponseData
  {
    const string _notFoundCode = "--";
    public static IPCountryLookupResponseData NotFound { get; private set; }

    static IPCountryLookupResponseData()
    {
      NotFound = new IPCountryLookupResponseData(_notFoundCode);
    }

    string _countryCode;

    public static IPCountryLookupResponseData FromCountry(string countryCode)
    {
      if (string.IsNullOrEmpty(countryCode))
      {
        return NotFound;
      }

      return new IPCountryLookupResponseData(countryCode);
    }

    private IPCountryLookupResponseData(string countryCode)
    {
      _countryCode = countryCode.ToLowerInvariant();
    }

    public bool CountryFound
    {
      get
      {
        return (_countryCode != _notFoundCode);
      }
    }

    public string CountryCode
    {
      get
      {
        return _countryCode;
      }
    }

    public string ToXML()
    {
      XElement lookupResponse = new XElement("IPCountryLookupResponseData");
      lookupResponse.Add(new XAttribute("countryCode", _countryCode));
      return lookupResponse.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
