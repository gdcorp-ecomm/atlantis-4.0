
namespace Atlantis.Framework.DotTypeCache.Interface
{
  public static class DotTypeEngineRequests
  {
    private static int _getDotTypeProductIdListRequest = 279;
    public static int GetDotTypeProductIdListRequest
    {
      get { return _getDotTypeProductIdListRequest; }
      set { _getDotTypeProductIdListRequest = value; }
    }

    private static int _regGetDotTypeRegistrarRequest = 281;
    public static int RegGetDotTypeRegistrarRequest
    {
      get { return _regGetDotTypeRegistrarRequest; }
      set { _regGetDotTypeRegistrarRequest = value; }
    }
  }
}
