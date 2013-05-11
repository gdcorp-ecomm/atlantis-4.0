namespace Atlantis.Framework.Interface.Tests.TestObjects
{
  public class RequestDataCacheKey : RequestData
  {
    public string Key { get; private set; }
    public RequestDataCacheKey(string key)
    {
      Key = key;
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(Key);
    }
  }
}
