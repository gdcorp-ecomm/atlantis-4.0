using System.Data;
using System.Diagnostics;
using Atlantis.Framework.MyaAccordionMetaData.Interface;
using Atlantis.Framework.MyaAccountList.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaAccountList.Tests
{
  [TestClass]
  public class MyaAccountListTest
  {
    private const string shopperId = "840748";
    public AccordionMetaData AMD
    {
      //3=Hosting, 1=Domain, 4=Email, 19=DBP, 21=EEM, 7=SEV
      get { return GetAccordionMetaData(21); }
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
      MyaAccordionMetaDataRequestData.MinimumAccordionMetaDataCount = 30;

      MyaAccordionMetaDataRequestData request = new MyaAccordionMetaDataRequestData("856907"
        , string.Empty
        , string.Empty
        , string.Empty
        , 0);

      MyaAccordionMetaDataResponseData response = (MyaAccordionMetaDataResponseData)Engine.Engine.ProcessRequest(request, 428);

      return response.GetAccordionById(accordionId);
    }
    #endregion
  }
}