using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.MyaAccountList.Interface;
using System.Diagnostics;
using Atlantis.Framework.MyaAccordionMetaData.Interface;
using System.Xml.Linq;

namespace Atlantis.Framework.MyaAccountList.Tests
{
  [TestClass]
  public class MyaAccountListTest
  {
    private const string shopperId = "856907";
    public AccordionMetaData AMD
    {
      //1=Hosting, 2=Domain, 3=Email, 5=DBP, 10=EEM
      get { return GetAccordionMetaData(1); }
    }

    public MyaAccountListTest()
    { }

    [TestMethod]
    [DeploymentItem("app.config")]
    [DeploymentItem("atlantis.config")]
    public void GetFreeAccountsOnly()
    {

      MyaAccountListRequestData request = new MyaAccountListRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, AMD, returnFreeListOnly: 1);

      MyaAccountListResponseData response = (MyaAccountListResponseData)Engine.Engine.ProcessRequest(request, 425);

      if (response.IsSuccess)
      {
        DataSet ds = response.AccountListDataSet;
        int total = response.PageTotals.TotalRecords;
        int pages = response.PageTotals.TotalPages;

        string a = response.ToXML();
        Debug.WriteLine(string.Format("TotalRecords: {0} | TotalPages: {1}", total, pages));
        Debug.WriteLine("******************************************");
        Debug.WriteLine(a);
      }

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("app.config")]
    [DeploymentItem("atlantis.config")]
    public void GetDefault5AccountListPage1()
    {
      MyaAccountListRequestData request = new MyaAccountListRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, AMD);

      MyaAccountListResponseData response = (MyaAccountListResponseData)Engine.Engine.ProcessRequest(request, 425);

      if (response.IsSuccess)
      {
        DataSet ds = response.AccountListDataSet;
        int total = response.PageTotals.TotalRecords;
        int pages = response.PageTotals.TotalPages;

        string a = response.ToXML();
        Debug.WriteLine(string.Format("TotalRecords: {0} | TotalPages: {1}", total, pages));
        Debug.WriteLine("******************************************");
        Debug.WriteLine(a);
      }

