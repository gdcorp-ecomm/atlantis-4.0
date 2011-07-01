using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace Atlantis.Framework.DataCache.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class DataCacheTests
  {
    public DataCacheTests()
    {
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
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void GetCurrencyDataAll()
    {
      Dictionary<string, Dictionary<string,string>> currencyInfo = DataCache.GetCurrencyDataAll();
      Assert.IsTrue(currencyInfo.ContainsKey("USD"));
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void GetRegistryFee()
    {
      //int price = DataCache.GetRegistryFee(101, "USD");
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void ValidDotTypes()
    {
      HashSet<string> dotTypes = DataCache.GetValidDotTypes();
    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void CustomClassBasic()
    {
      CustomClass resultFirst = DataCache.GetCustomCacheData<CustomClass>("First", CustomClass.GetCustomClass);
      CustomClass resultSecond = DataCache.GetCustomCacheData<CustomClass>("Second", CustomClass.GetCustomClass);
      CustomClass resultThird = DataCache.GetCustomCacheData<CustomClass>("First", CustomClass.GetCustomClass);

      Assert.AreEqual(resultFirst.Name, resultThird.Name);
      Assert.AreNotEqual(resultThird.Name, resultSecond.Name);

    }

    [TestMethod]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void CustomClassStruct()
    {
      int number = DataCache.GetCustomCacheData<int>("1", GetIntValue);
      int number2 = DataCache.GetCustomCacheData<int>("2", GetIntValue);
      int number3 = DataCache.GetCustomCacheData<int>("1", GetIntValue);
      Assert.AreEqual(number, number3);
    }

    private static int GetIntValue(string key)
    {
      if (key == "1")
        return 1;
      else
        return 2;
    }

  }
}
