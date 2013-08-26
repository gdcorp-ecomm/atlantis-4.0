using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.GetShopper.Interface;

namespace Atlantis.Framework.GetShopper.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  [DeploymentItem("Atlantis.Config")]
  [DeploymentItem("Atlantis.Framework.GetShopper.Impl.dll")]
  public class GetShopperTests
  {
    public GetShopperTests()
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
    public void GetShopperBasic()
    {
      GetShopper.Interface.GetShopperRequestData request = new Atlantis.Framework.GetShopper.Interface.GetShopperRequestData("822497", string.Empty, string.Empty, string.Empty, 0);
      request.AddField("gdshop_shopper_payment_type_id");
      request.RequestTimeout = TimeSpan.FromSeconds(1);
      GetShopper.Interface.GetShopperResponseData response = (GetShopper.Interface.GetShopperResponseData) Engine.Engine.ProcessRequest(request, 1);
    }

    [TestMethod]
    public void GetShopperCommPref()
    {
      GetShopper.Interface.GetShopperRequestData request = new Atlantis.Framework.GetShopper.Interface.GetShopperRequestData("822497", string.Empty, string.Empty, string.Empty, 0);
      request.AddCommunicationPref(200);
      request.RequestTimeout = TimeSpan.FromSeconds(1);
      GetShopper.Interface.GetShopperResponseData response = (GetShopper.Interface.GetShopperResponseData)Engine.Engine.ProcessRequest(request, 1);
      Assert.AreNotEqual(-1, response.GetCommunicationPref(2));
    }

    [TestMethod]
    public void GetShopperInterestPref()
    {
      GetShopper.Interface.GetShopperRequestData request = new Atlantis.Framework.GetShopper.Interface.GetShopperRequestData("822497", string.Empty, string.Empty, string.Empty, 0);
      request.AddInterestPref(2, 2);
      request.RequestTimeout = TimeSpan.FromSeconds(1);
      GetShopper.Interface.GetShopperResponseData response = (GetShopper.Interface.GetShopperResponseData)Engine.Engine.ProcessRequest(request, 1);
      Assert.AreNotEqual(-1, response.GetInterestPref(2, 2));
    }






    [TestMethod]
    [ExpectedException (typeof(Atlantis.Framework.Interface.AtlantisException))]
    public void GetShopperTimeout()
    {
      GetShopper.Interface.GetShopperRequestData request = new Atlantis.Framework.GetShopper.Interface.GetShopperRequestData("822497", string.Empty, string.Empty, string.Empty, 0);
      request.AddField("gdshop_shopper_payment_type_id");
      request.RequestTimeout = TimeSpan.FromMilliseconds(1);
      GetShopper.Interface.GetShopperResponseData response = (GetShopper.Interface.GetShopperResponseData)Engine.Engine.ProcessRequest(request, 1);
    }

    [TestMethod]
    public void GetShopperFirstNameTimings()
    {
      string name;
      name = GetFirstName("832652");
      name = GetFirstName("857753");
      name = GetFirstName("856595");
      name = GetFirstName("856602");
      name = GetFirstName("850398");
      name = GetFirstName("853392");
      name = GetFirstName("857744");
      name = GetFirstName("849362");
    }

    private string GetFirstName(string shopperId)
    {
      DateTime startTime = DateTime.Now;
      
      GetShopperRequestData request = new GetShopperRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, "fos", "127.0.0.1");
      request.AddField("first_name");
      request.RequestTimeout = TimeSpan.FromSeconds(1);
      GetShopperResponseData response = (GetShopperResponseData)Engine.Engine.ProcessRequest(request, 1);

      DateTime endTime = DateTime.Now;
      TimeSpan callTime = endTime.Subtract(startTime);
      Console.WriteLine(shopperId + " call = " + callTime.Ticks.ToString() + " ticks, " + callTime.TotalMilliseconds + " ms.");

      return response.GetField("first_name");

    }

    [TestMethod]
    public void NoDuplicateFields()
    {
      GetShopperRequestData request = new GetShopperRequestData(
        "832652", string.Empty, string.Empty, string.Empty, 0);
      request.AddField("first_name");
      request.AddField("first_name");
      request.AddField("firsT_name");
      request.AddField("last_name");

      string requestXml = request.ToXML().ToLowerInvariant();
      int first = requestXml.IndexOf("first_name");
      int last = requestXml.LastIndexOf("first_name");
      Assert.AreEqual(first, last);
    }

  }
}
