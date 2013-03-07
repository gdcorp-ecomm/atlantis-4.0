namespace Atlantis.Framework.TLDDataCache.Interface
{
  public static class TLDDataCacheEngineRequests
  {
    private static int _extendedTLDData = 668;
    public static int ExtendedTLDData
    {
      get { return _extendedTLDData; }
      set { _extendedTLDData = value; }
    }
  }
}
