using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.AuthHint.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthHint.Tests
{
  /// <summary>
  /// Summary description for AuthHintTests
  /// </summary>
  [TestClass]
  public class AuthHintTests
  {
    // shopper in DEV used by these tests (made specifically for them)
    private const string shopperId = "856084";
    private const string username = "joepassword";
    private const string street = "123 Sesame Street";

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
    public void HintNonsecureWsInConfig()
    {
      AuthHintRequestData request = new AuthHintRequestData(
        shopperId, String.Empty, String.Empty, String.Empty, 0, username, 1, street );
      AuthHintResponseData response = (AuthHintResponseData)Engine.Engine.ProcessRequest( request, 1211 );
    }

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    public void HintRequired()
    {
      AuthHintRequestData request = new AuthHintRequestData(
        String.Empty, String.Empty, String.Empty, String.Empty, 0, String.Empty, 1, String.Empty );
      AuthHintResponseData response = (AuthHintResponseData)Engine.Engine.ProcessRequest( request, 211 );
      Assert.IsFalse( response.IsSuccess );
      Assert.IsTrue( response.StatusCodes.Contains( AuthHintStatusCodes.LoginNameRequired ) );
      Assert.IsTrue( response.StatusCodes.Contains( AuthHintStatusCodes.StreetRequired ) );
    }

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    public void HintValid()
    {
      AuthHintRequestData request = new AuthHintRequestData(
        shopperId, String.Empty, String.Empty, String.Empty, 0, username, 1, street );
      AuthHintResponseData response = (AuthHintResponseData)Engine.Engine.ProcessRequest( request, 211 );
      Assert.IsTrue( response.IsSuccess );
      Assert.IsFalse( String.IsNullOrEmpty( response.Hint ) );
    }
  }
}
