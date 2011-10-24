using System.Collections.Generic;
using System.Diagnostics;
using Atlantis.Framework.MyaAccordionMetaData.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaAccordionMetaData.Tests
{
  [TestClass]
  public class GetMyaAccordionMetaDataTests
  {
    private const int _requestType = 428;
	
	
    public GetMyaAccordionMetaDataTests()
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
    public void MyaAccordionMetaDataTest()
    {
      MyaAccordionMetaDataRequestData request = new MyaAccordionMetaDataRequestData("856907"
        , string.Empty
        , string.Empty
        , string.Empty
        , 0 );

	    MyaAccordionMetaDataResponseData response = (MyaAccordionMetaDataResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);
	  
      Assert.IsTrue(response.IsSuccess);
      if (response.IsSuccess)
      {
        List<string> namespaces = new List<string>();
        namespaces.Add("hosting");
        namespaces.Add("pg|4");
        namespaces.Add("campblazer");
        namespaces.Add("pg|1");
        IList<AccordionMetaData> myAccordions = response.GetMyAccordionIds(namespaces);
        foreach (AccordionMetaData amd in myAccordions)
        {
          Debug.WriteLine(string.Format("ID: {0} | Title: {1} | SortOrder: {2}", amd.AccordionId, amd.AccordionTitle, amd.DefaultSortOrder));
        }
      }
    }
  }
}
