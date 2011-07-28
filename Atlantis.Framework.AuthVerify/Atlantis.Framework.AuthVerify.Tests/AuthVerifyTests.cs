using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.AuthVerify.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthVerify.Tests
{
  /// <summary>
  /// Summary description for AuthVerifyTests
  /// </summary>
  [TestClass]
  public class AuthVerifyTests
  {
    // shopper in DEV used by these tests (made specifically for them)
    private const string shopperId = "856084";
    private const string username = "joepassword";
    private const string pw = "password";
    private const string ipAddress = "0.0.0.0";

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
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
    [DeploymentItem( "atlantis.config" )]
    [ExpectedException( typeof( AtlantisException ) )]
    public void VerifyNonsecureWsInConfig()
    {
      AuthVerifyRequestData request = new AuthVerifyRequestData(
        shopperId, String.Empty, String.Empty, String.Empty, 0, username, pw, 1, ipAddress );
      AuthVerifyResponseData response = (AuthVerifyResponseData)Engine.Engine.ProcessRequest( request, 1209 );
    }

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    public void VerifyRequired()
    {
      AuthVerifyRequestData request = new AuthVerifyRequestData(
        String.Empty, String.Empty, String.Empty, String.Empty, 0, String.Empty, String.Empty, 1, String.Empty );
      AuthVerifyResponseData response = (AuthVerifyResponseData)Engine.Engine.ProcessRequest( request, 209 );
      Assert.IsFalse( response.IsSuccess );
      Assert.IsTrue( response.StatusCodes.Contains( AuthVerifyStatusCodes.LoginNameRequired ) );
      Assert.IsTrue( response.StatusCodes.Contains( AuthVerifyStatusCodes.IpAddressRequired ) );
      Assert.IsTrue( response.StatusCodes.Contains( AuthVerifyStatusCodes.PasswordRequired ) );
    }

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    public void VerifyValid()
    {
      AuthVerifyRequestData request = new AuthVerifyRequestData(
        shopperId, String.Empty, String.Empty, String.Empty, 0, username, pw, 1, ipAddress );
      AuthVerifyResponseData response = (AuthVerifyResponseData)Engine.Engine.ProcessRequest( request, 209 );
      Assert.IsTrue( response.IsSuccess );
    }
  }
}
