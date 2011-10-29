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
      DataCache.DataCache.GetCacheData("<AccordionData/>");
	  
      Assert.IsTrue(response.IsSuccess);
      if (response.IsSuccess)
      {
        List<string> namespaces = new List<string>();
        namespaces.Add("hosting");
        namespaces.Add("pg|4");
        namespaces.Add("campblazer");
        namespaces.Add("pg|1");
        namespaces.Add("bogus");
        namespaces.Add("domain");
        IList<AccordionMetaData> myAccordions = response.GetMyAccordions(namespaces);

        Debug.WriteLine("********************** GET MY ACCORDIONS **********************");
        Debug.WriteLine("");
        foreach (AccordionMetaData amd in myAccordions)
        {
          Debug.WriteLine(string.Format("ID: {0} | Title: {1} | SortOrder: {2}", amd.AccordionId, amd.AccordionTitle, amd.DefaultSortOrder));
        }

        Debug.WriteLine("");
        Debug.WriteLine("********************** GET ACCORDION BY ID (Email) **********************");
        AccordionMetaData emailAccordion = response.GetAccordionById(3);
        Debug.WriteLine(string.Format("ID: {0} | Title: {1} | CSSCoordinates: {2}", emailAccordion.AccordionId, emailAccordion.AccordionTitle, string.Format("{0},{1},{2},{3}", emailAccordion.IconnCssCoordinates.X, emailAccordion.IconnCssCoordinates.Y, emailAccordion.IconnCssCoordinates.Width, emailAccordion.IconnCssCoordinates.Height)));
        Debug.WriteLine("");
        Debug.WriteLine("********************** ToXML() **********************");
        Debug.WriteLine(response.ToXML());
      }
    }
  }
}
