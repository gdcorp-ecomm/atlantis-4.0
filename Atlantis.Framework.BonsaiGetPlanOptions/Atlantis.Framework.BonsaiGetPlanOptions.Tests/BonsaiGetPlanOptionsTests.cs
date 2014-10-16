using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.BonsaiGetPlanOptions.Interface;
using Atlantis.Framework.BonsaiGetPlanOptions.Interface.Types;

namespace Atlantis.Framework.BonsaiGetPlanOptions.Tests
{
  [TestClass]
  [DeploymentItem("Atlantis.Framework.BonsaiGetPlanOptions.Impl.dll")]
  public class BonsaiGetPlanOptionsTests
  {
    public TestContext TestContext { get; set; }

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
    public void GetPlanOptionsSharedHosting()
    {
      var request = new BonsaiGetPlanOptionsRequestData("847235", "http://localhost", string.Empty, string.Empty,
                                                        0, "SharedHosting1", "hosting", "billing", 0, 1);
      
      var response = Engine.Engine.ProcessRequest(request, 357) as BonsaiGetPlanOptionsResponseData;

      Assert.IsTrue(response != null && response.AtlantisException == null, "Request failed");

      Assert.IsTrue(response.ProductPlans != null && response.ProductPlans.Count == 8, "ProductPlans count failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "42114" && plan.IsCurrent) != -1, "ProductPlans 42114 owned failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "42004") != -1, "ProductPlans 42004 failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "42014") != -1, "ProductPlans 42014 failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "42024") != -1, "ProductPlans 42024 failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "42104") != -1, "ProductPlans 42104 failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "42124") != -1, "ProductPlans 42124 failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "7604") != -1, "ProductPlans 7604 failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "7616") != -1, "ProductPlans 7616 failed");

      Assert.IsTrue(response.FilteredProductPlans.Count == 0, "Filter plan count failed");

      Assert.IsTrue(response.PrepaidAddons != null && response.PrepaidAddons.Count == 2, "Prepaids count failed");
      Assert.IsTrue(response.PrepaidAddons.FindIndex(pp => pp.UnifiedProductId == "68") != -1, "Prepaid 68 failed");
      Assert.IsTrue(response.PrepaidAddons.FindIndex(pp => pp.UnifiedProductId == "69") != -1, "Prepaid 69 failed");

      Assert.IsTrue(response.Addons != null && response.Addons.Count() == 2, "Addons count failed");
      Assert.IsTrue(response.Addons.HasKey(14), "Addon category key 14 failed");
      Assert.IsTrue(response.Addons[14] != null && response.Addons[14].Count == 2, "Addon cat 14 count failed");
      Assert.IsTrue(response.Addons[14].Owned != null && response.Addons[14].Owned.UnifiedProductId == string.Empty, "Addon cat 14 owned upid '' failed");
      Assert.IsTrue(response.Addons[14].ToList().FindIndex(a => a.UnifiedProductId == "50") != -1, "Addon cat 14 upid 50 failed");
      Assert.IsTrue(response.Addons[14].ToList()[0].ChildAddons == null || response.Addons[14].ToList()[0].ChildAddons.Count() == 0, "Addons cat 14, childaddons empty failed");
      
      Assert.IsTrue(response.Addons.HasKey(15), "Addon category key 15 failed");
      Assert.IsTrue(response.Addons[15] != null && response.Addons[15].Count == 2, "Addon cat 15 count failed");
      Assert.IsTrue(response.Addons[15].Owned != null && response.Addons[15].Owned.UnifiedProductId == string.Empty, "Addon cat 15 owned upid '' failed");
      Assert.IsTrue(response.Addons[15].ToList().FindIndex(a => a.UnifiedProductId == "51") != -1, "Addon cat 15 upid 51 failed");
      Assert.IsTrue(response.Addons[15].ToList()[0].ChildAddons == null || response.Addons[15].ToList()[0].ChildAddons.Count() == 0, "Addons cat 15, childaddons empty failed");
      
      Assert.IsTrue(response.FilteredProductPlans != null && response.FilteredProductPlans.Count == 0);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPlanOptionsSharedHostingFiltered()
    {
      var request = new BonsaiGetPlanOptionsRequestData("847235", "http://localhost", string.Empty, string.Empty,
                                                        0, "SharedHostingFiltered1", "hosting", "billing", 0, 1);

      var response = Engine.Engine.ProcessRequest(request, 357) as BonsaiGetPlanOptionsResponseData;

      Assert.IsTrue(response != null && response.AtlantisException == null, "Request failed");
      Assert.IsTrue(response.ProductPlans != null && response.ProductPlans.Count == 2, "ProductPlans count failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "163" && plan.IsCurrent) != -1, "ProductPlans 163 owned failed");

      Assert.IsTrue(response.FilteredProductPlans.Count == 2, "Filtered plan count failed");
      Assert.IsTrue(response.FilteredProductPlans.FindIndex(fpp => fpp.UnifiedProductId == "42101") != -1 &&
                    response.FilteredProductPlans.FindIndex(fpp => fpp.UnifiedProductId == "42111") != -1, "Filtered plans failed");  
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPlanOptionsVdedLegacy()
    {
      var request = new BonsaiGetPlanOptionsRequestData("857527", "http://localhost", string.Empty, string.Empty,
                                                        0, "LegacyVph1", "vph", "orion", 0, 1);
      var response = Engine.Engine.ProcessRequest(request, 357) as BonsaiGetPlanOptionsResponseData;

      Assert.IsTrue(response != null && response.AtlantisException == null, "Request failed");

      Assert.IsTrue(response.ProductPlans != null && response.ProductPlans.Count == 1, "ProductPlans count failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "1212" && plan.IsCurrent) != -1, "ProductPlans 42114 owned failed");
      Assert.IsTrue(response.FilteredProductPlans.Count == 0, "Filtered plan count failed");

      Assert.IsTrue(response.PrepaidAddons != null && response.PrepaidAddons.Count == 1, "Prepaids count failed");
      Assert.IsTrue(response.PrepaidAddons.FindIndex(pp => pp.UnifiedProductId == "68") != -1, "Prepaid 68 failed");

      Assert.IsTrue(response.Addons != null && response.Addons.Count() == 5, "Addons count failed");
      Assert.IsTrue(response.Addons.HasKey(18), "Addon category key 18 failed");
      Assert.IsTrue(response.Addons[18] != null && response.Addons[18].Count == 2, "Addon cat 18 count failed");
      Assert.IsTrue(response.Addons[18].Owned != null && response.Addons[18].Owned.UnifiedProductId == string.Empty, "Addon cat 18 owned upid '' failed");

      Assert.IsTrue(response.Addons.HasKey(20), "Addon category key 20 failed");
      Assert.IsTrue(response.Addons[20] != null && response.Addons[20].Count == 8, "Addon cat 20 count failed");
      Assert.IsTrue(response.Addons[20].Owned != null && response.Addons[20].Owned.UnifiedProductId == string.Empty, "Addon cat 20 owned upid '' failed");

      Assert.IsTrue(response.Addons.HasKey(24), "Addon category key 24 failed");
      Assert.IsTrue(response.Addons[24] != null && response.Addons[24].Count == 1, "Addon cat 24 count failed");
      Assert.IsTrue(response.Addons[24].Owned != null && response.Addons[24].Owned.UnifiedProductId == "1209", "Addon cat 24 owned upid '1209' failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons != null && response.Addons[24].Owned.ChildAddons.Count() == 1, "Addon cat 24 owned childaddon count failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons.HasKey(25) && response.Addons[24].Owned.ChildAddons[25] != null, "Addon cat 24 owned childaddon cat 25 failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Count == 1 && 
                    response.Addons[24].Owned.ChildAddons[25].Owned != null &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.UnifiedProductId == string.Empty, "Addon cat 24, 25 owned failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons != null &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons.Count() == 1, "Addon cat 24, 25 childaddon count failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons.HasKey(26) &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26] != null, "Addon cat 24, 25, 26 failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Count == 1 &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned != null &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.UnifiedProductId == "1294", "Addon cat 24, 25, 26 owned failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons != null &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons.Count() == 1 &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons.HasKey(21) &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Count == 5, "Addon cat 24,25,26,21 failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned != null &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.UnifiedProductId == "1223", "Addon cat 24,25,26,21 owned failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].ToList().FindIndex(a => a.UnifiedProductId == "1239") != -1 &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].ToList().FindIndex(a => a.UnifiedProductId == "1222") != -1 &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].ToList().FindIndex(a => a.UnifiedProductId == "1224") != -1, "Cat 21 option failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons != null &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons.Count() == 1 &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons.HasKey(22) &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Count == 3, "Addon cat 24,25,26,21 childaddon count failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Owned != null &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Owned.UnifiedProductId == string.Empty, "Addon cat 24,25,26,21,22 owned failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].ToList().FindIndex(a => a.UnifiedProductId == "2750") != -1 &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].ToList().FindIndex(a => a.UnifiedProductId == "2751") != -1, "Addon cat 24,25,26,21,22 options failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Owned.ChildAddons != null &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Owned.ChildAddons.Count() == 1 &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Owned.ChildAddons.HasKey(23) &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Owned.ChildAddons[23].Count == 2, "Addon cat 24,25,26,21,22,23 count failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Owned.ChildAddons[23].Owned != null &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Owned.ChildAddons[23].Owned.UnifiedProductId == string.Empty, "Addon cat 24, 25, 26, 21, 22, 23 owned failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Owned.ChildAddons[23].ToList().FindIndex(a => a.UnifiedProductId == "1260") != -1, "Addon cat 24, 25, 26, 21, 22, 23 options failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Owned.ChildAddons[23].Owned.ChildAddons == null ||
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Owned.ChildAddons[23].Owned.ChildAddons.Count() == 0, "Addon cat 24, 25, 26, 21, 22, 23 owned children empty failed");

      Assert.IsTrue(response.Addons.HasKey(19), "Addon category key 19 failed");
      Assert.IsTrue(response.Addons[19] != null && response.Addons[19].Count == 4, "Addon cat 19 count failed");
      Assert.IsTrue(response.Addons[19].Owned != null && response.Addons[19].Owned.UnifiedProductId == string.Empty, "Addon cat 19 owned upid '' failed");

      Assert.IsTrue(response.Addons.HasKey(27), "Addon category key 27 failed");
      Assert.IsTrue(response.Addons[27] != null && response.Addons[27].Count == 1, "Addon cat 27 count failed");
      Assert.IsTrue(response.Addons[27].Owned != null && response.Addons[27].Owned.UnifiedProductId == string.Empty, "Addon cat 27 owned upid '' failed");
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPlanOptionsVdedLegacyLarge()
    {
      var request = new BonsaiGetPlanOptionsRequestData("857527", "http://localhost", string.Empty, string.Empty,
                                                        0, "LegacyVph2", "vph", "orion", 0, 1);
      var response = Engine.Engine.ProcessRequest(request, 357) as BonsaiGetPlanOptionsResponseData;

      Assert.IsTrue(response != null && response.AtlantisException == null, "Request failed");
      Assert.IsTrue(response.ProductPlans != null && response.ProductPlans.Count == 1, "ProductPlans count failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "1212" && plan.IsCurrent) != -1, "ProductPlans 1212 owned failed");

      Assert.IsTrue(response.FilteredProductPlans.Count == 0, "Filtered plan count failed");

      Assert.IsTrue(response.Addons != null && response.Addons.Count() == 5, "Addons count failed");
      Assert.IsTrue(response.Addons.HasKey(20) &&
                    response.Addons.HasKey(24) &&
                    response.Addons.HasKey(19) &&
                    response.Addons.HasKey(27) &&
                    response.Addons.HasKey(18), "Addons keys failed");

      Assert.IsTrue(response.PrepaidAddons != null && response.PrepaidAddons.Count == 1, "Prepaids count failed");
      Assert.IsTrue(response.PrepaidAddons.FindIndex(pp => pp.UnifiedProductId == "68") != -1, "Prepaid 68 failed");
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPlanOptionsVdedLegacyEu()
    {
      var request = new BonsaiGetPlanOptionsRequestData("857527", "http://localhost", string.Empty, string.Empty,
                                                        0, "LegacyVphEU1", "vph", "orion", 0, 1);
      var response = Engine.Engine.ProcessRequest(request, 357) as BonsaiGetPlanOptionsResponseData;

      Assert.IsTrue(response != null && response.AtlantisException == null, "Request failed");

      Assert.IsTrue(response.ProductPlans != null && response.ProductPlans.Count == 1, "ProductPlans count failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "1303" && plan.IsCurrent) != -1, "ProductPlans 1303 owned failed");
      Assert.IsTrue(response.FilteredProductPlans.Count == 0, "Filtered plan count failed");

      Assert.IsTrue(response.PrepaidAddons != null && response.PrepaidAddons.Count == 1, "Prepaids count failed");
      Assert.IsTrue(response.PrepaidAddons.FindIndex(pp => pp.UnifiedProductId == "68") != -1, "Prepaid 68 failed");

      Assert.IsTrue(response.Addons != null && response.Addons.Count() == 4, "Addons count failed");
      Assert.IsTrue(response.Addons.HasKey(18), "Addon category key 18 failed");
      Assert.IsTrue(response.Addons[18] != null && response.Addons[18].Count == 2, "Addon cat 18 count failed");
      Assert.IsTrue(response.Addons[18].Owned != null && response.Addons[18].Owned.UnifiedProductId == string.Empty, "Addon cat 18 owned upid '' failed");

      Assert.IsTrue(response.Addons.HasKey(24), "Addon category key 24 failed");
      Assert.IsTrue(response.Addons[24] != null && response.Addons[24].Count == 1, "Addon cat 24 count failed");
      Assert.IsTrue(response.Addons[24].Owned != null && response.Addons[24].Owned.UnifiedProductId == string.Empty, "Addon cat 24 owned upid '' failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons != null && response.Addons[24].Owned.ChildAddons.Count() == 1, "Addon cat 24 owned childaddon count failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons.HasKey(25) && response.Addons[24].Owned.ChildAddons[25] != null, "Addon cat 24 owned childaddon cat 25 failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Count == 1 &&
                    response.Addons[24].Owned.ChildAddons[25].Owned != null &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.UnifiedProductId == string.Empty, "Addon cat 24, 25 owned failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons != null &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons.Count() == 1, "Addon cat 24, 25 childaddon count failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons.HasKey(26) &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26] != null, "Addon cat 24, 25, 26 failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Count == 1 &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned != null &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.UnifiedProductId == string.Empty, "Addon cat 24, 25, 26 owned failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons != null &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons.Count() == 1 &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons.HasKey(21) &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Count == 7, "Addon cat 24,25,26,21 failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned != null &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.UnifiedProductId == string.Empty, "Addon cat 24,25,26,21 owned failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].ToList().FindIndex(a => a.UnifiedProductId == "1665") != -1 &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].ToList().FindIndex(a => a.UnifiedProductId == "1666") != -1 &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].ToList().FindIndex(a => a.UnifiedProductId == "1667") != -1 &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].ToList().FindIndex(a => a.UnifiedProductId == "1672") != -1 &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].ToList().FindIndex(a => a.UnifiedProductId == "1674") != -1 &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].ToList().FindIndex(a => a.UnifiedProductId == "3178") != -1, "Cat 21 option failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons != null &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons.Count() == 1 &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons.HasKey(22) &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Count == 1, "Addon cat 24,25,26,21,22 failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Owned != null &&
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Owned.UnifiedProductId == string.Empty, "Addon cat 24,25,26,21,22 owned failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Owned.ChildAddons == null ||
                    response.Addons[24].Owned.ChildAddons[25].Owned.ChildAddons[26].Owned.ChildAddons[21].Owned.ChildAddons[22].Owned.ChildAddons.Count() == 0, "Addon cat 24,25,26,21,22 child count failed");

      Assert.IsTrue(response.Addons.HasKey(19), "Addon category key 19 failed");
      Assert.IsTrue(response.Addons[19] != null && response.Addons[19].Count == 4, "Addon cat 19 count failed");
      Assert.IsTrue(response.Addons[19].Owned != null && response.Addons[19].Owned.UnifiedProductId == string.Empty, "Addon cat 19 owned upid '' failed");

      Assert.IsTrue(response.Addons.HasKey(27), "Addon category key 27 failed");
      Assert.IsTrue(response.Addons[27] != null && response.Addons[27].Count == 1, "Addon cat 27 count failed");
      Assert.IsTrue(response.Addons[27].Owned != null && response.Addons[27].Owned.UnifiedProductId == string.Empty, "Addon cat 27 owned upid '' failed");
    
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPlanOptionsVdedPackage()
    {
      var request = new BonsaiGetPlanOptionsRequestData("857527", "http://localhost", string.Empty, string.Empty,
                                                        0, "VphPackage1", "vph", "orion", 0, 1);
      var response = Engine.Engine.ProcessRequest(request, 357) as BonsaiGetPlanOptionsResponseData;

      Assert.IsTrue(response != null && response.AtlantisException == null, "Request failed");
      Assert.IsTrue(response.ProductPlans != null && response.ProductPlans.Count == 4, "ProductPlans count failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "3045" && plan.IsCurrent) != -1, "ProductPlans 3045 owned failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "3046") != -1 &&
                    response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "3047") != -1 &&
                    response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "3048") != -1, "ProductPlan options failed");

      Assert.IsTrue(response.FilteredProductPlans.Count == 0, "Filtered plan count failed");
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPlanOptionsDedLegacy()
    {
      var request = new BonsaiGetPlanOptionsRequestData("847235", "http://localhost", string.Empty, string.Empty,
                                                        0, "LegacyDhs1", "hosting", "billing", 0, 1);

      var response = Engine.Engine.ProcessRequest(request, 357) as BonsaiGetPlanOptionsResponseData;

      Assert.IsTrue(response != null && response.AtlantisException == null, "Request failed");

      Assert.IsTrue(response.ProductPlans != null && response.ProductPlans.Count == 1, "ProductPlans count failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "1211" && plan.IsCurrent) != -1, "ProductPlans 1211 owned failed");
      Assert.IsTrue(response.FilteredProductPlans.Count == 0, "Filtered plan count failed");

      Assert.IsTrue(response.PrepaidAddons != null && response.PrepaidAddons.Count == 1, "Prepaids count failed");
      Assert.IsTrue(response.PrepaidAddons.FindIndex(pp => pp.UnifiedProductId == "68") != -1, "Prepaid 68 failed");

      Assert.IsTrue(response.Addons != null && response.Addons.Count() == 11, "Addons count failed");
      Assert.IsTrue(response.Addons.HasKey(18) && response.Addons.HasKey(19) &&
                    response.Addons.HasKey(20) && response.Addons.HasKey(34) &&
                    response.Addons.HasKey(29) && response.Addons.HasKey(36) &&
                    response.Addons.HasKey(37) && response.Addons.HasKey(38) &&
                    response.Addons.HasKey(39) && response.Addons.HasKey(24) &&
                    response.Addons.HasKey(35), "Addons keys failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[21].Owned.ChildAddons[22].Owned.ChildAddons[23].Owned != null, "Assisted addon check failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[21].Count == 8, "Control panel addon options count failed");
      Assert.IsTrue(response.Addons[24].Owned.ChildAddons[21].Owned.UnifiedProductId == "1219", "Control panel owned failed");
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPlanOptionsDedPkg()
    {
      var request = new BonsaiGetPlanOptionsRequestData("847235", "http://localhost", string.Empty, string.Empty,
                                                        0, "DhsPackage1", "hosting", "billing", 0, 1);

      var response = Engine.Engine.ProcessRequest(request, 357) as BonsaiGetPlanOptionsResponseData;

      Assert.IsTrue(response != null && response.AtlantisException == null, "Request failed");
      Assert.IsTrue(response.ProductPlans != null && response.ProductPlans.Count == 1, "ProductPlans count failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "3150" && plan.IsCurrent) != -1, "ProductPlans 3150 owned failed");

      Assert.IsTrue(response.FilteredProductPlans.Count == 0, "Filtered plan count failed");
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPlanOptionsAddonsJson()
    {
      string actual;
      string expected;

      using (var reader = new StreamReader("LegacyVph1JSON.txt"))
      {
        expected = reader.ReadToEnd();
      }
      
      var request = new BonsaiGetPlanOptionsRequestData("857527", "http://localhost", string.Empty, string.Empty,
                                                        0, "LegacyVph1", "vph", "orion", 0, 1);
      var response = Engine.Engine.ProcessRequest(request, 357) as BonsaiGetPlanOptionsResponseData;

      var serializer = new DataContractJsonSerializer(typeof(CategoryAddonCollection));
      using (var ms = new MemoryStream())
      {
        serializer.WriteObject(ms, response.Addons);
        actual = Encoding.Default.GetString(ms.ToArray());
      }

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPlanOptionsCashParking1()
    {
      var request = new BonsaiGetPlanOptionsRequestData("847235", "http://localhost", string.Empty, string.Empty,
                                                        0, "CashParking1", "cashpark", "billing", 0, 1);

      var response = Engine.Engine.ProcessRequest(request, 357) as BonsaiGetPlanOptionsResponseData;

      Assert.IsTrue(response != null && response.AtlantisException == null, "Request failed");
      Assert.IsTrue(response.ProductPlans != null && response.ProductPlans.Count == 2, "ProductPlans count failed");

      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "5300" && plan.IsCurrent) != -1, "ProductPlans 5300 owned failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "5304") != -1, "ProductPlans 5304 failed");
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPlanOptionsOFF1()
    {
      var request = new BonsaiGetPlanOptionsRequestData("17874", "http://localhost", string.Empty, string.Empty,
                                                        0, "OFF1", "onlinestor", "billing", 0, 1);

      var response = Engine.Engine.ProcessRequest(request, 357) as BonsaiGetPlanOptionsResponseData;

      Assert.IsTrue(response != null && response.AtlantisException == null, "Request failed");
      Assert.IsTrue(response.ProductPlans != null && response.ProductPlans.Count == 2, "ProductPlans count failed");

      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "3810" && plan.IsCurrent) != -1, "ProductPlans 3810 owned failed");
      Assert.IsTrue(response.ProductPlans.FindIndex(plan => plan.UnifiedProductId == "3830") != -1, "ProductPlans 3830 failed");
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPlanOptionsEmail1()
    {
      var request = new BonsaiGetPlanOptionsRequestData("64j", "http://localhost", string.Empty, string.Empty,
                                                        0, "Email1", "email", "billing", 0, 1);

      var response = Engine.Engine.ProcessRequest(request, 357) as BonsaiGetPlanOptionsResponseData;

      Assert.IsTrue(response != null && response.AtlantisException == null, "Request failed");
      Assert.IsTrue(response.ProductPlans != null && response.ProductPlans.Count == 2, "ProductPlans count failed");

    }
  
  }
}
