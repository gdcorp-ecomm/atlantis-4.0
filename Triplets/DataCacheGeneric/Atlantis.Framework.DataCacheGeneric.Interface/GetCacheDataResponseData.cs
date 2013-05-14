using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DataCacheGeneric.Interface
{
  public class GetCacheDataResponseData : IResponseData
  {
    string _cacheDataXml;

    public static GetCacheDataResponseData FromCacheDataXml(string cacheDataXml)
    {
      return new GetCacheDataResponseData(cacheDataXml);
    }

    private GetCacheDataResponseData(string cacheDataXml)
    {
      _cacheDataXml = cacheDataXml;
    }

    public string ToXML()
    {
      return _cacheDataXml;
    }

    public AtlantisException GetException()
    {
      return null;
    }
  }
}
