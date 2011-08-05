using System.Diagnostics;
using Atlantis.Framework.MessagingShopperNote.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Atlantis.Framework.MessagingShopperNote.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetMessagingShopperNoteTests
  {

    private const string _shopperId = "83439";  // DEV: 856907  TEST: 83439
    private const int _requestType = 200;


    public GetMessagingShopperNoteTests()
    { }

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
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
    public void MessagingShopperNoteTestWithCCNumber()
    {
      MessagingShopperNoteRequestData request = new MessagingShopperNoteRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , "NewsBlazer:91205 - Entered Account and used 4111111111111111 and used 4111-1111-1111-1111 and used 41111111-1111-1111"
         , "Cust-666"
         , NoteTypes.SHOPPER_NOTE
         , 1);

      // Set some other optional data
      request.RequestingIp = "10.23.19.197";
      request.AccessRoleId = "1";
      request.NoteTypeLookupId = "1";
      request.SessionId = "1";
      request.ShortDescription = "Test Note From Kent";
      request.TaskActionTypeId = "1";

      MessagingShopperNoteResponseData response = (MessagingShopperNoteResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MessagingShopperNoteTest()
    {
      MessagingShopperNoteRequestData request = new MessagingShopperNoteRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , "NewsBlazer:91205 - Entered Account"
         , "Cust-666"
         , NoteTypes.SHOPPER_NOTE
         , 1);

      // Set some other optional data
      request.RequestingIp = "10.23.19.197";
      request.AccessRoleId = "1";
      request.NoteTypeLookupId = "1";
      request.SessionId = "1";
      request.ShortDescription = "Test Note From Kent";
      request.TaskActionTypeId = "1";



      MessagingShopperNoteResponseData response = (MessagingShopperNoteResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
