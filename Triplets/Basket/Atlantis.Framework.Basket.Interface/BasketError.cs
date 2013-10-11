using System.Xml.Linq;

namespace Atlantis.Framework.Basket.Interface
{
  public class BasketError
  {
    // <ERROR><NUMBER>-1073468912</NUMBER><DESC> The specified shopper is not valid.</DESC><SOURCE>WscgdBasketService::CWscgdBasketService::AddItem</SOURCE><LEVEL>ERROR</LEVEL></ERROR>
    internal static BasketError FromErrorXml(XElement errorElement)
    {
      var result = new BasketError();

      if (errorElement != null)
      {
        result.Number = errorElement.ChildElementValue("NUMBER");
        result.Description = errorElement.ChildElementValue("DESC");
        result.Source = errorElement.ChildElementValue("SOURCE");
        result.Level = errorElement.ChildElementValue("LEVEL");
      }

      return result;
    }

    public string Number {get; internal set;}
    public string Description { get; internal set; }
    public string Source { get; internal set; }
    public string Level { get; internal set; }

    internal BasketError()
    {
      Number = string.Empty;
      Description = string.Empty;
      Source = string.Empty;
      Level = string.Empty;
    }
  }
}
