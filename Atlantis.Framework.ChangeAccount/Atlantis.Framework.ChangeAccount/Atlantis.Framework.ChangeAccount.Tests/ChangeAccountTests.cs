using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.ChangeAccount.Interface;

namespace Atlantis.Framework.ChangeAccount.Tests
{
  /// <summary>
  /// Summary description for ChangeAccountTests
  /// </summary>
  [TestClass]
  public class ChangeAccountTests
  {
    public ChangeAccountTests()
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
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ChangeAccountAddPrepaids()
    {
      string accountChangeXML =
        "<ClientChange ProductNamespace=\"hosting\" TreeID=\"0\" AccountID=\"375973\" ShopperID=\"857020\" />";

      string itemRequestXML =
            "<itemRequest><itemTemplate itemTrackingCode=\"mbl_mya_sharedhosting_renew\" /></itemRequest>";

      ChangeAccountRequestData requestData = new ChangeAccountRequestData("857020", "http://localhost",
        "", "", 0, "375973", "hosting", "billing", accountChangeXML, 52002, 1, itemRequestXML);

      ChangeAccountResponseData responseData = (ChangeAccountResponseData)Engine.Engine.ProcessRequest(requestData, 77);
      Assert.AreEqual(responseData.IsSuccess, true);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ChangeAccountBatchAddPrepaids()
    {
      List<ChangeAccountRequestObject> changeList = new List<ChangeAccountRequestObject>();
      string accountChangeXML =
        "<ClientChange ProductNamespace=\"hosting\" TreeID=\"0\" AccountID=\"375973\" ShopperID=\"857020\" />";

      string itemRequestXML =
            "<itemRequest><itemTemplate itemTrackingCode=\"mbl_mya_sharedhosting_renew\" /></itemRequest>";


      ChangeAccountRequestObject requestObject = new ChangeAccountRequestObject("375973", "hosting", "billing", accountChangeXML,
        52002, 1, itemRequestXML);
      changeList.Add(requestObject);

      accountChangeXML =
       "<ClientChange ProductNamespace=\"email\" TreeID=\"0\" AccountID=\"375017\" ShopperID=\"857020\" />";

      itemRequestXML =
            "<itemRequest><itemTemplate itemTrackingCode=\"mbl_mya_email_renew\" /></itemRequest>";


      requestObject = new ChangeAccountRequestObject("375017", "email", "billing", accountChangeXML,
        11866, 1, itemRequestXML);
      changeList.Add(requestObject);

      ChangeAccountRequestData requestData = new ChangeAccountRequestData("857020", "http://localhost",
      "", "", 0, changeList);
      ChangeAccountResponseData responseData = (ChangeAccountResponseData)Engine.Engine.ProcessRequest(requestData, 77);
      Assert.AreEqual(responseData.IsSuccess, true);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void InvalidChangeAccountBatchAddPrepaids()
    {
      List<ChangeAccountRequestObject> changeList = new List<ChangeAccountRequestObject>();
      string accountChangeXML =
        "<ClientChange ProductNamespace=\"hosting\" TreeID=\"0\" AccountID=\"375973\" ShopperID=\"857020\" />";

      string itemRequestXML =
            "<itemRequest><itemTemplate itemTrackingCode=\"mbl_mya_sharedhosting_renew\" /></itemRequest>";


      ChangeAccountRequestObject requestObject = new ChangeAccountRequestObject("375973", "hosting", "billing", accountChangeXML,
        52002, 1, itemRequestXML);
      changeList.Add(requestObject);

      accountChangeXML =
       "<ClientChange ProductNamespace=\"eail\" TreeID=\"0\" AccountID=\"37517\" ShopperID=\"857020\" />";

      itemRequestXML =
            "<itemRequest><itemTemplate itemTrackingCode=\"mbl_ma_email_renew\" /></itemRequest>";


      requestObject = new ChangeAccountRequestObject("37507", "emil", "blling", accountChangeXML,
        11866, 1, itemRequestXML);
      changeList.Add(requestObject);

      ChangeAccountRequestData requestData = new ChangeAccountRequestData("857020", "http://localhost",
      "", "", 0, changeList);
      ChangeAccountResponseData responseData = (ChangeAccountResponseData)Engine.Engine.ProcessRequest(requestData, 77);
      Assert.AreEqual(responseData.IsSuccess, false);
    }
  }
}
