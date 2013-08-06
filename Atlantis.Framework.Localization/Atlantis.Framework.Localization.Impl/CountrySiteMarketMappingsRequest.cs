using System;
using System.Xml.Linq;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Localization.Interface;

namespace Atlantis.Framework.Localization.Impl
{
  public class CountrySiteMarketMappingsRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      //  TODO:  Change to an actual DataCache request when it is available
      
      CountrySiteMarketMappingsRequestData request = requestData as CountrySiteMarketMappingsRequestData;
      string cacheDataXml = GetResourceDataXml(request.CountrySite);
      IResponseData result = CountrySiteMarketMappingsResponseData.FromCacheDataXml(cacheDataXml);

      return result;
    }

    private string GetResourceDataXml(string countrySite)
    {
      XElement resourceXml =
        XElement.Parse(Interface.Properties.Resources.DefaultCountrySiteMarketMappings);

      XElement mappings = new XElement("data",
        from rx in resourceXml.Descendants("item")
          where countrySite.Equals(rx.Attribute("catalog_countrySite").Value, StringComparison.OrdinalIgnoreCase)
          select rx);

      XAttribute count = new XAttribute("count", mappings.Descendants("item").Count());
      mappings.Add(count);

      return mappings.ToString();
    }
    #endregion
  }
}
