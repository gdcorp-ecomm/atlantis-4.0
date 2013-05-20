using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Geo.Interface
{
  public class CountryResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private List<Country> _countryList;
    private Dictionary<string, Country> _countriesByName;
    private Dictionary<string, Country> _countriesByCode;
    private Dictionary<int, Country> _countriesById;

    public static CountryResponseData FromDataCacheXml(string countriesXml)
    {
      XElement countries = XElement.Parse(countriesXml);

      List<Country> countryList = new List<Country>();
      var countryElements = countries.Descendants("country");
      foreach (var countryElement in countryElements)
      {
        Country country = Country.FromCacheElement(countryElement);
        countryList.Add(country);
      }

      return new CountryResponseData(countryList);
    }

    public static CountryResponseData FromException(AtlantisException exception)
    {
      return new CountryResponseData(exception);
    }

    private CountryResponseData(List<Country> countries)
    {
      _countryList = countries;

      _countriesByName = new Dictionary<string, Country>(_countryList.Count, StringComparer.OrdinalIgnoreCase);
      _countriesByCode = new Dictionary<string, Country>(_countryList.Count, StringComparer.OrdinalIgnoreCase);
      _countriesById = new Dictionary<int, Country>(_countryList.Count);

      foreach (Country country in _countryList)
      {
        _countriesByName[country.Name] = country;
        _countriesByCode[country.Code] = country;
        _countriesById[country.Id] = country;
      }
    }

    private CountryResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public IEnumerable<Country> Countries
    {
      get { return _countryList; }
    }

    public Country FindCountryByName(string name)
    {
      Country result = null;
      if (_countriesByName.ContainsKey(name))
      {
        result = _countriesByName[name];
      }
      return result;
    }

    public Country FindCountryByCode(string code)
    {
      Country result = null;
      if (_countriesByCode.ContainsKey(code))
      {
        result = _countriesByCode[code];
      }
      return result;
    }

    public Country FindCountryById(int id)
    {
      Country result = null;
      if (_countriesById.ContainsKey(id))
      {
        result = _countriesById[id];
      }
      return result;
    }

    public string ToXML()
    {
      XElement countries = new XElement("countries");

      if (_countryList != null)
      {
        foreach (Country country in _countryList)
        {
          XElement countryElement = new XElement("country");
          countryElement.Add(
            new XAttribute("id", country.Id.ToString()),
            new XAttribute("code", country.Code),
            new XAttribute("name", country.Name));
          countries.Add(countryElement);
        }
      }

      return countries.ToString(SaveOptions.DisableFormatting);
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}
