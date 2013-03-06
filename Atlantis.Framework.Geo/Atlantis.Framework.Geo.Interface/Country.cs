using System.Xml.Linq;

namespace Atlantis.Framework.Geo.Interface
{
  public class Country
  {
    public int Id { get; private set; }
    public string Code { get; private set; }
    public string Name { get; private set; }
    public string CallingCode { get; private set; }

    private Country()
    { }

    public static Country FromCacheElement(XElement countryElement)
    {
      Country result = new Country();

      result.Id = countryElement.GetAttributeValueInt("id", 0);
      result.Code = countryElement.GetAttributeValue("code", string.Empty);
      result.Name = countryElement.GetAttributeValue("name", string.Empty);
      result.CallingCode = countryElement.GetAttributeValue("callingcode", string.Empty);

      return result;
    }
  }
}
