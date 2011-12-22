using System.Diagnostics;
using Atlantis.Framework.MYAWstQuoteStatusByErid.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MYAWstQuoteStatusByErid.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetMYAWstQuoteStatusByEridTests
  {

    private const string _shopperId = "856907";
    private const int _wstQuoteRequestType = 137;


    public GetMYAWstQuoteStatusByEridTests()
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
    public void MYAWstQuoteStatusByEridTest()
    {
      const string erid = "e48ad4b0-ce24-48b7-8a62-d471abefbd79";

      MYAWstQuoteStatusByEridRequestData request = new MYAWstQuoteStatusByEridRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , erid);

      MYAWstQuoteStatusByEridResponseData response = (MYAWstQuoteStatusByEridResponseData)Engine.Engine.ProcessRequest(request, _wstQuoteRequestType);

      Debug.WriteLine("*************************************");
      Debug.WriteLine(string.Format("QuoteResponseID: {0}", response.QuoteResponse.Id));
      Debug.WriteLine(string.Format("QuoteResponseStatusId: {0}", response.QuoteResponse.StatusId));
      Debug.WriteLine(string.Format("QuoteResponseStatusDescription: {0}", response.QuoteResponse.StatusDescription));
      Debug.WriteLine(string.Format("QuoteResponseIsSuccess: {0}", response.QuoteResponse.IsSuccess));
      Debug.WriteLine("*************************************");
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MYAWstQuoteStatusByEridErrorTest()
    {
      const string erid = "qe48ad4b0-ce24-48b7-8a62-d471abefbd79";

      MYAWstQuoteStatusByEridRequestData request = new MYAWstQuoteStatusByEridRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , erid);

      MYAWstQuoteStatusByEridResponseData response = (MYAWstQuoteStatusByEridResponseData)Engine.Engine.ProcessRequest(request, _wstQuoteRequestType);

      Debug.WriteLine("*************************************");
      Debug.WriteLine(string.Format("QuoteResponseError: {0}", response.QuoteResponse.Error));
      Debug.WriteLine(string.Format("QuoteResponseIsSuccess: {0}", response.QuoteResponse.IsSuccess));
      Debug.WriteLine("*************************************");
      Debug.WriteLine(response.ToXML());
      Assert.IsFalse(response.IsSuccess);
    }
  }
}
