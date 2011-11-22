
namespace Atlantis.Framework.SEVGetWebsiteId.Interface
{
  public class SEVReplacementData
  {
    public string UserWebsiteId { get; private set; }
    public string WebsiteUrl { get; private set; }

    public SEVReplacementData(string userWebsiteId, string websiteUrl)
    {
      UserWebsiteId = userWebsiteId;
      WebsiteUrl = websiteUrl;
    }
  }
}
