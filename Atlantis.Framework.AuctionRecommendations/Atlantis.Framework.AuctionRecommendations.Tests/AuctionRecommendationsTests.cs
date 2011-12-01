using Atlantis.Framework.AuctionRecommendations.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuctionRecommendations.Tests
{
  [TestClass]
  public class AuctionRecommendationsTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void AuctionRecommendationsGet()
    {
      AuctionRecommendationsResponseData response = null;
      string domain = "topdog.com";
      AuctionDomainToCheck auctionCheck = new AuctionDomainToCheck(domain);
      AuctionRecommendationsRequestData request = new AuctionRecommendationsRequestData("850398", string.Empty, string.Empty, string.Empty, 0, auctionCheck, string.Empty, string.Empty, "whois", 3000);
      response = (AuctionRecommendationsResponseData)Engine.Engine.ProcessRequest(request, 324);

      Assert.IsTrue(response._isSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ActionRecommendationsGetShopperId()
    {
      //please contact the auctions team for a new searchbyshopperkey
      AuctionRecommendationsResponseData response = null;
      AuctionRecommendationsRequestData request = new AuctionRecommendationsRequestData("850398", "1327C016-B5D3-4eca-B70E-E8F10A15FCDB", string.Empty, string.Empty, string.Empty, 0, "sales", 3000);
      response = (AuctionRecommendationsResponseData)Engine.Engine.ProcessRequest(request, 324);

      Assert.IsTrue(response._isSuccess);
    }
  }
}