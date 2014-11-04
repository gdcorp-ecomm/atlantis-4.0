using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Web;
using Atlantis.Framework.MyaAccordionMetaData.Interface;
using Atlantis.Framework.MyaAccordionMetaData.Interface.MetaData;
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
    [DeploymentItem("Atlantis.Framework.MyaAccordionMetaData.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
    public void MyaAccordionMetaDataTest()
    {

      var gdContextId = 1;

      MyaAccordionMetaDataRequestData.MinimumAccordionMetaDataCount = 1;

      MyaAccordionMetaDataRequestData request = new MyaAccordionMetaDataRequestData("856907"
        , string.Empty
        , string.Empty
        , string.Empty
        , 0 );

      MyaAccordionMetaDataResponseData response = (MyaAccordionMetaDataResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);


      Assert.IsTrue(response.IsSuccess);
      if (response.IsSuccess)
      {

        foreach (AccordionMetaData accordion in response.AccordionMetaDataItems)
        {
          Debug.WriteLine("");
          Debug.WriteLine(string.Format("********************** GET ACCORDION BY ID ({0}) **********************", accordion.GetAccordionTitle(gdContextId)));
          PropertyInfo[] properties = accordion.GetType().GetProperties();
          foreach (PropertyInfo p in properties)
          {
            switch (p.Name)
            {
              case "Namespaces":
                string list = string.Empty;
                HashSet<string> nss = p.GetValue(accordion, null) as HashSet<string>;
                foreach (string ns in nss)
                {
                  list = list + string.Format("{0},", ns);
                }
                list.TrimEnd(',');
                Debug.WriteLine(string.Format("{0}: {1}", p.Name, list));
                break;
              case "IconCssCoordinates":
                CssSpriteCoordinate coords = p.GetValue(accordion, null) as CssSpriteCoordinate;
                Debug.WriteLine(string.Format("{0}: {1}", p.Name, string.Format("X:{0},Y:{1},Width:{2},Height:{3}", coords.X, coords.Y, coords.Width, coords.Height)));
                break;
              case "Content":
                Debug.WriteLine(string.Format("{0}: {1}", p.Name, string.Format("AccountList: {0} | JsonPage: {1} | CiOptions: {2} | ShowOptionsMgrOnly: {3}", accordion.Content.AccountList, accordion.Content.JsonPage, accordion.Content.CiOptions, accordion.Content.ShowOptionsButtonForManagerOnly)));
                if (accordion.Content.ShowBuyLink)
                {
                  StringBuilder sb = new StringBuilder();
                  Debug.WriteLine(string.Format("{0}: {1}", p.Name, string.Format("Link: {0} | Page: {1} | Type: {2}", accordion.Content.BuyMoreLink.Link, accordion.Content.BuyMoreLink.Page, accordion.Content.BuyMoreLink.Type)));
                  foreach (string key in accordion.Content.BuyMoreLink.GetNewQsKeys())
                  {
                    sb.Append(string.Format("{0}: {1}", key, accordion.Content.BuyMoreLink.GetNewQsKeys()[key]));
                  }
                  if (sb.Length > 0)
                  {
                    Debug.WriteLine(string.Format("{0} QsKeys: {1}", p.Name, sb.ToString()));
                  }
                }
                if (accordion.Content.ShowHelpLink)
                {
                  StringBuilder sb = new StringBuilder();
                  Debug.WriteLine(string.Format("{0}: {1}", p.Name, string.Format("Link: {0} | Page: {1} | Type: {2}", accordion.Content.HelpLink.Link, accordion.Content.HelpLink.Page, accordion.Content.HelpLink.Type)));
                  foreach (string key in accordion.Content.HelpLink.GetNewQsKeys())
                  {
                    sb.Append(string.Format("{0}: {1}", key, accordion.Content.HelpLink.GetNewQsKeys()[key]));
                  }
                  if (sb.Length > 0)
                  {
                    Debug.WriteLine(string.Format("{0} QsKeys: {1}", p.Name, sb.ToString()));
                  }
                }
                break;
              case "WorkspaceLogin":								
                if (accordion.WorkspaceLogin.HasLink)
                {
                  Debug.WriteLine(string.Format("{0}-ButtonText: {1}", p.Name, accordion.WorkspaceLogin.ButtonText));
                  StringBuilder sb = new StringBuilder();
                  Debug.WriteLine(string.Format("{0}: {1}", p.Name, string.Format("Link: {0} | Page: {1} | Type: {2}", accordion.WorkspaceLogin.LinkUrl.Link, accordion.WorkspaceLogin.LinkUrl.Page, accordion.WorkspaceLogin.LinkUrl.Type)));
                  foreach (string key in accordion.WorkspaceLogin.LinkUrl.GetNewQsKeys())
                  {
                    sb.Append(string.Format("{0}: {1}", key, accordion.WorkspaceLogin.LinkUrl.GetNewQsKeys()[key]));
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
                Debug.WriteLine(string.Format("Do Any Links Contain Identification Rules: {0}", accordion.ControlPanels.DoLinkUrlsContainIdentificationRules));
                #if DEBUG
                foreach(LinkUrlData link in accordion.ControlPanels.LinkUrls)
                {
                  StringBuilder sb = new StringBuilder();
                  StringBuilder sb3 = new StringBuilder();
                  Debug.WriteLine(string.Format("{0}: {1}", p.Name, string.Format("Link: {0} | Page: {1} | Type: {2} | IdentificationRule: {3} | IdentificationValue: {4}", link.Link, link.Page, link.Type, link.IdentificationRule, link.IdentificationValue)));
                  foreach (string key in link.GetNewQsKeys())
                  {
                    sb.Append(string.Format("{0}: {1} | ", key, link.GetNewQsKeys()[key]));
                  }           
                  if (sb.Length > 0)
                  {
                    Debug.WriteLine(string.Format("{0} QsKeys: {1}", p.Name, sb.ToString().TrimEnd(' ').TrimEnd('|')));
                  }
                  bool isSecure = false;
                  for (int i = 1; i < 5; i++)
                  {
                    link.EnvironmentHttpsRequirements.TryGetValue(i, out isSecure);
                    sb3.Append(string.Format("Env #{0} : {1} | ", i, isSecure));
                  }
                  Debug.WriteLine(string.Format("{0} Environment Security: {1}", p.Name, sb3.ToString().TrimEnd(' ').TrimEnd('|')));
                }
                if (accordion.ControlPanels.LinkUrls.Exists(new System.Predicate<LinkUrlData>(url => url.IdentificationRule != string.Empty)))
                {
                  foreach (LinkUrlData link in accordion.ControlPanels.LinkUrls)
                  {
                    Debug.WriteLine(string.Format("{0} - IdentificationRule: {1} = {2} - HasManagerLink: {3}", p.Name, link.IdentificationRule, link.IdentificationValue, accordion.ControlPanels.HasManagerLink(link.IdentificationValue)));
                  }
                }
                else 
                {
                  Debug.WriteLine(string.Format("{0} - HasManagerLink: {1}", p.Name, accordion.ControlPanels.HasManagerLink(string.Empty)));
                }
                #endif
                break;
              case "AccordionTitle":
                Debug.WriteLine(string.Format("{0}: {1}", p.Name, HttpUtility.HtmlDecode(p.GetValue(accordion, null).ToString())));
                break;
              case "CmsDisplayGroups":
                StringBuilder sb2 = new StringBuilder();
                foreach (int group in accordion.CmsDisplayGroups)
                {
                  sb2.AppendFormat("{0} | ", group);
                }
                Debug.WriteLine(string.Format("{0}: {1}", p.Name, sb2.ToString().TrimEnd(' ').TrimEnd('|')));
                break;
              case "ProductMaps":
                ProductMap pmap = new ProductMap();
                accordion.ContainsProductMapForGroup(25, out pmap);
                StringBuilder sb4 = new StringBuilder();
                foreach (ProductMap pm in accordion.ProductMaps.Values)
                {
                  sb4.AppendFormat("group: {0}, types: {1}, ns: {3}, desc: {2}\n", pm.Group, string.Join("* ", pm.TypeList), pm.Description, string.Join("* ", pm.NamespaceList));
                }
                Debug.WriteLine(string.Format("{0}: {1}", p.Name, sb4.ToString()));
                break;
              case "SurveyData":
                StringBuilder sb5 = new StringBuilder();
                if (accordion.DoesProductHaveSurveys)
                {
                  foreach (SurveyData s in accordion.SurveyData.Values)
                  {
                    if (s.LinkUrl != null)
                    {
                      sb5.AppendFormat("type: {0}, linkText: {1}, productId: {2}, linkUrlLink: {3}, linkUrlPage: {4}", s.Type, s.LinkText, s.ProductId, s.LinkUrl.Link, s.LinkUrl.Page);
                    }
                    else
                    {
                      sb5.AppendFormat("type: {0}, linkText: {1}, productId: {2}", s.Type, s.LinkText, s.ProductId);
                    }
                    sb5.Append(" | ");
                  }
                }
                Debug.WriteLine(string.Format("{0}: {1}", p.Name, sb5.ToString()));
                break;
              default:
                Debug.WriteLine(string.Format("{0}: {1}", p.Name, p.GetValue(accordion, null)));
                break;
            }
          }
          Debug.WriteLine(string.Format("HasProductList: {0}", accordion.HasProductList()));
          Debug.WriteLine(string.Format("ShowWorkspaceLogin: {0}", accordion.ShowWorkspaceLogin()));
          //Debug.WriteLine(string.Format("IsAllInnerXmlValid: {0}", accordion.IsAllInnerXmlValid.HasValue ? "False" : "True"));
        }
        Debug.WriteLine("");
        Debug.WriteLine("Response.IsSuccess: {0}", response.IsSuccess);
        //Debug.WriteLine("Response.NoXmlParsingErrors: {0}", response.NoXmlParsingErrors);
        Debug.WriteLine("");
                
        Debug.WriteLine("********************** ToXML() **********************");
        Debug.WriteLine(HttpUtility.HtmlDecode(response.ToXML()));
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.MyaAccordionMetaData.Impl.dll")]
    public void CacheSoilTest()
    {
      #if DEBUG

      MyaAccordionMetaDataRequestData request = new MyaAccordionMetaDataRequestData("856907"
        , string.Empty
        , string.Empty
        , string.Empty
        , 0);

      MyaAccordionMetaDataResponseData response1 = (MyaAccordionMetaDataResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);
      AccordionMetaData amd1 = response1.GetAccordionById(3);  // Web Hosting

      NameValueCollection qsKeysCopy1 = new NameValueCollection(amd1.ControlPanels.LinkUrls[0].GetNewQsKeys());
      NameValueCollection qsKeysCopy2 = new NameValueCollection(amd1.ControlPanels.LinkUrls[0].GetNewQsKeys());
      foreach (string key in qsKeysCopy1)
      {
        switch (amd1.ControlPanels.LinkUrls[0].GetNewQsKeys()[key])
        {
          case "%AUTHGUID%":
            qsKeysCopy2[key] = "20ak0a09d022095j15g0u8";
            amd1.ControlPanels.LinkUrls[0].GetNewQsKeys()[key] = "20ak0a09d022095j15g0u8";
            break;
          case "%CN%":
            qsKeysCopy2[key] = "TESTING_1_2_3";
            amd1.ControlPanels.LinkUrls[0].GetNewQsKeys()[key] = "TESTING_1_2_3";
            break;
          case "%ERID%":
            qsKeysCopy2[key] = "1111122222333334444455555";
            amd1.ControlPanels.LinkUrls[0].GetNewQsKeys()[key] = "1111122222333334444455555";
            break;
        }
      }
      Debug.WriteLine(string.Format("Copy1: {0}, Copy2: {1}", qsKeysCopy1.GetHashCode(), qsKeysCopy2.GetHashCode()));
      Debug.WriteLine(string.Format("First Call Hosting QsKey Cache Properties: auth_guid={0} | account_uid={1} | common_name={2}", amd1.ControlPanels.LinkUrls[0].GetNewQsKeys()["auth_guid"], amd1.ControlPanels.LinkUrls[0].GetNewQsKeys()["account_uid"], amd1.ControlPanels.LinkUrls[0].GetNewQsKeys()["common_name"]));
      Debug.WriteLine(string.Format("First Call Hosting Copy Properties: auth_guid={0} | account_uid={1} | common_name={2}", qsKeysCopy2["auth_guid"], qsKeysCopy2["account_uid"], qsKeysCopy2["common_name"]));

      Assert.AreNotEqual(amd1.ControlPanels.LinkUrls[0].GetNewQsKeys()["account_uid"], qsKeysCopy2["account_uid"]);

      MyaAccordionMetaDataResponseData response2 = (MyaAccordionMetaDataResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);
      AccordionMetaData amd2 = response2.GetAccordionById(3);  // Web Hosting
      qsKeysCopy2 = new NameValueCollection(amd1.ControlPanels.LinkUrls[0].GetNewQsKeys());

      Debug.WriteLine(string.Format("Second Call Hosting QsKey Cache Properties: auth_guid={0} | account_uid={1} | common_name={2}", amd2.ControlPanels.LinkUrls[0].GetNewQsKeys()["auth_guid"], amd2.ControlPanels.LinkUrls[0].GetNewQsKeys()["account_uid"], amd2.ControlPanels.LinkUrls[0].GetNewQsKeys()["common_name"]));
      Debug.WriteLine(string.Format("Second Call Hosting Copy Properties (unset): auth_guid={0} | account_uid={1} | common_name={2}", qsKeysCopy2["auth_guid"], qsKeysCopy2["account_uid"], qsKeysCopy2["common_name"]));

      Assert.AreEqual(amd1.ControlPanels.LinkUrls[0].GetNewQsKeys()["account_uid"], amd2.ControlPanels.LinkUrls[0].GetNewQsKeys()["account_uid"]);

      #endif
    }
  }
}
