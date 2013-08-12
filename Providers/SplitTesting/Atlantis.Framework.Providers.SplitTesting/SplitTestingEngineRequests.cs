namespace Atlantis.Framework.Providers.SplitTesting
{
  public static class SplitTestingEngineRequests
  {
    static SplitTestingEngineRequests()
    {
      ActiveSplitTests = 684;
      ActiveSplitTestDetails = 685;
    }

    public static int ActiveSplitTests { get; set; }
    public static int ActiveSplitTestDetails { get; set; }
  }
}
