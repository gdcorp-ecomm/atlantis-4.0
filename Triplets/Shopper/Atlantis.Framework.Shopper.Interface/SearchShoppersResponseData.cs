using Atlantis.Framework.Shopper.Interface.BaseClasses;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace Atlantis.Framework.Shopper.Interface
{
  public class SearchShoppersResponseData : ShopperResponseData
  {
    readonly List<Dictionary<string, string>> _shoppersFound = new List<Dictionary<string, string>>();

    public static SearchShoppersResponseData FromShopperSearchXml(string shopperSearchXml)
    {
      var searchElement = XElement.Parse(shopperSearchXml);
      ShopperResponseStatus status = ShopperResponseStatus.FromResponseElement(searchElement);

      var result = new SearchShoppersResponseData(status);

      if (result.Status.Status != ShopperResponseStatusType.Success)
      {
        return result;
      }

      var shopperElements = searchElement.Descendants("Shopper");
      foreach (var shopperElement in shopperElements)
      {
        var shopperDictionary = new Dictionary<string, string>();
        foreach (var shopperAttribute in shopperElement.Attributes())
        {
          shopperDictionary[shopperAttribute.Name.LocalName] = shopperAttribute.Value;
        }
        result.AddFoundShopper(shopperDictionary);
      }

      return result;
    }

    private SearchShoppersResponseData(ShopperResponseStatus status) 
      : base(status)
    {
      _shoppersFound = new List<Dictionary<string, string>>(); 
    }

    private void AddFoundShopper(Dictionary<string, string> shopperDictionary)
    {
      _shoppersFound.Add(shopperDictionary);
    }

    public IEnumerable<IDictionary<string, string>> ShoppersFound
    {
      get { return _shoppersFound; }
    }

    public int Count
    {
      get { return _shoppersFound.Count; }
    }

    public override string ToXML()
    {
      var element = new XElement("SearchShoppersResponseData");
      element.Add(
        new XAttribute("count", _shoppersFound.Count.ToString(CultureInfo.InvariantCulture)),
        new XAttribute("status", Status.Status.ToString()));
      return element.ToString(SaveOptions.DisableFormatting);
    }

  }
}
