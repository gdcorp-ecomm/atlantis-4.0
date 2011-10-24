using System;
using Atlantis.Framework.DomainsBotDatabase.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DomainsBotDatabase.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class DomiansBotDatabaseTests
  {
    public DomiansBotDatabaseTests()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DomainsBotAuctionTest()
    {
      DomainsBotDatabaseRequestData oRequestData = new DomainsBotDatabaseRequestData("77311",
        "http://www.godaddy.com/domains/actions/DoDomainSearch.aspx?ci=14647",
        String.Empty, "79392f48-ddc2-4b28-bd06-2d8fd480c637", 14, "VehicleAr", "com", 1);
      oRequestData.DatabaseToUse = "godaddycloseouts";
      oRequestData.RemoveKeys = true;
      oRequestData.RequestTimeout = TimeSpan.FromMilliseconds(5000);
      DomainsBotDatabaseResponseData response
        = (DomainsBotDatabaseResponseData)Engine.Engine.ProcessRequest(oRequestData, 45);
      Assert.IsTrue(response.NoErrors);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DomainsBotDatabaseTest()
    {
      DomainsBotDatabaseRequestData oRequestData = new DomainsBotDatabaseRequestData("77311",
        "http://www.godaddy.com/domains/actions/DoDomainSearch.aspx?ci=14647",
        String.Empty, "79392f48-ddc2-4b28-bd06-2d8fd480c637", 14, "VehicleAr", "co", 5);
      oRequestData.DatabaseToUse = "godaddypremiumtest";
      oRequestData.RemoveKeys = true;
      oRequestData.RequestTimeout = TimeSpan.FromMilliseconds(5000);
      string xml = oRequestData.ToXML();
      DomainsBotDatabaseResponseData response
        = (DomainsBotDatabaseResponseData)Engine.Engine.ProcessRequest(oRequestData, 45);
      Assert.IsTrue(response.NoErrors);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void NameAdministrationInventoryTest()
    {
      DomainsBotDatabaseRequestData oRequestData = new DomainsBotDatabaseRequestData("77311",
        "http://www.godaddy.com/domains/actions/DoDomainSearch.aspx?ci=14647",
        String.Empty, "79392f48-ddc2-4b28-bd06-2d8fd480c637", 14, "BostonTaxis", "com", 1);
      oRequestData.DatabaseToUse = "godaddynai";
      oRequestData.RemoveKeys = true;
      oRequestData.RequestTimeout = TimeSpan.FromMilliseconds(5000);
      DomainsBotDatabaseResponseData response
        = (DomainsBotDatabaseResponseData)Engine.Engine.ProcessRequest(oRequestData, 45);
      Assert.IsTrue(response.NoErrors);
    }
  }

  /* *
   http://xml.domainsbot.com/XMLServices/firstimpact3.asmx/SearchDatabaseDomains?database=godaddynai&key=bostontaxis&tlds=com,net,org,biz,info,us,tv,mobi,co,me,cc,ws&limit=100&removekeys=false&filters=&orderby=&supportedlanguages=en
   http://xml.domainsbot.com/XMLServices/firstimpact3.asmx/SearchDatabaseDomains?database=godaddynai,godaddycloseouts,godaddypremium&key=yodaspeak&tlds=com,net,org,biz,info,us,tv,mobi,co,me,cc,ws&limit=100&removekeys=false&filters=&orderby=&supportedlanguages=en
  * */
}
