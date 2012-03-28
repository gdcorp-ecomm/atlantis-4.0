using System.Diagnostics;
using Atlantis.Framework.AuthTwoFactorAddPhone.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Auth.Interface;

namespace Atlantis.Framework.AuthTwoFactorAddPhone.Tests
{
  [TestClass]
  public class GetAuthTwoFactorAddPhoneTests
  {
    private const string _shopperId = "856907";
    private const int _requestType = 512;
  
  
    public GetAuthTwoFactorAddPhoneTests()
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
    public void AuthTwoFactorAddPhoneTest()
    {
      AuthTwoFactorAddPhoneRequestData request = new AuthTwoFactorAddPhoneRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0 
        , "4805288819"
        , "Verizon Wireless"
        , "MyHost"
        , "127.0.0.1");

      AuthTwoFactorAddPhoneResponseData response = (AuthTwoFactorAddPhoneResponseData)Engine.Engine.ProcessRequest(request, _requestType);      
    
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.StatusCode == TwoFactorWebserviceResponseCodes.Success);
    }
  }
}
