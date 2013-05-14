using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DataCacheGeneric.Interface
{
  public class GetCacheDataRequestData : RequestData
  {
    private string _requestXml;

    public GetCacheDataRequestData(string genericCacheRequextXml)
    {
      _requestXml = genericCacheRequextXml;
    }

    public override string ToXML()
    {
      return _requestXml;
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(_requestXml);
    }
  }
}
