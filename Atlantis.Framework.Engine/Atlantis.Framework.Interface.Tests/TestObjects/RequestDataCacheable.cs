namespace Atlantis.Framework.Interface.Tests.TestObjects
{
  public class RequestDataCacheable : RequestData
  {
    public override string GetCacheMD5()
    {
      return "CACHEME";
    }
  }
}
