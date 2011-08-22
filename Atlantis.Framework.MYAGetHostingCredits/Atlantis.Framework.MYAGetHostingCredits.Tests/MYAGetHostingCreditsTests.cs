using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.MYAGetHostingCredits.Interface;

namespace Atlantis.Framework.MYAGetHostingCredits.Tests
{
  [TestClass]
  public class GetMYAGetHostingCreditsTests
  {

    private const string _shopperId = "842749";
    private const int _hostingCreditsRequestType = 141;

    public GetMYAGetHostingCreditsTests()
    {
    }

    public TestContext TestContext { get; set; }

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
    [DeploymentItem("bin/netconnect.dll")]
    public void MYAGetHostingCreditsTest()
    {
      var productTypeIds = new List<int> {82, 84, 132, 141};

      var request = new MYAGetHostingCreditsRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , productTypeIds);

      var response = (MYAGetHostingCreditsResponseData)Engine.Engine.ProcessRequest(request, _hostingCreditsRequestType);

      foreach (HostingCredit hc in response.HostingCredits)
      {
        Debug.WriteLine(string.Format("Id: {0}", hc.Id));
        Debug.WriteLine(string.Format("Count: {0}", hc.Count));
        Debug.WriteLine("******************************");
      }

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("bin/netconnect.dll")]
    public void MYAGetHostingCreditsSerializeTest()
    {
      var productTypeIds = new List<int> {82, 84, 132, 141};

      var request = new MYAGetHostingCreditsRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , productTypeIds);

      var response = (MYAGetHostingCreditsResponseData)Engine.Engine.ProcessRequest(request, _hostingCreditsRequestType);
      var serializedResponse = new MYAGetHostingCreditsResponseData(response.ToXML());

      Assert.AreEqual(response.HostingCredits.Count, serializedResponse.HostingCredits.Count);

      foreach (HostingCredit hc in response.HostingCredits)
      {
        foreach (HostingCredit shc in serializedResponse.HostingCredits)
        {
          if (hc.Id == shc.Id)
          {
            Assert.AreEqual(hc.Id, shc.Id);
            Assert.AreEqual(hc.Count, shc.Count);
          }
        }
      }
      Debug.WriteLine(serializedResponse.ToXML());
      Assert.IsTrue(serializedResponse.IsSuccess);
    }
  }
}
