using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Geo.Interface
{
  public class CountryRequestData : RequestData
  {
    public CountryRequestData() 
    {
    }

    public override string GetCacheMD5()
    {
      return "countries";
    }

    public override string ToXML()
    {
      return "<countries />";
    }
  }
}
