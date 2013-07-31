using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Brand.Interface
{
  public class ProductLineNameRequestData : RequestData
  {
    public ProductLineNameRequestData() { }

    public override string GetCacheMD5()
    {
      return "productLineNames";
    }

    public override string ToXML()
    {
      return "<productLineNames />";
    }
  }
}