using System.Collections.Generic;
using System.Xml.Linq;

namespace Atlantis.Framework.Geo.Interface
{
  public class CountryNamesResponseData : LanguageNamesResponseData
  {
    public static CountryNamesResponseData Empty { get; private set; }

    static CountryNamesResponseData()
    {
      Empty = new CountryNamesResponseData();
    }

    public static CountryNamesResponseData FromServiceData(string serviceDataXml)
    {
      var countryNamesById = new Dictionary<int, string>();

      // <LocaleData lang="pt-br"><Item id="226" country="Estados Unidos"/>etc.
      var data = XElement.Parse(serviceDataXml);
      var countryElements = data.Descendants("Item");

      foreach (var countryElement in countryElements)
      {
        var countryId = countryElement.GetAttributeValueInt("id", -1);
        var name = countryElement.GetAttributeValue("country", null);

        if ((countryId != -1) && (name != null))
        {
          countryNamesById[countryId] = name;
        }
      }

      return new CountryNamesResponseData(countryNamesById);
    }

    private CountryNamesResponseData()
    {
    }

    private CountryNamesResponseData(Dictionary<int, string> namesByCountryId)
      : base(namesByCountryId)
    {
    }
  }
}
