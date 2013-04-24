using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Notices.Interface
{
  public class MaintenanceNoticeRequestData : RequestData
  {
    public string WebSite { get; private set; }
    public MaintenanceNoticeRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount, string webSite)
      :base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      WebSite = webSite ?? string.Empty;
    }

    public override string GetCacheMD5()
    {
      return WebSite.ToUpperInvariant();
    }
  }
}
