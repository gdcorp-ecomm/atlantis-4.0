using System.Diagnostics;
using Atlantis.Framework.PayeeProfilesList.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.PayeeProfilesList.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.PayeeProfilesList.Impl.dll")]
  public class GetPayeeProfilesListTests
  {
    private const string _shopperId = "856907";
    private const int _requestType = 480;


    public GetPayeeProfilesListTests()
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
    public void PayeeProfilesListTest()
    {
      PayeeProfilesListRequestData request = new PayeeProfilesListRequestData(_shopperId);

      PayeeProfilesListResponseData response = (PayeeProfilesListResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine("Payee Profile List");
      Debug.WriteLine("");
      foreach (PayeeProfileListData ppld in response.PayeeList)
      {
        Debug.WriteLine(string.Format("CapID: {0} | FriendlyName: {1}", ppld.CapId, ppld.FriendlyName));
      }
      Debug.WriteLine("");
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
