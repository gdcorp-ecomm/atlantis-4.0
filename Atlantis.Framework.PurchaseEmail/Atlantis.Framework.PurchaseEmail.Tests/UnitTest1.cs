using System;
using System.IO;
using System.Reflection;
using Atlantis.Framework.PurchaseEmail.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

    private string LoadSampleOrderXml(string filename)
    {
      string path = Assembly.GetExecutingAssembly().Location;
      string fullpath = Path.Combine(Path.GetDirectoryName(path), filename);
      string orderXml = File.ReadAllText(fullpath);
      return orderXml;
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dataprovider.xml")]
    [DeploymentItem("DomainWithNoAutoRenewViaAlipay.xml")]
    public void PurchaseEmailAlipayNoAutoRenewalNoBackupSourceTest()
    {
      Engine.Engine.ReloadConfig();
      string orderXml = LoadSampleOrderXml("DomainWithNoAutoRenewViaAlipay.xml");
      PurchaseEmailRequestData request =
        new PurchaseEmailRequestData("70364", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
      PurchaseEmailResponseData response =
       (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dataprovider.xml")]
    [DeploymentItem("DomainWithAutoRenewViaAlipay.xml")]
    public void PurchaseEmailAlipayAutoRenewalNoBackupSourceTest()
    {
      Engine.Engine.ReloadConfig();
      string orderXml = LoadSampleOrderXml("DomainWithAutoRenewViaAlipay.xml");
      PurchaseEmailRequestData request =
        new PurchaseEmailRequestData("70364", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
      PurchaseEmailResponseData response =
       (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dataprovider.xml")]
    [DeploymentItem("DomainWithAutoRenewViaAlipayWithVisaBackupSource.xml")]
    public void PurchaseEmailAlipayAutoRenewalWithBackupSourceTest()
    {
      Engine.Engine.ReloadConfig();
      string orderXml = LoadSampleOrderXml("DomainWithAutoRenewViaAlipayWithVisaBackupSource.xml");
      PurchaseEmailRequestData request =
        new PurchaseEmailRequestData("122508", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
      PurchaseEmailResponseData response =
       (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);
    }


    [TestMethod]
    [DeploymentItem("GiftCard.xml")]
    [ExpectedException(typeof(ArgumentException))]
    public void GiftCardAdditionalInfo()
    {
      Engine.Engine.ReloadConfig();
      string orderXml = LoadSampleOrderXml("GiftCard.xml");
      PurchaseEmailRequestData request =
        new PurchaseEmailRequestData("832652", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
      PurchaseEmailResponseData response =
       (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dataprovider.xml")]
    [DeploymentItem("sampleorder1min.xml")]
    public void PurchaseEmailBasicTest()
    {
      string orderXml = LoadSampleOrderXml("sampleorder1min.xml");
      PurchaseEmailRequestData request =
        new PurchaseEmailRequestData("832652", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
      request.AddOption("IsDevServer", "true");

      PurchaseEmailResponseData response =
        (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

      Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dataprovider.xml")]
    [DeploymentItem("HostingOrder.xml")]
    public void PurchaseEmailHostingOrderTest()
    {
        string orderXml = LoadSampleOrderXml("HostingOrder.xml");
        PurchaseEmailRequestData request =
          new PurchaseEmailRequestData("861796", string.Empty, string.Empty, string.Empty, 0, orderXml,"ES");
        request.AddOption("IsDevServer", "true");

        PurchaseEmailResponseData response =
          (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

        Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dataprovider.xml")]
    [DeploymentItem("HostingOrder2.xml")]
    public void PurchaseEmailHostingOrderTest2()
    {
        string orderXml = LoadSampleOrderXml("HostingOrder2.xml");
        PurchaseEmailRequestData request =
          new PurchaseEmailRequestData("861796", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
        request.AddOption("IsDevServer", "true");

        PurchaseEmailResponseData response =
          (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

        Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dataprovider.xml")]
    [DeploymentItem("HostingDomainOrder.xml")]
    public void PurchaseEmailHostingDomainOrderTest()
    {
        string orderXml = LoadSampleOrderXml("HostingDomainOrder.xml");
        PurchaseEmailRequestData request =
          new PurchaseEmailRequestData("861796", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
        request.AddOption("IsDevServer", "true");

        PurchaseEmailResponseData response =
          (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

        Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dataprovider.xml")]
    [DeploymentItem("HostingDomainOrder.xml")]
    public void PurchaseEmailHostingDomainNewCustomerOrderTest()
    {
        string orderXml = LoadSampleOrderXml("HostingDomainOrder.xml");
        PurchaseEmailRequestData request =
          new PurchaseEmailRequestData("861796", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
        request.AddOption("IsDevServer", "true");
        request.AddOption("IsNewShopper", "true");

        PurchaseEmailResponseData response =
          (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

        Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dataprovider.xml")]
    [DeploymentItem("WSTOrder.xml")]
    public void PurchaseEmailWSTOrderTest()
    {
        string orderXml = LoadSampleOrderXml("WSTOrder.xml");
        PurchaseEmailRequestData request =
          new PurchaseEmailRequestData("121079", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
        //request.AddOption("IsDevServer", "true");
        //request.AddOption("IsNewShopper", "true");

        PurchaseEmailResponseData response =
          (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

        Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dataprovider.xml")]
    [DeploymentItem("DomainWithDBPOrder.xml")]
    public void PurchaseEmailDBPOrderTest()
    {
        string orderXml = LoadSampleOrderXml("DomainWithDBPOrder.xml");
        PurchaseEmailRequestData request =
          new PurchaseEmailRequestData("861796", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
        request.AddOption("IsDevServer", "true");
        //request.AddOption("IsNewShopper", "true");

        PurchaseEmailResponseData response =
          (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

        Assert.IsTrue(response.IsSuccess);
    }



    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dataprovider.xml")]
    [DeploymentItem("samplebadshopperorder.xml")]
    public void PurchaseEmailBasicTestErrorBuildingMessageRequest()
    {
      string orderXml = LoadSampleOrderXml("samplebadshopperorder.xml");
      PurchaseEmailRequestData request =
        new PurchaseEmailRequestData("nothere", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
      request.AddOption("IsDevServer", "true");

      PurchaseEmailResponseData response =
        (PurchaseEmailResponseData)Engine.Engine.ProcessRequest(request, 83);

      Assert.IsTrue(response.IsSuccess);
    }


    [TestMethod]
    [DeploymentItem("sampleorder1min.xml")]
    [ExpectedException(typeof(ArgumentException))]
    public void PurchaseEmailInvalidOptionTest()
    {
      string orderXml = LoadSampleOrderXml("sampleorder1min.xml");
      PurchaseEmailRequestData request =
        new PurchaseEmailRequestData("832652", string.Empty, string.Empty, string.Empty, 0, orderXml, "ES");
      request.AddOption("CoolEmail", "true");
    }

  }
}