      string xml1 = response.ToXML();
      string xml2 = response.AlsoToXML();
            
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("app.config")]
    [DeploymentItem("atlantis.config")]
    public void GetDefault5AccountListPage2()
    {
      MyaAccountListRequestData request = new MyaAccountListRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, AMD, currentPage: 2);

      MyaAccountListResponseData response = (MyaAccountListResponseData)Engine.Engine.ProcessRequest(request, 425);

      if (response.IsSuccess)
      {
        DataSet ds = response.AccountListDataSet;
        int total = response.PageTotals.TotalRecords;
        int pages = response.PageTotals.TotalPages;

        string a = response.ToXML();
        Debug.WriteLine(string.Format("TotalRecords: {0} | TotalPages: {1}", total, pages));
        Debug.WriteLine("******************************************");
        Debug.WriteLine(a);
      }

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("app.config")]
    [DeploymentItem("atlantis.config")]
    public void GetAccountsExpiringIn5Days()
    {
      MyaAccountListRequestData request = new MyaAccountListRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, AMD, daysTillExpiration: 5);

      MyaAccountListResponseData response = (MyaAccountListResponseData)Engine.Engine.ProcessRequest(request, 425);

      if (response.IsSuccess)
      {
        DataSet ds = response.AccountListDataSet;
        int total = response.PageTotals.TotalRecords;
        int pages = response.PageTotals.TotalPages;

        string a = response.ToXML();
        Debug.WriteLine(string.Format("TotalRecords: {0} | TotalPages: {1}", total, pages));
        Debug.WriteLine("******************************************");
        Debug.WriteLine(a);
      }

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("app.config")]
    [DeploymentItem("atlantis.config")]
    public void GetAllAccounts()
    {
      MyaAccountListRequestData request = new MyaAccountListRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, AMD, returnAllFlag: 1);

      MyaAccountListResponseData response = (MyaAccountListResponseData)Engine.Engine.ProcessRequest(request, 425);

      if (response.IsSuccess)
      {
        DataSet ds = response.AccountListDataSet;
        int total = response.PageTotals.TotalRecords;
        int pages = response.PageTotals.TotalPages;

        string a = response.ToXML();
        Debug.WriteLine(string.Format("TotalRecords: {0} | TotalPages: {1}", total, pages));
        Debug.WriteLine("******************************************");
        Debug.WriteLine(a);
      }

      Assert.IsTrue(response.IsSuccess);
    }

    #region Debug
    private AccordionMetaData GetAccordionMetaData(int accordionId)
    {
      XElement root = new XElement("data",
        new XAttribute("count", "5"));
      XElement accordionData = new XElement("nothing");

      switch (accordionId)
      {
        case 1:
          accordionData = new XElement("accordiondata"
            , new XAttribute("accordionId", 1)
            , new XAttribute("accordionTitle", "Web Hosting")
            , new XAttribute("accordionXml", "<accordion ciexpansion='11111' cirenewnow='22222' cisetup='33333' controlpanelrequiresaccount='false' iconcsscoordinates='-34px,-82px,14px,12px' isproductofferedfree='true' showsetupformanageronly='false' />")
            , new XAttribute("contentXml", "<content><data accountlist='mya_accountListGetHosting_sp' usercontrol='GetProductList.ascx'/></content>")
            , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='HCCURL' ci='44444' type='std'><qskey name='rID' value='%RID%'/><qskey name='pID' value='%PID%'/><qskey name='erID' value='%ERID%'/><qskey name='common_name' value='%CN%'/></linkurl></controlpanels>")
            , new XAttribute("defaultSortOrder", 3)
            , new XAttribute("namespaces", "pg|1,Hosting,hostingfree")
            , new XAttribute("workspaceLoginXml", "<workspace/>"));
          break;
        case 2:
          accordionData = new XElement("accordiondata"
            , new XAttribute("accordionId", 2)
            , new XAttribute("accordionTitle", "Domains")
            , new XAttribute("accordionXml", "<accordion ciexpansion='98765' cirenewnow='98766' cisetup='98767' controlpanelrequiresaccount='false' iconcsscoordinates='-154px,-92px,14px,12px' isproductofferedfree='false' showsetupformanageronly='false' />")
            , new XAttribute("contentXml", "<content><data accountlist='' usercontrol='domains.ascx'/></content>")
            , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='DCCURL' ci='98768' type='std'><qskey name='rID' value='%RID%'/><qskey name='pID' value'%PID%'/><qskey name='erID' value='-1'/><qskey name='common_name' value='%CN%'/></linkurl></controlpanels>")
            , new XAttribute("defaultSortOrder", 1)
            , new XAttribute("namespaces", "pg|30,domain")
            , new XAttribute("workspaceLoginXml", "<workspace/>"));
          break;
        case 3:
          accordionData = new XElement("accordiondata"
            , new XAttribute("accordionId", 3)
            , new XAttribute("accordionTitle", "Email")
            , new XAttribute("accordionXml", "<accordion ciexpansion='55555' cirenewnow='66666' cisetup='77777' controlpanelrequiresaccount='false' iconcsscoordinates='-34px,-42px,14px,12px' isproductofferedfree='true' showsetupformanageronly='false' />")
            , new XAttribute("contentXml", "<content><data accountlist='mya_accountListGetEmail_sp' usercontrol='GetProductList.ascx'/></content>")
            , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='ECCURL' ci='88888' type='std'><qskey name='rID' value='%RID%'/><qskey name='pID' value='%PID%'/><qskey name='erID' value='%ERID%'/><qskey name='common_name' value='%CN%'/></linkurl></controlpanels>")
            , new XAttribute("defaultSortOrder", 4)
            , new XAttribute("namespaces", "pg|4,email,emailfree,smtprelay,pg|16,emailfwd")
            , new XAttribute("workspaceLoginXml", "<workspace><linkurl link='SECURESERVERLOGINURL' ci='99999' type='std'><qskey name='apptag' value='wbe'/></linkurl></workspace>"));
          break;
        case 5:
          accordionData = new XElement("accordiondata"
            , new XAttribute("accordionId", 5)
            , new XAttribute("accordionTitle", "Domains By Proxy")
            , new XAttribute("accordionXml", "<accordion ciexpansion='' cirenewnow='58766' cisetup='' controlpanelrequiresaccount='false' iconcsscoordinates='-104px,-22px,14px,12px' isproductofferedfree='false' showsetupformanageronly='false' />")
            , new XAttribute("contentXml", "<content/>")
            , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='DBPURL' ci='58768' type='std'><qskey name='rID' value='%RID%'/><qskey name='pID' value'%PID%'/><qskey name='erID' value='-1'/><qskey name='common_name' value='%CN%'/></linkurl></controlpanels>")
            , new XAttribute("defaultSortOrder", 19)
            , new XAttribute("namespaces", "pg|9, dbp")
            , new XAttribute("workspaceLoginXml", "<workspace/>"));
          break;
        case 10:
          accordionData = new XElement("accordiondata"
            , new XAttribute("accordionId", 10)
            , new XAttribute("accordionTitle", "Express Email Marketing")
            , new XAttribute("accordionXml", "<accordion ciexpansion='12345' cirenewnow='12346' cisetup='' controlpanelrequiresaccount='true' iconcsscoordinates='-54px,-102px,14px,12px' isproductofferedfree='false' showsetupformanageronly='false' />")
            , new XAttribute("contentXml", "<content><data accountlist='mya_accountListGetEEM_sp' usercontrol='GetProductList.ascx'/></content>")
            , new XAttribute("controlPanelXml", "<controlpanels><linkurl link='EEMURL' ci='12348' type='std'><qskey name='rID' value='%RID%'/><qskey name='pID' value'%PID%'/><qskey name='erID' value='-1'/><qskey name='common_name' value='%CN%'/><qskey name='start' value='%START%'/><qskey name='recurring' value='%RECUR%'/><qskey name='id' value='%ID%'/><qskey name='pbid' value='%PBID%'/><qskey name='pbtype' value='%PBTYPE%'/></linkurl></controlpanels>")
            , new XAttribute("defaultSortOrder", 21)
            , new XAttribute("namespaces", "pg|21,campblazer")
            , new XAttribute("workspaceLoginXml", "<workspace/>"));
          break;
      }

      root.Add(accordionData);
      XDocument xDoc = XDocument.Parse(root.ToString());

      List<AccordionMetaData> accordions = new List<AccordionMetaData>();

      accordions = (from accordion in xDoc.Element("data").Elements()
                    select new AccordionMetaData()
                    {
                      AccordionId = Convert.ToInt32(accordion.Attribute("accordionId").Value),
                      AccordionTitle = accordion.Attribute("accordionTitle").Value,
                      AccordionXml = accordion.Attribute("accordionXml").Value,
                      ContentXml = accordion.Attribute("contentXml").Value,
                      ControlPanelXml = accordion.Attribute("controlPanelXml").Value,
                      DefaultSortOrder = Convert.ToInt32(accordion.Attribute("defaultSortOrder").Value),
                      Namespaces = Convert.ToString(accordion.Attribute("namespaces").Value).ToLowerInvariant().Replace(" ", "").Split(',').ToList<string>(),
                      WorkspaceLoginXml = accordion.Attribute("workspaceLoginXml").Value
                    }
                  ).ToList<AccordionMetaData>();

      return accordions[0];
    }
    #endregion
  }
}