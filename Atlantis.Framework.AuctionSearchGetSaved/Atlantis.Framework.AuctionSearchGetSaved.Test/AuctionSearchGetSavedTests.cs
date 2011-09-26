using System.Diagnostics;
using Atlantis.Framework.AuctionSearch.Interface;
using Atlantis.Framework.AuctionSearchGetSaved.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.AuctionSearchGetSaved.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetAuctionSearchGetSavedTests
  {
    [TestMethod]
	  [DeploymentItem("atlantis.config")]
    public void AuctionSearchGetSavedTest()
    {
      var requestorInformation = new RequestorInformation { ExternalIpAddress = "1.2.3.4", RequestingServerIp = "5.6.7.8", RequestingServerName = "testingserver", SourceSystemId = 26};

      var request = new AuctionSearchGetSavedRequestData(requestorInformation, "840820"
        , string.Empty
        , string.Empty
        , string.Empty
        , 0 );

      var response = (AuctionSearchGetSavedResponseData)Engine.Engine.ProcessRequest(request, 382);
	  
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
      if (response.IsSuccess)
      {
        foreach (var search in response.SavedSearches)
        {
          Debug.WriteLine("SearchName: " + search.SearchName);
        }
      }
    }
  }
}
