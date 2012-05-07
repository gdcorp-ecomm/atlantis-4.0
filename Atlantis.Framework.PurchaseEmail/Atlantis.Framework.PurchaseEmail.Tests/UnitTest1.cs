using System;
using System.IO;
using System.Reflection;
using Atlantis.Framework.PurchaseEmail.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Security.Cryptography;
using Atlantis.Framework.DataProvider.Interface;

namespace Atlantis.Framework.PurchaseEmail.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class UnitTest1
  {
    public UnitTest1()
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
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dataprovider.xml")]
    [DeploymentItem("DomainWithNoAutoRenewViaAlipay.xml")]
    public void ProcessTestOrder()
    {
      Engine.Engine.ReloadConfig();
      //Load Test order from Test and Re-Send Purchase Email
      string orderID = "446492";
      string shopperid = "75866";

      string orderXml = GetOrderXml(orderID, shopperid, 1);
      PurchaseEmailRequestData request =
        new PurchaseEmailRequestData(shopperid, string.Empty,orderID, string.Empty, 0, orderXml, "EN");
      PurchaseEmailResponseData response =
       (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);
      System.Diagnostics.Debug.WriteLine(request.ToXML());
      System.Diagnostics.Debug.WriteLine(response.ToXML());
    }

    //private string LoadSampleOrderXml(string filename)
    //{
    //  string path = Assembly.GetExecutingAssembly().Location;
    //  string fullpath = Path.Combine(Path.GetDirectoryName(path), filename);
    //  string orderXml = File.ReadAllText(fullpath);
    //  return orderXml;
    //}


    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //[DeploymentItem("dataprovider.xml")]
    //[DeploymentItem("DomainWithNoAutoRenewViaAlipay.xml")]
    //public void PurchaseEmailAlipayNoAutoRenewalNoBackupSourceTest()
    //{
    //  Engine.Engine.ReloadConfig();
    //  string orderXml = LoadSampleOrderXml("DomainWithNoAutoRenewViaAlipay.xml");
    //  PurchaseEmailRequestData request =
    //    new PurchaseEmailRequestData("70364", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
    //  PurchaseEmailResponseData response =
    //   (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);
    //}


    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //[DeploymentItem("dataprovider.xml")]
    //[DeploymentItem("DomainWithAutoRenewViaAlipay.xml")]
    //public void PurchaseEmailAlipayAutoRenewalNoBackupSourceTest()
    //{
    //  Engine.Engine.ReloadConfig();
    //  string orderXml = LoadSampleOrderXml("DomainWithAutoRenewViaAlipay.xml");
    //  PurchaseEmailRequestData request =
    //    new PurchaseEmailRequestData("70364", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
    //  PurchaseEmailResponseData response =
    //   (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);
    //}


    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //[DeploymentItem("dataprovider.xml")]
    //[DeploymentItem("DomainWithAutoRenewViaAlipayWithVisaBackupSource.xml")]
    //public void PurchaseEmailAlipayAutoRenewalWithBackupSourceTest()
    //{
    //  Engine.Engine.ReloadConfig();
    //  string orderXml = LoadSampleOrderXml("DomainWithAutoRenewViaAlipayWithVisaBackupSource.xml");
    //  PurchaseEmailRequestData request =
    //    new PurchaseEmailRequestData("122508", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
    //  PurchaseEmailResponseData response =
    //   (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);
    //}


    //[TestMethod]
    //[DeploymentItem("GiftCard.xml")]
    //[ExpectedException(typeof(ArgumentException))]
    //public void GiftCardAdditionalInfo()
    //{
    //  Engine.Engine.ReloadConfig();
    //  string orderXml = LoadSampleOrderXml("GiftCard.xml");
    //  PurchaseEmailRequestData request =
    //    new PurchaseEmailRequestData("832652", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
    //  PurchaseEmailResponseData response =
    //   (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);
    //}

    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //[DeploymentItem("dataprovider.xml")]
    //[DeploymentItem("sampleorder1min.xml")]
    //public void PurchaseEmailBasicTest()
    //{
    //  string orderXml = LoadSampleOrderXml("sampleorder1min.xml");
    //  PurchaseEmailRequestData request =
    //    new PurchaseEmailRequestData("832652", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
    //  request.AddOption("IsDevServer", "true");

    //  PurchaseEmailResponseData response =
    //    (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

    //  Assert.IsFalse(response.IsSuccess);
    //}

    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //[DeploymentItem("dataprovider.xml")]
    //[DeploymentItem("HostingOrder.xml")]
    //public void PurchaseEmailHostingOrderTest()
    //{
    //    string orderXml = LoadSampleOrderXml("HostingOrder.xml");
    //    PurchaseEmailRequestData request =
    //      new PurchaseEmailRequestData("861796", string.Empty, string.Empty, string.Empty, 0, orderXml,"ES");
    //    request.AddOption("IsDevServer", "true");

    //    PurchaseEmailResponseData response =
    //      (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

    //    Assert.IsTrue(response.IsSuccess);
    //}

    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //[DeploymentItem("dataprovider.xml")]
    //[DeploymentItem("HostingOrder2.xml")]
    //public void PurchaseEmailHostingOrderTest2()
    //{
    //    string orderXml = LoadSampleOrderXml("HostingOrder2.xml");
    //    PurchaseEmailRequestData request =
    //      new PurchaseEmailRequestData("861796", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
    //    request.AddOption("IsDevServer", "true");

    //    PurchaseEmailResponseData response =
    //      (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

    //    Assert.IsTrue(response.IsSuccess);
    //}

    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //[DeploymentItem("dataprovider.xml")]
    //[DeploymentItem("HostingDomainOrder.xml")]
    //public void PurchaseEmailHostingDomainOrderTest()
    //{
    //    string orderXml = LoadSampleOrderXml("HostingDomainOrder.xml");
    //    PurchaseEmailRequestData request =
    //      new PurchaseEmailRequestData("861796", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
    //    request.AddOption("IsDevServer", "true");

    //    PurchaseEmailResponseData response =
    //      (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

    //    Assert.IsTrue(response.IsSuccess);
    //}

    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //[DeploymentItem("dataprovider.xml")]
    //[DeploymentItem("HostingDomainOrder.xml")]
    //public void PurchaseEmailHostingDomainNewCustomerOrderTest()
    //{
    //    string orderXml = LoadSampleOrderXml("HostingDomainOrder.xml");
    //    PurchaseEmailRequestData request =
    //      new PurchaseEmailRequestData("861796", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
    //    request.AddOption("IsDevServer", "true");
    //    request.AddOption("IsNewShopper", "true");

    //    PurchaseEmailResponseData response =
    //      (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

    //    Assert.IsTrue(response.IsSuccess);
    //}

    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //[DeploymentItem("dataprovider.xml")]
    //[DeploymentItem("WSTOrder.xml")]
    //public void PurchaseEmailWSTOrderTest()
    //{
    //    string orderXml = LoadSampleOrderXml("WSTOrder.xml");
    //    PurchaseEmailRequestData request =
    //      new PurchaseEmailRequestData("121079", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
    //    //request.AddOption("IsDevServer", "true");
    //    //request.AddOption("IsNewShopper", "true");

    //    PurchaseEmailResponseData response =
    //      (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

    //    Assert.IsTrue(response.IsSuccess);
    //}

    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //[DeploymentItem("dataprovider.xml")]
    //[DeploymentItem("DomainWithDBPOrder.xml")]
    //public void PurchaseEmailDBPOrderTest()
    //{
    //    string orderXml = LoadSampleOrderXml("DomainWithDBPOrder.xml");
    //    PurchaseEmailRequestData request =
    //      new PurchaseEmailRequestData("861796", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
    //    request.AddOption("IsDevServer", "true");
    //    //request.AddOption("IsNewShopper", "true");

    //    PurchaseEmailResponseData response =
    //      (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

    //    Assert.IsTrue(response.IsSuccess);
    //}



    //[TestMethod]
    //[DeploymentItem("atlantis.config")]
    //[DeploymentItem("dataprovider.xml")]
    //[DeploymentItem("samplebadshopperorder.xml")]
    //public void PurchaseEmailBasicTestErrorBuildingMessageRequest()
    //{
    //  string orderXml = LoadSampleOrderXml("samplebadshopperorder.xml");
    //  PurchaseEmailRequestData request =
    //    new PurchaseEmailRequestData("nothere", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
    //  request.AddOption("IsDevServer", "true");

    //  PurchaseEmailResponseData response =
    //    (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

    //  Assert.IsTrue(response.IsSuccess);
    //}


    //[TestMethod]
    //[DeploymentItem("sampleorder1min.xml")]
    //[ExpectedException(typeof(ArgumentException))]
    //public void PurchaseEmailInvalidOptionTest()
    //{
    //  string orderXml = LoadSampleOrderXml("sampleorder1min.xml");
    //  PurchaseEmailRequestData request =
    //    new PurchaseEmailRequestData("832652", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
    //  request.AddOption("CoolEmail", "true");
    //}

    
    private string GetOrderXml(string orderId, string shopperId,int privateLabelID)
    {
      string _orderXML = string.Empty;
      if (string.IsNullOrEmpty(_orderXML))
      {
        try
        {
          Dictionary<string, object> parameters = new Dictionary<string, object>(2);
          parameters["bstrOrderID"] = orderId;
          parameters["bstrShopperID"] = shopperId;
          //Cache Order XML
          CartGetOrderXML request = new CartGetOrderXML(shopperId, string.Empty, orderId, string.Empty, 0, "WsceCommerce_GetOrderXML", parameters, privateLabelID);
          request.RequestTimeout = new TimeSpan(0, 0, 4);
          DataProviderResponseData response = (DataProviderResponseData)DataCache.DataCache.GetProcessRequest(request, 35);
          object responseObject = response.GetResponseObject();

          //object responseObject = PageHelpers.ActionHelpers.DataProviderCall(parameters, "WsceCommerce_GetOrderXML", _siteContext, _shopperContext);
          //null or unknown response
          if (responseObject == null)
          {
            throw new ApplicationException("WsceCommerce webservice returned a NULL or unknown response.");
          }
          _orderXML = (string)responseObject;
        }
        catch (Exception ex)
        {
          System.Diagnostics.Debug.WriteLine(ex.ToString());

        }
      }
      return _orderXML;
    }

    private class CartGetOrderXML : Atlantis.Framework.DataProvider.Interface.DataProviderRequestData
    {
      private int _privateLabelID = 1;

      public CartGetOrderXML(string shopperID,
                                string sourceURL,
                                string orderID,
                                string pathway,
                                int pageCount,
                                string requestName,
                                Dictionary<string, object> parms,
                                int privateLabelID)
        : base(shopperID, sourceURL, orderID, pathway, pageCount, requestName, parms)
      {
        _privateLabelID = privateLabelID;
      }

      public override string GetCacheMD5()
      {
        MD5 oMD5 = new MD5CryptoServiceProvider();

        oMD5.Initialize();

        byte[] stringBytes

        = System.Text.ASCIIEncoding.ASCII.GetBytes("ACOS_ORDER_XML:" + _privateLabelID.ToString() + ":" + ShopperID + ":" + OrderID);

        byte[] md5Bytes = oMD5.ComputeHash(stringBytes);

        string sValue = BitConverter.ToString(md5Bytes, 0);

        return sValue.Replace("-", "");

      }
    }

  }


  

}
