
namespace Atlantis.Framework.AuctionRecommendations.Interface
{
  public class AuctionDomainToCheck
  {
    private string domainName = string.Empty;
    public string DomainName
    {
      get { return domainName; }
    }

    private string tldIds = string.Empty;
    public string TldIds
    {
      get { return tldIds; }
    }

    private string minAuctionEndTime = string.Empty;
    public string MinAuctionEndTime
    {
      get { return minAuctionEndTime; }
    }

    public AuctionDomainToCheck(string domain)
    {
      domainName = domain;
    }
    public AuctionDomainToCheck(string domain, string tlds)
    {
      domainName = domain;
      tldIds = tlds;
    }
  }
}
