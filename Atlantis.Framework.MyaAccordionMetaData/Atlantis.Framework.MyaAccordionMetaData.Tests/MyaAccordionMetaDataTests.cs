using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Web;
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
    [DeploymentItem("C:/TFS/AF40/Atlantis.Framework.MyaAccordionMetaData/Atlantis.Framework.MyaAccordionMetaData.Interface/MetaData/Accordion.xsd")]
    [DeploymentItem("C:/TFS/AF40/Atlantis.Framework.MyaAccordionMetaData/Atlantis.Framework.MyaAccordionMetaData.Interface/MetaData/Content.xsd")]
    [DeploymentItem("C:/TFS/AF40/Atlantis.Framework.MyaAccordionMetaData/Atlantis.Framework.MyaAccordionMetaData.Interface/MetaData/ControlPanel.xsd")]
    [DeploymentItem("C:/TFS/AF40/Atlantis.Framework.MyaAccordionMetaData/Atlantis.Framework.MyaAccordionMetaData.Interface/MetaData/LinkUrl.xsd")]
    [DeploymentItem("C:/TFS/AF40/Atlantis.Framework.MyaAccordionMetaData/Atlantis.Framework.MyaAccordionMetaData.Interface/MetaData/WorkspaceLogin.xsd")]
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
        namespaces.Add("dbp");
        namespaces.Add("bogus");
        namespaces.Add("domain");
        IList<AccordionMetaData> myAccordions = response.GetMyAccordions(namespaces);

        Debug.WriteLine("********************** GET MY ACCORDIONS **********************");
        Debug.WriteLine("");
        foreach (AccordionMetaData amd in myAccordions)
        {
          Debug.WriteLine(string.Format("{0}: (ID={1}, DefaultSortOrder={2})", amd.AccordionTitle, amd.AccordionId, amd.DefaultSortOrder));
        }

        foreach (AccordionMetaData accordion in myAccordions)
        {
          Debug.WriteLine("");
          Debug.WriteLine(string.Format("********************** GET ACCORDION BY ID ({0}) **********************", accordion.AccordionTitle));
          PropertyInfo[] properties = accordion.GetType().GetProperties();
          foreach (PropertyInfo p in properties)
          {
            switch (p.Name)
            {
              case "Namespaces":
                string list = string.Empty;
                List<string> nss = p.GetValue(accordion, null) as List<string>;
                foreach (string ns in nss)
                {
                  list = list + string.Format("{0},", ns);
                }
                list.TrimEnd(',');
                Debug.WriteLine(string.Format("{0}: {1}", p.Name, list));
                break;
              case "IconCssCoordinates":
                AccordionMetaData.CssSpriteCoordinate coords = p.GetValue(accordion, null) as AccordionMetaData.CssSpriteCoordinate;
                Debug.WriteLine(string.Format("{0}: {1}", p.Name, string.Format("X:{0},Y:{1},Width:{2},Height:{3}", coords.X, coords.Y, coords.Width, coords.Height)));
                break;
              case "Content":
                AccordionMetaData.ContentData inner = p.GetValue(accordion, null) as AccordionMetaData.ContentData;
                Debug.WriteLine(string.Format("{0}: {1}", p.Name, string.Format("AccountList: {0} | JsonPage: {1}", inner.AccountList, inner.JsonPage)));
                break;
              case "WorkspaceLogin":
                AccordionMetaData.WorkspaceLoginData wsl = p.GetValue(accordion, null) as AccordionMetaData.WorkspaceLoginData;
                if (wsl.HasLink)
                {
                  StringBuilder sb = new StringBuilder();
                  Debug.WriteLine(string.Format("{0}: {1}", p.Name, string.Format("Link: {0} | Ci: {1} | Type: {2}", wsl.LinkUrl.Link, wsl.LinkUrl.CiCode, wsl.LinkUrl.Type)));
                  foreach (string key in wsl.LinkUrl.QsKeys)
                  {
                    sb.Append(string.Format("{0}: {1}", key, wsl.LinkUrl.QsKeys[key]));
                  }
                  if (sb.Length > 0)
                  {
                    Debug.WriteLine(string.Format("{0} QsKeys: {1}", p.Name, sb.ToString()));
                  }
                }
                else
                {
                  Debug.WriteLine(string.Format("{0}: N/A", p.Name));
                }
                break;
              case "ControlPanels":
                AccordionMetaData.ControlPanelData cp = p.GetValue(accordion, null) as AccordionMetaData.ControlPanelData;
                foreach(AccordionMetaData.LinkUrlData link in cp.LinkUrls)
                {
                  StringBuilder sb = new StringBuilder();
                  Debug.WriteLine(string.Format("{0}: {1}", p.Name, string.Format("Link: {0} | Ci: {1} | Type: {2}", link.Link, link.CiCode, link.Type)));
                  foreach (string key in link.QsKeys)
                  {
                    sb.Append(string.Format("{0}: {1}", key, link.QsKeys[key]));
                  }
                  if (sb.Length > 0)
                  {
                    Debug.WriteLine(string.Format("{0} QsKeys: {1}", p.Name, sb.ToString()));
                  }
                }
                Debug.WriteLine(string.Format("{0} - HasManagerLink: {1}", p.Name, cp.HasManagerLink));
                break;
              default:
                Debug.WriteLine(string.Format("{0}: {1}", p.Name, p.GetValue(accordion, null)));
                break;
            }
          }
          Debug.WriteLine(string.Format("HasProductList: {0}", accordion.HasProductList()));
          Debug.WriteLine(string.Format("ShowWorkspaceLogin: {0}", accordion.ShowWorkspaceLogin()));
        }
        Debug.WriteLine("");        
                
        Debug.WriteLine("********************** ToXML() **********************");
        Debug.WriteLine(HttpUtility.HtmlDecode(response.ToXML()));
      }
    }
  }
}
