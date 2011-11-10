using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.BonsaiPlanFeatures.Interface;
using Atlantis.Framework.BonsaiPlanFeatures.Interface.Types;

namespace Atlantis.Framework.BonsaiPlanFeatures.Tests
{
  [TestClass]
  public class BonsaiPlanFeaturesTests
  {
    /*
        NOTE: These tests may become out of date if the Products/Plan Features/UnifiedProductIds change 
    */

    #region Properties
    private readonly Dictionary<string, string> m_defaultUnlimitedPlanFeatures = new Dictionary<string, string>
                             {
                               {"addl_data_transfer_mb", "0"},
                               {"addl_disk_space_mb", "0"},
                               {"ads_enabled", "0"},
                               {"applicationname", string.Empty},
                               {"cgi_enabled", "0"},
                               {"coldfusion_allowed", "0"},
                               {"compute_cycles", "1"},
                               {"data_transfer_mb", "99999999"},
                               {"datacenter_region", "US"},
                               {"dedicated_ip_allowed", "1"},
                               {"disk_space_mb", "10000"},
                               {"dot_net_enabled", "1"},
                               {"grid_enabled", "1"},
                               {"java_allowed", "0"},
                               {"num_domain_pointers", "0"},
                               {"num_mssql_databases", "1"},
                               {"num_mysql_databases", "10"},
                               {"num_subDomains", "-1"},
                               {"operatingsystem", "Windows"},
                               {"php_enabled", "0"},
                               {"value_app_enabled", "1"},
                               {"win_php_allowed", "0"},
                               {"wsc_commerce_allowed", "0"},
                               {"wsc_plugins_enabled", "0"},
                               {"pf_id", "42101"}
                             };

    public TestContext TestContext { get; set; }
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetUnlimtedHostingPlanFeatures()
    {
      var request = new BonsaiPlanFeaturesRequestData("shopperId", "sourceUrl", "orderId", "pathway", 1, 42101,
                                                      "hosting", false, null);
      var response = Engine.Engine.ProcessRequest(request, 392) as BonsaiPlanFeaturesResponseData;

      Assert.IsNotNull(response);
      Assert.IsTrue(VerifyExpectedResult(m_defaultUnlimitedPlanFeatures, response.PlanFeatures));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetFreeUnlimtedHostingPlanFeatures()
    {
      var request = new BonsaiPlanFeaturesRequestData("shopperId", "sourceUrl", "orderId", "pathway", 1, 42101,
                                                      "hosting", true, null);
      var response = Engine.Engine.ProcessRequest(request, 392) as BonsaiPlanFeaturesResponseData;

      Assert.IsNotNull(response);

      var expectedResult = new Dictionary<string, string>(m_defaultUnlimitedPlanFeatures);
      expectedResult["data_transfer_mb"] = "300000";

      Assert.IsTrue(VerifyExpectedResult(expectedResult, response.PlanFeatures));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetFreeUnlimtedHostingPlanFeaturesWithOverrides()
    {
      var overrides = new List<UnifiedProductIdOverride>
                        {
                          new UnifiedProductIdOverride {UnifiedProductId = 50, Quantity = 1},
                          new UnifiedProductIdOverride {UnifiedProductId = 51, Quantity = 2}
                        };

      var request = new BonsaiPlanFeaturesRequestData("shopperId", "sourceUrl", "orderId", "pathway", 1, 42101,
                                                      "hosting", true, overrides);
      var response = Engine.Engine.ProcessRequest(request, 392) as BonsaiPlanFeaturesResponseData;

      Assert.IsNotNull(response);

      var expectedResult = new Dictionary<string, string>(m_defaultUnlimitedPlanFeatures);
      expectedResult["data_transfer_mb"] = "300000";
      expectedResult["addl_data_transfer_mb"] = "2000";
      expectedResult["addl_disk_space_mb"] = "1000";

      Assert.IsTrue(VerifyExpectedResult(expectedResult, response.PlanFeatures));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetPlanFeatures()
    {
      int unifiedProductId = 60729;
      string productNamespace = "outlook";
      bool isFree = false;

      var request = new BonsaiPlanFeaturesRequestData("shopperId", "sourceUrl", "orderId", "pathway", 1, unifiedProductId,
                                                      productNamespace, isFree, null);
      var response = Engine.Engine.ProcessRequest(request, 392) as BonsaiPlanFeaturesResponseData;

      Assert.IsNotNull(response);
    }

    private static bool VerifyExpectedResult(Dictionary<string, string> expectedResult, Dictionary<string, string> actualResult)
    {
      if (expectedResult == null ^ actualResult == null)
        return false;

      if ((expectedResult == null && actualResult == null) || (expectedResult.Count == 0 && actualResult.Count == 0))
        return true;

      foreach (var key in expectedResult.Keys)
      {
        if (!actualResult.ContainsKey(key) || expectedResult[key] != actualResult[key])
          return false;
      }

      return true;
    }
  }
}
