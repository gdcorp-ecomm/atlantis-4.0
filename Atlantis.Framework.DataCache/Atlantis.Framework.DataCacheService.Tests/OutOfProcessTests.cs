using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.DataCacheService.Tests
{
  [TestClass]
  //[DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class OutOfProcessTests
  {
    public OutOfProcessTests()
    {
    }

    private TestContext testContextInstance;

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
    public void GetAppSetting()
    {
      string setting = null;
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        setting = comCache.GetAppSetting("SALES_VALID_COUNTRY_SUBDOMAINS");
      }
      Assert.IsFalse(string.IsNullOrEmpty(setting));
    }

    [TestMethod]
    public void GetCacheData()
    {
      string xmlData;
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        xmlData = comCache.GetCacheData("<LinkInfo><param name=\"contextID\" value=\"1\" /></LinkInfo>");
      }

      XElement parsedElement = XElement.Parse(xmlData);
    }

    [TestMethod]
    public void GetCountriesXml()
    {
      string xmlData;
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        xmlData = comCache.GetCountriesXml();
      }

      XElement parsedElement = XElement.Parse(xmlData);
    }

    [TestMethod]
    public void GetStatesXml()
    {
      string xmlData;
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        xmlData = comCache.GetStatesXml(226);
      }

      XElement parsedElement = XElement.Parse(xmlData);
    }

    [TestMethod]
    public void GetCurrencyDataXml()
    {
      string xmlData;
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        xmlData = comCache.GetCurrencyDataXml();
      }

      XElement parsedElement = XElement.Parse(xmlData);
    }

    [TestMethod]
    public void GetTLDData()
    {
      string xmlData;
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        xmlData = comCache.GetTLDData("0");
      }

      XElement parsedElement = XElement.Parse(xmlData);
    }

    [TestMethod]
    public void GetTLDList()
    {
      string xmlData;
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        xmlData = comCache.GetTLDList(1, 2);
      }

      XElement parsedElement = XElement.Parse(xmlData);
    }

    [TestMethod]
    public void GetPLData()
    {
      string plData;
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        plData = comCache.GetPLData(1724, 0);
      }
      Assert.IsFalse(string.IsNullOrEmpty(plData));
    }

    [TestMethod]
    public void GetPrivateLabelId()
    {
      int privateLableId;
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        privateLableId = comCache.GetPrivateLabelId("hunter");
      }
      Assert.AreEqual(1724, privateLableId);
    }

    [TestMethod]
    public void GetProgId()
    {
      string progId;
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        progId = comCache.GetProgId(1724);
      }
      Assert.AreEqual("hunter", progId);
    }

    [TestMethod]
    public void GetPrivateLabelType()
    {
      int privateLableType;
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        privateLableType = comCache.GetPrivateLabelType(1724);
      }
      Assert.AreEqual(2, privateLableType);
    }

    [TestMethod]
    public void IsPrivateLabelActive()
    {
      bool isPrivateLabelActive;
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        isPrivateLabelActive = comCache.IsPrivateLabelActive(1724);
      }
      Assert.IsTrue(isPrivateLabelActive);
    }

    [TestMethod]
    public void ConvertToPFID()
    {
      int pfid;
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        pfid = comCache.ConvertToPFID(101, 2);
      }
      Assert.AreNotEqual(101, pfid);
    }

    [TestMethod]
    public void WithOptionsGetListPrice()
    {
      bool success;
      int price;
      bool isEstimate;

      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        success = comCache.WithOptionsGetListPrice(101, 1, "0,USD", out price, out isEstimate);
      }

      Assert.IsTrue(success);
      Assert.IsFalse(isEstimate);
      Assert.AreNotEqual(0, price);
    }

    [TestMethod]
    public void WithOptionsGetPromoPrice()
    {
      bool success;
      int price;
      bool isEstimate;

      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        success = comCache.WithOptionsGetPromoPrice(101, 1, 1, "0,USD", out price, out isEstimate);
      }

      Assert.IsTrue(success);
      Assert.IsFalse(isEstimate);
      Assert.AreNotEqual(0, price);
    }

    [TestMethod]
    public void WithOptionsGetIsProductOnSale()
    {
      bool isOnSale;

      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        isOnSale = comCache.WithOptionsIsProductOnSale(101, 1, "0,USD");
      }
    }

    [TestMethod]
    public void GetMgrCategoriesForUser()
    {
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        string result = comCache.GetMgrCategoriesForUser(2055);
        XElement parsedResult = XElement.Parse(result);
      }
    }

    private const string _REQUESTFORMATDISCOUNT = "<PriceEstimateRequest privateLabelID=\"{0}\" membershipLevel=\"{1}\" transactionCurrency=\"{2}\"><Item unifiedProductID=\"{4}\" discount_code=\"{3}\"/></PriceEstimateRequest>";

    [TestMethod]
    public void GetPriceEstimateDiscountCode()
    {
      string request = string.Format(_REQUESTFORMATDISCOUNT, "1", "0", "USD", "cbwsp01", "56950");

      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        string response = comCache.GetPriceEstimate(request);
        XElement parsedResponse = XElement.Parse(response);
      }
    }

    private const string _REQUESTFORMATSOURCE = "<PriceEstimateRequest privateLabelID=\"{0}\" membershipLevel=\"{1}\" transactionCurrency=\"{2}\" source_code=\"{3}\"><Item unifiedProductID=\"{4}\" /></PriceEstimateRequest>";

    [TestMethod]
    public void GetPriceEstimateSourceCode()
    {
      string request = string.Format(_REQUESTFORMATSOURCE, "1", "0", "USD", "testhost", "42002");

      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        string response = comCache.GetPriceEstimate(request);
        XElement parsedResponse = XElement.Parse(response);
      }
    }

    [TestMethod]
    public void DisplayCachedData()
    {
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        int privateLabelId = comCache.GetPrivateLabelId("hunter");
        string cacheData = comCache.DisplayCachedData(4, "getprivatelabelid");
      }
    }

    [TestMethod]
    public void ClearCachedData()
    {
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        int privateLabelId = comCache.GetPrivateLabelId("hunter");
        comCache.ClearCachedData("getprivatelabelid");
        string cacheData = comCache.DisplayCachedData(4, "getprivatelabelid");
      }
    }

    [TestMethod]
    public void GetShopperRenewingServices()
    {
      using (GdDataCacheOutOfProcess comCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        var response = comCache.GetShopperRenewingServices("856907");
        Assert.IsTrue(response.Split('|').Length.Equals(2));
      }
    }
  }
}
