using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.AuthToken.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthToken.Tests
{
  /// <summary>
  /// Summary description for AuthTokenTests
  /// </summary>
  [TestClass]
  public class AuthTokenTests
  {
    // shopper in DEV used by these tests (made specifically for them)
    private const string shopperId = "856084";

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
    public void AuthTokenNonsecureWsInConfig()
    {
      AuthTokenRequestData request = new AuthTokenRequestData(
        shopperId, String.Empty, String.Empty, String.Empty, 0, 1 );
      AuthTokenResponseData response = (AuthTokenResponseData)Engine.Engine.ProcessRequest( request, 1208 );
    }

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    public void AuthTokenRequired()
    {
      AuthTokenRequestData request = new AuthTokenRequestData(
        String.Empty, String.Empty, String.Empty, String.Empty, 0, 1 );
      AuthTokenResponseData response = (AuthTokenResponseData)Engine.Engine.ProcessRequest( request, 208 );
      Assert.IsFalse( response.IsSuccess );
      Assert.IsTrue( response.StatusCodes.Contains( AuthTokenStatusCodes.ShopperIdRequired ) );
    }

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    public void AuthTokenValid()
    {
      AuthTokenRequestData request = new AuthTokenRequestData(
        shopperId, String.Empty, String.Empty, String.Empty, 0, 1 );
      AuthTokenResponseData response = (AuthTokenResponseData)Engine.Engine.ProcessRequest( request, 208 );
      Assert.IsTrue( response.IsSuccess );
      Assert.IsFalse( String.IsNullOrEmpty( response.AuthToken ) );
    }
  }
}
