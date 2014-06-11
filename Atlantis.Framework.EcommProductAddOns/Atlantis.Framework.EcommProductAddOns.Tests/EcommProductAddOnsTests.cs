using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.EcommProductAddOns.Interface;

namespace Atlantis.Framework.EcommProductAddOns.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("atlantis.framework.privatelabel.impl.dll")]
  [DeploymentItem("atlantis.framework.datacachegeneric.impl.dll")]
  [DeploymentItem("atlantis.framework.ecommproductaddons.impl.dll")]
  public class GetEcommProductAddOnsTests
  {
    private const string SHOPPER_ID = "856907";
    private const int REQUEST_TYPE = 652;
	
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
    public void EcommProductNoAddOnsTest()
    {
      var request = new EcommProductAddOnsRequestData(SHOPPER_ID
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , 1
        , "a97bbfcd-48e3-11df-b65b-005056956427"
        , "hosting");

      var response = (EcommProductAddOnsResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE);      
	  
      Debug.WriteLine(response.ToXML());
      Assert.IsFalse(response.HasAddOns);
    }

    [TestMethod]
    public void EcommProductHasAddOnsTest()
    {
      var request = new EcommProductAddOnsRequestData("83439"
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , 1
        , "9cf56fc3-14f3-11e3-9fc8-005056953ce3"
        , "outlook");

      var response = (EcommProductAddOnsResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.HasAddOns);
    }

    [TestMethod]
    public void EcommProductOverrideIdTypeTest()
    {
      var request = new EcommProductAddOnsRequestData(SHOPPER_ID
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , 1
        , "d423f0f3-0d15-11df-a185-005056956427"
        , "dhs"
        , "Bonsai");

      var response = (EcommProductAddOnsResponseData)Engine.Engine.ProcessRequest(request, REQUEST_TYPE);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.HasAddOns);
    }
  }
}
