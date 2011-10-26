using System;
using Atlantis.Framework.DomainBundleId.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.DomainBundleId.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetDomainBundleIdTests
  {
  
    private const string _shopperId = "840820";
	
	
    public GetDomainBundleIdTests()
    {
      //
      // TODO: Add constructor logic here
      //
    }

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
    [DeploymentItem("app.config")]
    public void DomainBundleIdTest()
    {
      MockHttpContext.SetMockHttpContext("default.aspx", "http://localhost/default.aspx", string.Empty);

      DomainBundleIdRequestData request = new DomainBundleIdRequestData(_shopperId
                                                                        , string.Empty
                                                                        , string.Empty
                                                                        , string.Empty
                                                                        , 0
                                                                        , 2128851);

      DomainBundleIdResponseData response = (DomainBundleIdResponseData)Engine.Engine.ProcessRequest(request, 423);


      if (response != null)
      {
        Console.WriteLine("***************************");
        Console.WriteLine("HasBundleId: " + response.HasBundleId);
        Console.WriteLine(string.Format("BundleId: {0}", response.BundleId != null ? response.BundleId.Value.ToString() : string.Empty));
        Console.WriteLine(string.Format("ProductId: {0}", response.ProductId != null ? response.ProductId.Value.ToString() : string.Empty));
        Console.WriteLine("***************************");
        Console.WriteLine("***************************");
        Console.WriteLine(response.ToXML());
        Assert.IsTrue(response.IsSuccess);
      }
    }
  }
}
