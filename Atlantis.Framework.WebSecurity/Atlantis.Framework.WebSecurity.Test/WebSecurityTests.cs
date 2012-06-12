using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.WebSecurity
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetWebSecurityTests
  {
  
    private const string _shopperId = "";
	
	
    public GetWebSecurityTests()
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
	[DeploymentItem("App.config")]
    public void WebSecurityTest()
    {
      var pageSection = (RequestValidationPageSection)ConfigurationManager.GetSection("atlantis/security");
      var results = new StringBuilder();
      if (pageSection != null)
      {
        for (int i = 0; i < pageSection.Pages.Count; i++)
        {

          results.AppendFormat("key={0} name={1} path={2}{3}",
                            pageSection.Pages[i].Key,
                            pageSection.Pages[i].RelativePath, 
                            pageSection.Pages[i].Name,
                            Environment.NewLine);

        }
      }
	  
      Console.WriteLine(results.ToString());
      Debug.WriteLine(results.ToString());
      Assert.IsTrue(pageSection != null);
    }
  }
}
