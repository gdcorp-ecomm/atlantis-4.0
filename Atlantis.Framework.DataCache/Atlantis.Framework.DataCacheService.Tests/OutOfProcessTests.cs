﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Atlantis.Framework.DataCacheService.Tests
{
  [TestClass]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
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
        success = comCache.WithOptionsGetListPrice("101", 1, "0,USD", out price, out isEstimate);
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
        success = comCache.WithOptionsGetPromoPrice("101", 1, 1, "0,USD", out price, out isEstimate);
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
        isOnSale = comCache.WithOptionsIsProductOnSale("101", 1, "0,USD");
      }
    }

  }
}