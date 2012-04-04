﻿using System.Diagnostics;
using Atlantis.Framework.AuthTwoFactorDisable.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Auth.Interface;

namespace Atlantis.Framework.AuthTwoFactorDisable.Tests
{
  [TestClass]
  public class GetAuthTwoFactorDisableTests
  {
    private const string _shopperId = "Disable_Your_Own_$hit";
    private const int _requestType = 511;


    public GetAuthTwoFactorDisableTests()
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
    public void AuthTwoFactorDisableTest()
    {
      AuthTwoFactorDisableRequestData request = new AuthTwoFactorDisableRequestData(_shopperId
                                                                                    , string.Empty
                                                                                    , string.Empty
                                                                                    , string.Empty
                                                                                    , 0
                                                                                    , "password"
                                                                                    , "token"
                                                                                    , 1
                                                                                    , "1"
                                                                                    , "4805058800"
                                                                                    , "MeHost"
                                                                                    , "127.0.0.1");

      AuthTwoFactorDisableResponseData response = (AuthTwoFactorDisableResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.StatusCode == TwoFactorWebserviceResponseCodes.Success);
    }
  }
}
