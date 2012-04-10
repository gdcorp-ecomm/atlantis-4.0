using System.Collections.Generic;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DotTypeCache.Tests
{
  [TestClass]
  public class DotTypeCacheTests
  {
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

    [TestInitialize]
    public void InitializeTests()
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, TestContexts>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, TestContexts>();
      MockHttpContext.SetMockHttpContext("default.aspx", "http://siteadmin.debug.intranet.gdg/default.aspx", string.Empty);
      ISiteContext siteContext = HttpProviderContainer.Instance.Resolve<ISiteContext>();
      ((TestContexts)siteContext).SetContextInfo(1, "832652");
      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      ((TestContexts)shopperContext).SetContextInfo(1, "832652");
      IManagerContext managerContext = HttpProviderContainer.Instance.Resolve<IManagerContext>();
      ((TestContexts)managerContext).SetContextInfo(1, "832652");

    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void GetDotTypeProductIds()
    {
      int productIdDefault = DotTypeCache.GetRegistrationProductId("co.uk", 2, 1);
      int productIdFirstRegistrar = DotTypeCache.GetTransferProductId("co.uk", "1", 2, 1);
      int productIdSecondRegistrar = DotTypeCache.GetRegistrationProductId("co.uk","2", 2, 1);
      string description = DotTypeCache.GetAdditionalInfoValue("co.uk", "Description");
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo("co.uk");
      Assert.IsTrue((productIdDefault * productIdFirstRegistrar * productIdSecondRegistrar) != 0);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void GetDotTypeProductIds2()
    {
      List<int> regLengths = new List<int>();
      regLengths.Add(1);
      regLengths.Add(2);
      regLengths.Add(3);
      regLengths.Add(4);
      regLengths.Add(5);
      regLengths.Add(6);
      regLengths.Add(7);
      regLengths.Add(8);
      regLengths.Add(9);
      regLengths.Add(10);
      IDotTypeInfo dotTypeInfo = DotTypeCache.GetDotTypeInfo("CO.UK");
      List<int> RegistrationProductIds = dotTypeInfo.GetValidRegistrationProductIdList(1, regLengths.ToArray());
      List<int> TransferProductIds = dotTypeInfo.GetValidTransferProductIdList(1, regLengths.ToArray());
      List<int> renewProductIds = dotTypeInfo.GetValidRenewalProductIdList(1, regLengths.ToArray());
      List<int> renewProductIds2 = dotTypeInfo.GetValidRenewalProductIdList("1", 1, regLengths.ToArray());
      List<int> renewProductIds3 = dotTypeInfo.GetValidRenewalProductIdList("2", 1, regLengths.ToArray());
      int renewProductId = dotTypeInfo.GetRenewalProductId("1", 2, 1);
      Assert.IsTrue(renewProductId == 13313);
      Assert.IsTrue((RegistrationProductIds.Count * TransferProductIds.Count * renewProductIds.Count) != 0);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void GetDotTypeProductIdsXxx()
    {
      List<int> regLengths = new List<int>();
      regLengths.Add(1);
      regLengths.Add(2);
      regLengths.Add(3);
      regLengths.Add(4);
      regLengths.Add(5);
      regLengths.Add(6);
      regLengths.Add(7);
      regLengths.Add(8);
      regLengths.Add(9);
      regLengths.Add(10);
      //IDotTypeInfo dotTypeInfo = DotTypeCache.GetDotTypeInfo("XXX");
      int productId = DotTypeCache.GetPreRegProductId("XXX", 1, 1);
      ///List<int> RegistrationProductIds = dotTypeInfo.GetValidRegistrationProductIdList(1, regLengths.ToArray());
      ///List<int> TransferProductIds = dotTypeInfo.GetValidTransferProductIdList(1, regLengths.ToArray());
      //List<int> renewProductIds = dotTypeInfo.GetValidRenewalProductIdList(1, regLengths.ToArray());
      Assert.IsTrue(productId != 0);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("dottypecache.config")]
    [DeploymentItem("Interop.gdDataCacheLib.dll")]
    public void GetDotTypeProductIdsCoUk()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("CO.UK");
      string registrarId = ((IMultiRegDotTypeInfo)dotType).GetRegistrarIdByPfid(67323);
      Assert.IsTrue(!string.IsNullOrEmpty(registrarId));
    }
  }
}
