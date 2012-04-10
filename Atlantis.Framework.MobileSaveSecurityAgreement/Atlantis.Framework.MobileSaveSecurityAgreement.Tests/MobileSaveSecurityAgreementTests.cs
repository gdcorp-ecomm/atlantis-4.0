using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.MobileSaveSecurityAgreement.Interface;
using Atlantis.Framework.Engine;
using Atlantis.Framework.MobileSaveSecurityAgreement.Interface;

// MobileSaveSecurityAgreementRequestData;
namespace Atlantis.Framework.MobileSaveSecurityAgreement.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class MobileSaveSecurityAgreementTests
  {
    private const string _shopperId = "857527";
    private const int _requestType = 244;

    public MobileSaveSecurityAgreementTests()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    #region Test Context
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
    #endregion

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
    public void SaveAgreeemnt()
    {
      /*MobileSaveSecurityAgreementRequestData request = new MobileSaveSecurityAgreementRequestData(_shopperId
          , string.Empty
          , string.Empty
          , string.Empty
          , 0
          , 4
          , "1");*/
      MobileSaveSecurityAgreementRequestData request = new MobileSaveSecurityAgreementRequestData(_shopperId, 
        string.Empty, 
        string.Empty,
        string.Empty,
        0,
        AppId.Iphone,
        "kljdflkjljkjl",
        MobileSaveSecurityAgreementType.UsernameAndPassword,
        MobileApplicationType.UserPass               
        );

      MobileSaveSecurityAgreementResponseData response = (MobileSaveSecurityAgreementResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Assert.IsTrue(response.GetException() == null && response.Successful);
    }
  }
}
