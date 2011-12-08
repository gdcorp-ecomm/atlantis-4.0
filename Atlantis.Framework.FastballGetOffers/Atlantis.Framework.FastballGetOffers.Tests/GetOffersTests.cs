using System;
using System.IO;
using System.Reflection;
using Atlantis.Framework.FastballGetOffers.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.FastballGetOffers.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetOffersTests
  {
    public GetOffersTests()
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

    private string LoadSampleXml(string filename)
    {
      string path = Assembly.GetExecutingAssembly().Location;
      string fullpath = Path.Combine(Path.GetDirectoryName(path), filename);
      string sampleXml = File.ReadAllText(fullpath);
      return sampleXml;
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
    public void BasicRequestXml()
    {
      FastballGetOffersRequestData request = new FastballGetOffersRequestData(
        "860427", "http://yuck.com", string.Empty, "TestPathGuid", 7, 1, 2, "dppCrossSell", null);

      Assert.IsFalse(string.IsNullOrEmpty(request.CandidateRequestXml));
      Assert.IsFalse(string.IsNullOrEmpty(request.ChannelRequestXml));

    }

    [TestMethod]
    [DeploymentItem("SampleGoodResult.xml")]
    public void BasicResponseObject()
    {
      string xmlResponse = LoadSampleXml("SampleGoodResult.xml");
      FastballGetOffersResponseData response = new FastballGetOffersResponseData(xmlResponse);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("SampleParseError.xml")]
    public void BadXmlResponseObject()
    {
      string xmlResponse = LoadSampleXml("SampleParseError.xml");
      FastballGetOffersResponseData response = new FastballGetOffersResponseData(xmlResponse);
      Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.confg")]
    public void BasicEngineCall()
    {
      Guid pathway = Guid.NewGuid();
      FastballGetOffersRequestData request = new FastballGetOffersRequestData(
        "860427", "http://yuck.com", string.Empty, string.Empty, 7, 1, 2, "dppCrossSell", null);

      FastballGetOffersResponseData response = (FastballGetOffersResponseData)Engine.Engine.ProcessRequest(request, 230);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.confg")]
    public void SessionCacheEngineCall()
    {
      //SessionCache.ContextWrapper.UseMockContext = true;

      Guid pathway = Guid.NewGuid();
      FastballGetOffersRequestData request = new FastballGetOffersRequestData(
        "860427", "http://yuck.com", string.Empty, string.Empty, 7, 1, 2, "dppCrossSell", null);

      FastballGetOffersResponseData response = SessionCache.SessionCache.GetProcessRequest<FastballGetOffersResponseData>(request, 230);
      Assert.IsTrue(response.IsSuccess);

      FastballGetOffersResponseData response2 = SessionCache.SessionCache.GetProcessRequest<FastballGetOffersResponseData>(request, 230);
      Assert.IsTrue(response2.IsSuccess);

      Assert.AreEqual(response.ToXML(), response2.ToXML());

    }

    [TestMethod]
    [DeploymentItem("atlantis.confg")]
    public void BannerEngineCall()
    {
      Guid pathway = Guid.NewGuid();
      FastballGetOffersRequestData request = new FastballGetOffersRequestData(
        "860427", "", string.Empty, string.Empty, 7, 1, 39, "iPhone3CrossSellBanner", null);

      FastballGetOffersResponseData response = (FastballGetOffersResponseData)Engine.Engine.ProcessRequest(request, 230);
      Assert.IsTrue(response.IsSuccess);
    }


  }
}
