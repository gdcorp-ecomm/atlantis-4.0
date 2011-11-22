using System.Diagnostics;
using Atlantis.Framework.EEMGetAuthenticationGuid.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.EEMGetAuthenticationGuid.Tests
{
  [TestClass]
  public class GetEEMGetAuthenticationGuidTests
  {
    private const string _shopperId = "856907";
    private const int _requestType = 456;


    public GetEEMGetAuthenticationGuidTests()
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
    public void EEMGetAuthenticationGuidTest()
    {
      EEMGetAuthenticationGuidRequestData request = new EEMGetAuthenticationGuidRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , 1181);

      EEMGetAuthenticationGuidResponseData response = (EEMGetAuthenticationGuidResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
