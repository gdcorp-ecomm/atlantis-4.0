using System.Diagnostics;
using Atlantis.Framework.PrivacyPlusBundleXML.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PrivacyPlusBundleXML.Tests
{
  [TestClass]
  public class GetPrivacyPlusBundleXMLTests
  {
    private const string _shopperId = "840820";
    private const int _requestType = 422;


    public GetPrivacyPlusBundleXMLTests()
    { }

    private TestContext testContextInstance;

    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
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
    public void PrivacyPlusBundleXMLTest()
    {
      PrivacyPlusBundleXMLRequestData request = new PrivacyPlusBundleXMLRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , 2128851
        , 1);

      PrivacyPlusBundleXMLResponseData response = (PrivacyPlusBundleXMLResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Debug.WriteLine(string.Format("Renewal UnifiedProductID: {0}", response.RenewalUnifiedProductId));
      Debug.WriteLine(string.Format("BundleID: {0}", response.BundleId));
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
