using System.Diagnostics;
using Atlantis.Framework.HCCGetServiceAgreement.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.HCCGetServiceAgreement.Tests
{


  [TestClass()]
  public class HCCGetServiceAgreementTest
  {


    private TestContext testContextInstance;

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
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    [TestMethod()]
    [DeploymentItem("atlantis.config")]
    public void GetServiceAgreement()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);
      string shopperId = "12530";
      string sourceUrl = string.Empty; // TODO: Initialize to an appropriate value
      string orderIo = string.Empty; // TODO: Initialize to an appropriate value
      string pathway = string.Empty; // TODO: Initialize to an appropriate value
      int pageCount = 0; // TODO: Initialize to an appropriate value
      string accountUid = "e4ca712d-1e92-4aeb-a182-6439464ef879";

      HCCGetServiceAgreementRequestData request = new HCCGetServiceAgreementRequestData(shopperId, sourceUrl, orderIo, pathway, pageCount, accountUid);

     // HCCGetServiceAgreementResponseData response = Engine.Engine.ProcessRequest(request, 262) as HCCGetServiceAgreementResponseData;
      // Cache call
      var response = SessionCache.SessionCache.GetProcessRequest<HCCGetServiceAgreementResponseData>(request, 262);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess && !response.ToXML().Contains("<status>Failed</status>"), response.ToXML());
      
    }
  }
}
