using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Geo.Interface
{
  public class CountryRequestData : RequestData
  {
    public CountryRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount) 
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
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
