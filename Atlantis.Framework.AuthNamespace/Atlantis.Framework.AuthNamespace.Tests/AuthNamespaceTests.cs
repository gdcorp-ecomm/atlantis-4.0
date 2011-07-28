using System;

using Atlantis.Framework.AuthNamespace.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.AuthNamespace.Tests
{
  /// <summary>
  /// Summary description for AuthNamespaceTests
  /// </summary>
  [TestClass]
  public class AuthNamespaceTests
  {
    // shopper in DEV used by these tests
    // private const string shopperId = "856084";
    // TODO: A product needs to be set up on the account to test against
    private const string @namespace = "wst";
    private const string key = "joepassword.me";

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
    public void NamespaceNonsecureWsInConfig()
    {
      AuthNamespaceRequestData request = new AuthNamespaceRequestData(
        String.Empty, String.Empty, String.Empty, String.Empty, 0, @namespace, key, 1 );
      AuthNamespaceResponseData response = (AuthNamespaceResponseData)Engine.Engine.ProcessRequest( request, 1210 );
    }

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    public void NamespaceRequired()
    {
      AuthNamespaceRequestData request = new AuthNamespaceRequestData(
        String.Empty, String.Empty, String.Empty, String.Empty, 0, String.Empty, String.Empty, 1 );
      AuthNamespaceResponseData response = (AuthNamespaceResponseData)Engine.Engine.ProcessRequest( request, 210 );
      Assert.IsFalse( response.IsSuccess );
      Assert.IsTrue( response.StatusCodes.Contains( AuthNamespaceStatusCodes.NamespaceRequired ) );
      Assert.IsTrue( response.StatusCodes.Contains( AuthNamespaceStatusCodes.KeyRequired ) );
    }

    [TestMethod]
    [DeploymentItem( "atlantis.config" )]
    public void NamespaceValid()
    {
      AuthNamespaceRequestData request = new AuthNamespaceRequestData(
        String.Empty, String.Empty, String.Empty, String.Empty, 0, @namespace, key, 1 );
      AuthNamespaceResponseData response = (AuthNamespaceResponseData)Engine.Engine.ProcessRequest( request, 210 );
      Assert.IsTrue( response.IsSuccess );
      Assert.IsFalse( String.IsNullOrEmpty( response.ShopperId ) );
      Assert.IsFalse( String.IsNullOrEmpty( response.Email ) );
    }
  }
}
