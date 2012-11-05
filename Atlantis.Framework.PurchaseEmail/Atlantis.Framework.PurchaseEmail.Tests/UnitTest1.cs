using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Atlantis.Framework.DataProvider.Interface;
using Atlantis.Framework.MessagingProcess.Interface;
using Atlantis.Framework.PurchaseEmail.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PurchaseEmail.Tests
{
  [TestClass]
  public class UnitTest1
  {
    public TestContext TestContext { get; set; }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dataprovider.xml")]
    [DeploymentItem("Atlantis.Framework.PurchaseEmail.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.MessagingProcess.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.DataProvider.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.GetShopper.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.ProductOffer.Impl.dll")]
    [DeploymentItem("Atlantis.Framework.LinkInfo.Impl.dll")]
    public void ProcessTestOrder()
    {
      Engine.Engine.ReloadConfig();

      // DBP Misc Fee - e-mails tfemiani@godaddy.com
      // const string ORDER_ID = "463641";
      // const string SHOPPER_ID = "128561";

      //Load Test order from Test and Re-Send Purchase Email
      const string ORDER_ID = "502430";
      const string SHOPPER_ID = "193337";

      string orderXml = GetOrderXml(ORDER_ID, SHOPPER_ID, 1);
      PurchaseEmailRequestData request = new PurchaseEmailRequestData(SHOPPER_ID, string.Empty, ORDER_ID, string.Empty, 0, orderXml, "EN");
      PurchaseEmailResponseData response = (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

      System.Diagnostics.Debug.WriteLine("Request:");
      System.Diagnostics.Debug.WriteLine(request.ToXML());
      System.Diagnostics.Debug.WriteLine("");
      System.Diagnostics.Debug.WriteLine("Response:");
      System.Diagnostics.Debug.WriteLine(response.ToXML());
      foreach (MessagingProcessResponseData responses in response.MessageResponses)
      {
        System.Diagnostics.Debug.WriteLine(responses.ToXML());
      }
      List<string> requestedEmails = response.GetRequestedEmails();
      foreach (string requestedEmail in requestedEmails)
      {
        System.Diagnostics.Debug.WriteLine(requestedEmail);
      }
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


    private static string GetOrderXml(string orderId, string shopperId, int privateLabelID)
    {
      string orderXML = string.Empty;
      try
      {
        Dictionary<string, object> parameters = new Dictionary<string, object>(2);
        parameters["bstrOrderID"] = orderId;
        parameters["bstrShopperID"] = shopperId;
        //Cache Order XML
        CartGetOrderXML request = new CartGetOrderXML(shopperId, string.Empty, orderId, string.Empty, 0, "WsceCommerce_GetOrderXML", parameters, privateLabelID);
        request.RequestTimeout = TimeSpan.FromSeconds(4);
        DataProviderResponseData response = (DataProviderResponseData)DataCache.DataCache.GetProcessRequest(request, 35);
        object responseObject = response.GetResponseObject();

        //object responseObject = PageHelpers.ActionHelpers.DataProviderCall(parameters, "WsceCommerce_GetOrderXML", _siteContext, _shopperContext);
        //null or unknown response
        if (responseObject == null)
        {
          throw new ApplicationException("WsceCommerce webservice returned a NULL or unknown response.");
        }
        orderXML = (string)responseObject;
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.ToString());
      }
      return orderXML;
    }

    private class CartGetOrderXML : DataProviderRequestData
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
        MD5 md5Encryptor = new MD5CryptoServiceProvider();
        md5Encryptor.Initialize();

        byte[] stringBytes = System.Text.Encoding.ASCII.GetBytes("ACOS_ORDER_XML:" + _privateLabelID + ":" + ShopperID + ":" + OrderID);
        byte[] md5Bytes = md5Encryptor.ComputeHash(stringBytes);
        string sValue = BitConverter.ToString(md5Bytes, 0);

        return sValue.Replace("-", "");
      }
    }
  }
}
