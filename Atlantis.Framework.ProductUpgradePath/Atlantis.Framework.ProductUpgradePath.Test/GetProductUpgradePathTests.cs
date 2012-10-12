using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.ProductUpgradePath.Impl;
using Atlantis.Framework.ProductUpgradePath.Interface;
using Atlantis.Framework.Interface;
using System.Collections.Generic;

namespace Atlantis.Framework.ProductUpgradePath.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetProductUpgradePathTests
  {
  
    private const string _shopperId = "";
	
	
    public GetProductUpgradePathTests()
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
  [DeploymentItem("Atlantis.Framework.ProductUpgradePath.Impl.dll")]
    public void ProductUpgradeMonthlyPathTest()
    {
     ProductUpgradePathRequestData request = new ProductUpgradePathRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0, 56950, 1);

     List<ProductOptions> filterOptions = new List<ProductOptions>();
     filterOptions.Add(new ProductOptions(3, DurationUnit.Month));
     filterOptions.Add(new ProductOptions(6, DurationUnit.Month));
     filterOptions.Add(new ProductOptions(12, DurationUnit.Month));
     filterOptions.Add(new ProductOptions(24, DurationUnit.Month));
     filterOptions.Add(new ProductOptions(36, DurationUnit.Month));
     request.ProductOptions = filterOptions;
     ProductUpgradePathResponseData response = (ProductUpgradePathResponseData)Engine.Engine.ProcessRequest(request, 607);

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ProductUpgradePath.Impl.dll")]
    public void ProductYearlyUpgradePath()
    {
      ProductUpgradePathRequestData request = new ProductUpgradePathRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0, 6601, 1);

      List<ProductOptions> filterOptions = new List<ProductOptions>();
      filterOptions.Add(new ProductOptions(1, DurationUnit.Year));
      filterOptions.Add(new ProductOptions(2, DurationUnit.Year));
      filterOptions.Add(new ProductOptions(3, DurationUnit.Year));
      filterOptions.Add(new ProductOptions(5, DurationUnit.Year));
      filterOptions.Add(new ProductOptions(10, DurationUnit.Year));
      request.ProductOptions = filterOptions;
      ProductUpgradePathResponseData currentresp = (ProductUpgradePathResponseData)Engine.Engine.ProcessRequest(request, 607);
      System.Diagnostics.Debug.WriteLine(currentresp.Products[6601].DisplayTerm);
      foreach (KeyValuePair<string, int> currentAttrib in currentresp.Products[6601])
      {
        System.Diagnostics.Debug.WriteLine(currentAttrib.Key + ":" + currentAttrib.Value);
      }

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ProductUpgradePath.Impl.dll")]
    public void ProductYearlyUpgradeMonthlyProductPath()
    {
      ProductUpgradePathRequestData request = new ProductUpgradePathRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0, 56950, 1);

      List<ProductOptions> filterOptions = new List<ProductOptions>();
      filterOptions.Add(new ProductOptions(3, DurationUnit.Month));
      filterOptions.Add(new ProductOptions(6, DurationUnit.Month));
      filterOptions.Add(new ProductOptions(1, DurationUnit.Year));
      filterOptions.Add(new ProductOptions(2, DurationUnit.Year));
      filterOptions.Add(new ProductOptions(3, DurationUnit.Year));
      filterOptions.Add(new ProductOptions(5, DurationUnit.Year));
      filterOptions.Add(new ProductOptions(10, DurationUnit.Year));
      request.ProductOptions = filterOptions;
      ProductUpgradePathResponseData response = (ProductUpgradePathResponseData)Engine.Engine.ProcessRequest(request, 607);
      foreach (KeyValuePair<string, int> currentAttrib in response.Products[56950])
      {
        System.Diagnostics.Debug.WriteLine(currentAttrib.Key + ":" + currentAttrib.Value);
      }

    }
  }
}
