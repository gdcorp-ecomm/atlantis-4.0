using System.Linq;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.DotTypeCache.Static;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Providers.TLDDataCache;
using Atlantis.Framework.Providers.TLDDataCache.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Atlantis.Framework.DotTypeCache.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("dottypecache.config")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeRegistry.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.RegDotTypeProductIds.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DCCDomainsDataCache.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.StaticTypes.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeAvailability.Impl.dll")]
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

    public IDotTypeProvider DotTypeProvider
    {
      get
      {
        return HttpProviderContainer.Instance.Resolve<IDotTypeProvider>();
      }
    }

    [TestInitialize]
    public void InitializeTests()
    {
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, MockNoManagerContext>();
      HttpProviderContainer.Instance.RegisterProvider<IDotTypeProvider, DotTypeProvider>();
      HttpProviderContainer.Instance.RegisterProvider<ITLDDataCacheProvider, TLDDataCacheProvider>();
      MockHttpRequest request = new MockHttpRequest("http://siteadmin.debug.intranet.gdg/default.aspx");
      MockHttpContext.SetFromWorkerRequest(request);

      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetNewShopper("832652");
    }

    [TestMethod]
    public void GetDotTypeProductIds()
    {
      int productIdDefault = DotTypeCache.GetRegistrationProductId("co.uk", 2, 1);
      int productIdFirstRegistrar = DotTypeCache.GetTransferProductId("co.uk", "1", 2, 1);
      int productIdSecondRegistrar = DotTypeCache.GetRegistrationProductId("co.uk", "2", 2, 1);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo("co.uk");
      Assert.IsTrue((productIdDefault * productIdSecondRegistrar) != 0);
      //Assert.IsTrue((productIdDefault * productIdFirstRegistrar * productIdSecondRegistrar) != 0);
    }

    [TestMethod]
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
      string domainTld = "CO.UK";
      IDotTypeInfo dotTypeInfo = DotTypeCache.GetDotTypeInfo(domainTld);
      List<int> RegistrationProductIds = dotTypeInfo.GetValidRegistrationProductIdList(1, regLengths.ToArray());
      List<int> TransferProductIds = dotTypeInfo.GetValidTransferProductIdList(1, regLengths.ToArray());
      List<int> renewProductIds = dotTypeInfo.GetValidRenewalProductIdList(1, regLengths.ToArray());
      List<int> renewProductIds2 = dotTypeInfo.GetValidRenewalProductIdList("1", 1, regLengths.ToArray());
      List<int> renewProductIds3 = dotTypeInfo.GetValidRenewalProductIdList("2", 1, regLengths.ToArray());
      int renewProductId = dotTypeInfo.GetRenewalProductId("1", 2, 1);
      Assert.IsTrue(renewProductId == 219656);
      Assert.IsTrue((RegistrationProductIds.Count * TransferProductIds.Count * renewProductIds.Count) != 0);
    }

    [TestMethod]
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
      int productId = DotTypeCache.GetPreRegProductId("XXX", LaunchPhases.GeneralAvailability, 1, 1);
      ///List<int> RegistrationProductIds = dotTypeInfo.GetValidRegistrationProductIdList(1, regLengths.ToArray());
      ///List<int> TransferProductIds = dotTypeInfo.GetValidTransferProductIdList(1, regLengths.ToArray());
      //List<int> renewProductIds = dotTypeInfo.GetValidRenewalProductIdList(1, regLengths.ToArray());
      Assert.IsTrue(productId != 0);
    }

    [TestMethod]
    public void GetDotTypeProductIdsCoUk()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("CO.UK");
      string registryId = dotType.GetRegistryIdByProductId(67324);
      Assert.IsTrue(!string.IsNullOrEmpty(registryId));
    }

    [TestMethod]
    public void InvalidDotTypeValid()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("MICCOBLAH");
      Assert.AreEqual(DotTypeCache.InvalidDotType, dotType);
    }

    [TestMethod]
    public void DotTypeProviderExists()
    {
      IDotTypeProvider dotTypeProvider = HttpProviderContainer.Instance.Resolve<IDotTypeProvider>();
      Assert.IsNotNull(dotTypeProvider);
    }

    [TestMethod]
    public void TLDMLAvailable()
    {
      bool result = TLDMLIsAvailable("CZ");
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void TLDMLAvailableLowerCase()
    {
      bool result = TLDMLIsAvailable("cz");
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void TLDMLNotAvailable()
    {
      bool result = TLDMLIsAvailable("NET");
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void TLDMLNotAvailableLowerCase()
    {
      bool result = TLDMLIsAvailable("net");
      Assert.IsFalse(result);
    }

    private bool TLDMLIsAvailable(string dotType)
    {
      MethodInfo TLDMLIsAvailableMethod = (typeof(TLDMLDotTypes)).GetMethod("TLDMLIsAvailable", BindingFlags.Static | BindingFlags.NonPublic);
      var args = new object[2] { dotType, HttpProviderContainer.Instance };
      return (bool)TLDMLIsAvailableMethod.Invoke(null, args);
    }

    [TestMethod]
    public void OrgStaticVsTLDMLEnabedProductIds()
    {
      Type staticDotTypesType = typeof(StaticDotTypes);
      MethodInfo getStaticDotType = staticDotTypesType.GetMethod("GetDotType", BindingFlags.Static | BindingFlags.Public);
      object[] methodParms = new object[1] { "org" };
      IDotTypeInfo staticOrg = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

      IDotTypeInfo tldmlOrg = DotTypeCache.GetDotTypeInfo("org");

      int static3yearOrg = staticOrg.GetRegistrationProductId(3, 1);
      int tldml3yearOrg = tldmlOrg.GetRegistrationProductId(3, 1);
      Assert.AreEqual(static3yearOrg, tldml3yearOrg);

      static3yearOrg = staticOrg.GetRenewalProductId(3, 1);
      tldml3yearOrg = tldmlOrg.GetRenewalProductId(3, 1);
      Assert.AreEqual(static3yearOrg, tldml3yearOrg);

      static3yearOrg = staticOrg.GetTransferProductId(3, 1);
      tldml3yearOrg = tldmlOrg.GetTransferProductId(3, 1);
      Assert.AreEqual(static3yearOrg, tldml3yearOrg);
    }

    [TestMethod]
    public void GetDotTypeProductCoUk()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("CO.UK");
      ITLDProduct product = dotType.Product;
      Assert.IsTrue(product != null);
    }

    [TestMethod]
    public void GetDotTypeProductComAu()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("COM.AU");
      ITLDProduct product = dotType.Product;
      Assert.IsTrue(product != null);
    }

    [TestMethod]
    public void GetDotTypeProductNet()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("NET");
      ITLDProduct product = dotType.Product;
      Assert.IsTrue(product != null);
    }

    [TestMethod]
    public void GetDotTypeProductInvalidDotType()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("RAJ");
      ITLDProduct product = dotType.Product;
      Assert.IsTrue(product == null);
    }

    [TestMethod]
    public void GetDotTypePreRegYearsForNet()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("NET");
      ITLDValidYearsSet vys = dotType.Product.PreregistrationYears("testing");
      Assert.IsTrue(vys.Min > 0);
    }

    #region GetDotTypeInfo

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
      TestMethod]
    public void GetDotTypeInfo()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);
      Assert.IsNotNull(info);
      //Assert.AreEqual(dotType.ToUpper(), info.DotType, "DotType does not match for : " + dotType.ToUpper());
      Assert.AreEqual(dotType.ToLower(), info.DotType.ToLower(), "DotType does not match for : " + dotType);

      /*Assert.AreEqual((string)testContextInstance.DataRow["HasPreRegIds"], info.HasPreRegIds.ToString(),
          "HasPreRegIds does not match for: " + dotType.ToUpper() + ". Expected: " +
          (string)testContextInstance.DataRow["HasPreRegIds"] + " Actual: " + info.HasPreRegIds.ToString());
      Assert.AreEqual((string)testContextInstance.DataRow["HasRegistrationIds"], info.HasRegistrationIds.ToString(),
          "HasRegistrationIds does not match for: " + dotType.ToUpper() + ". Expected: " +
          (string)testContextInstance.DataRow["HasRegistrationIds"] + " Actual: " + info.HasRegistrationIds.ToString());
      Assert.AreEqual((string)testContextInstance.DataRow["HasRenewalIds"], info.HasRenewalIds.ToString(),
          "HasRenewalIds does not match for: " + dotType.ToUpper() + ". Expected: " +
          (string)testContextInstance.DataRow["HasRenewalIds"] + " Actual: " + info.HasRenewalIds.ToString());
      Assert.AreEqual((string)testContextInstance.DataRow["HasTransferIds"], info.HasTransferIds.ToString(),
          "HasTransferIds does not match for: " + dotType.ToUpper() + ". Expected: " +
          (string)testContextInstance.DataRow["HasTransferIds"] + " Actual: " + info.HasTransferIds.ToString());
      Assert.AreEqual((string)testContextInstance.DataRow["HasExpiredAuctionIds"], info.HasExpiredAuctionRegIds.ToString(),
          "HasExpiredAuctionIds does not match for: " + dotType.ToUpper() + ". Expected: " +
          (string)testContextInstance.DataRow["HasExpiredAuctionIds"] + " Actual: " + info.HasExpiredAuctionRegIds.ToString());
       */
      Assert.AreEqual((string)testContextInstance.DataRow["MaxPreRegLength"], info.GetMaxPreRegLength(LaunchPhases.GeneralAvailability).ToString(),
          "MaxPreRegLength does not match for: " + dotType.ToUpper() + ". Expected: " +
          (string)testContextInstance.DataRow["MaxPreRegLength"] + " Actual: " + info.GetMaxPreRegLength(LaunchPhases.GeneralAvailability).ToString());
      Assert.AreEqual((string)testContextInstance.DataRow["MaxExpAuctionRegLength"], info.MaxExpiredAuctionRegLength.ToString(),
          "MaxExpAuctionRegLength does not match for: " + dotType.ToUpper() + ". Expected: " +
          (string)testContextInstance.DataRow["MaxExpAuctionRegLength"] + " Actual: " + info.MaxExpiredAuctionRegLength.ToString());
      Assert.AreEqual((string)testContextInstance.DataRow["MaxRegistrationLength"], info.MaxRegistrationLength.ToString(),
          "MaxRegistrationLength does not match for: " + dotType.ToUpper() + ". Expected: " +
          (string)testContextInstance.DataRow["MaxRegistrationLength"] + " Actual: " + info.MaxRegistrationLength.ToString());
      Assert.AreEqual((string)testContextInstance.DataRow["MaxRenewalLength"], info.MaxRenewalLength.ToString(),
          "MaxRenewalLength does not match for: " + dotType.ToUpper() + ". Expected: " +
          (string)testContextInstance.DataRow["MaxRenewalLength"] + " Actual: " + info.MaxRenewalLength.ToString());
      Assert.AreEqual((string)testContextInstance.DataRow["MaxTransferLength"], info.MaxTransferLength.ToString(),
          "MaxTransferLength does not match for: " + dotType.ToUpper() + ". Expected: " +
          (string)testContextInstance.DataRow["MaxTransferLength"] + " Actual: " + info.MaxTransferLength.ToString());
      Assert.AreEqual((string)testContextInstance.DataRow["MinPreRegLength"], info.GetMinPreRegLength(LaunchPhases.GeneralAvailability).ToString(),
          "MinPreRegLength does not match for: " + dotType.ToUpper() + ". Expected: " +
          (string)testContextInstance.DataRow["MinPreRegLength"] + " Actual: " + info.GetMinPreRegLength(LaunchPhases.GeneralAvailability).ToString());
      Assert.AreEqual((string)testContextInstance.DataRow["MinExpAuctionRegLength"], info.MinExpiredAuctionRegLength.ToString(),
         "MinExpAuctionRegLength does not match for: " + dotType.ToUpper() + ". Expected: " +
         (string)testContextInstance.DataRow["MinExpAuctionRegLength"] + " Actual: " + info.MinExpiredAuctionRegLength.ToString());
      Assert.AreEqual((string)testContextInstance.DataRow["MinRegistrationLength"], info.MinRegistrationLength.ToString(),
          "MinRegistrationLength does not match for: " + dotType.ToUpper() + ". Expected: " +
          (string)testContextInstance.DataRow["MinRegistrationLength"] + " Actual: " + info.MinRegistrationLength.ToString());
      Assert.AreEqual((string)testContextInstance.DataRow["MinRenewalLength"], info.MinRenewalLength.ToString(),
          "MinRenewalLength does not match for: " + dotType.ToUpper() + ". Expected: " +
          (string)testContextInstance.DataRow["MinRenewalLength"] + " Actual: " + info.MinRenewalLength.ToString());
      Assert.AreEqual((string)testContextInstance.DataRow["MinTransferLength"], info.MinTransferLength.ToString(),
          "MinTransferLength does not match for: " + dotType.ToUpper() + ". Expected: " +
          (string)testContextInstance.DataRow["MinTransferLength"] + " Actual: " + info.MinTransferLength.ToString());
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetDotTypeInfo_Empty()
    {
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(string.Empty);
      Assert.AreEqual("INVALID", info.DotType);
    }

    [TestMethod]
    public void N_GetDotTypeInfo_Invalid()
    {
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo("blah");
      Assert.AreEqual("INVALID", info.DotType);
    }

    [TestMethod]
    public void N_GetDotTypeInfo_Null()
    {
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(null);
      Assert.AreEqual("INVALID", info.DotType);
    }

    #endregion

    #endregion

    #region HasDotTypeInfo

    [DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
      DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
          DataAccessMethod.Sequential), TestMethod]
    public void HasDotTypeInfo()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      bool result = DotTypeCache.HasDotTypeInfo(dotType);
      Assert.IsTrue(result, "HasDotTypeInfo not returned for " + dotType);
    }

    #region Negative Tests

    [TestMethod]
    public void N_HasDotTypeInfo_Empty()
    {
      bool result = DotTypeCache.HasDotTypeInfo(string.Empty);
      Assert.IsFalse(result);
    }

    [TestMethod]
    public void N_HasDotTypeInfo_Invalid()
    {
      bool result = DotTypeCache.HasDotTypeInfo("blah");
      Assert.IsFalse(result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void N_HasDotTypeInfo_Null()
    {
      bool result = DotTypeCache.HasDotTypeInfo(null);
      Assert.IsFalse(result);
    }

    #endregion

    #endregion

    #region GetPreRegProductId

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetPreRegProductId_MinimumPreRegLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      Console.WriteLine("DotType: " + dotType);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);

      List<int> preRegList = info.GetValidPreRegProductIdList(LaunchPhases.GeneralAvailability, 1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (preRegList.Count > 0)
      {
        int productId = DotTypeCache.GetPreRegProductId(dotType, LaunchPhases.GeneralAvailability, info.GetMinPreRegLength(LaunchPhases.GeneralAvailability), 1);
        Assert.IsTrue(productId > 0, "ProductID < 0 for dotType: " + dotType);
      }
    }

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetPreRegProductId_MaximumPreRegLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);
      List<int> preRegList = info.GetValidPreRegProductIdList(LaunchPhases.GeneralAvailability, 1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (preRegList.Count > 0)
      {
        int productId = DotTypeCache.GetPreRegProductId(dotType, LaunchPhases.GeneralAvailability, info.GetMaxPreRegLength(LaunchPhases.GeneralAvailability), 1);
        Assert.IsTrue(productId > 0, "ProductID < 0 for dotType: " + dotType);
      }
    }

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetPreRegProductId_BulkProductId()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);
      List<int> preRegList = info.GetValidPreRegProductIdList(LaunchPhases.GeneralAvailability, 1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (preRegList.Count > 0)
      {
        int productId = DotTypeCache.GetPreRegProductId(dotType, LaunchPhases.GeneralAvailability, info.GetMinPreRegLength(LaunchPhases.GeneralAvailability), 100);
        Assert.IsTrue(productId > 0, "ProductID < 0 for dotType: " + dotType);

        //verify it is different than domainCount = 1
        int productId1stTier = DotTypeCache.GetPreRegProductId(dotType, LaunchPhases.GeneralAvailability, info.GetMinPreRegLength(LaunchPhases.GeneralAvailability), 1);
        Assert.AreNotEqual(productId1stTier, productId, "ProductID for bulk tiers is the same for dotType: " + dotType);
      }
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetPreRegProductId_EmptyDotType()
    {
      int productId = DotTypeCache.GetPreRegProductId(string.Empty, LaunchPhases.GeneralAvailability, 1, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetPreRegProductId_InvalidDotType()
    {
      int productId = DotTypeCache.GetPreRegProductId("blah", LaunchPhases.GeneralAvailability, 1, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetPreRegProductId_NullDotType()
    {
      int productId = DotTypeCache.GetPreRegProductId(null, LaunchPhases.GeneralAvailability, 1, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetPreRegProductId_InvalidRegistrationLength()
    {
      int productId = DotTypeCache.GetPreRegProductId("com", LaunchPhases.GeneralAvailability, 20, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetPreRegProductId_InvalidDomainCount()
    {
      int productId = DotTypeCache.GetPreRegProductId("com", LaunchPhases.GeneralAvailability, 1, 10000);
      Assert.AreEqual(0, productId);
    }

    #endregion

    #endregion

    #region GetExpiredAucctionRegistrationProductID

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetExpiredAuctionRegistrationProductId_MinimumRegistrationLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);
      List<int> expiredList = info.GetValidExpiredAuctionRegProductIdList(1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (expiredList.Count > 0)
      {
        int productId = DotTypeCache.GetExpiredAuctionRegProductId(dotType, info.MinExpiredAuctionRegLength, 1);
        Assert.IsTrue(productId > 0, "ProductID < 0 for dotType: " + dotType);
      }
    }

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetExpiredAuctionRegistrationProductId_MaximumRegistrationLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);
      List<int> expiredList = info.GetValidExpiredAuctionRegProductIdList(1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (expiredList.Count > 0)
      {
        int productId = DotTypeCache.GetExpiredAuctionRegProductId(dotType, info.MaxExpiredAuctionRegLength, 1);
        Assert.IsTrue(productId > 0, "ProductID < 0 for dotType: " + dotType);
      }
    }

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetExpiredAuctionRegistrationProductId_BulkProductID()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);
      List<int> expiredList = info.GetValidExpiredAuctionRegProductIdList(1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (expiredList.Count > 0)
      {
        int productId = DotTypeCache.GetExpiredAuctionRegProductId(dotType, info.MinExpiredAuctionRegLength, 100);
        Assert.IsTrue(productId > 0, "ProductID < 0 for dotType: " + dotType);
      }
    }

    #endregion

    #region GetRegistrationProductId

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetRegistrationProductId_MinimumRegistrationLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);
      List<int> expiredList = info.GetValidExpiredAuctionRegProductIdList(1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (expiredList.Count > 0)
      {
        int productId = DotTypeCache.GetRegistrationProductId(dotType, info.MinRegistrationLength, 1);
        Assert.IsTrue(productId > 0, "ProductID < 0 for dotType: " + dotType);
      }
    }

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetRegistrationProductId_MaximumRegistrationLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);
      List<int> expiredList = info.GetValidExpiredAuctionRegProductIdList(1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (expiredList.Count > 0)
      {
        int productId = DotTypeCache.GetRegistrationProductId(dotType, info.MaxRegistrationLength, 1);
        Assert.IsTrue(productId > 0, "ProductID < 0 for dotType: " + dotType);
      }
    }

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetRegistrationProductId_BulkProductId()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);
      List<int> expiredList = info.GetValidExpiredAuctionRegProductIdList(1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (expiredList.Count > 0)
      {
        int productId = DotTypeCache.GetRegistrationProductId(dotType, info.MinRegistrationLength, 100);
        Assert.IsTrue(productId > 0, "ProductID < 0 for dotType: " + dotType);
      }
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetRegistrationProductId_EmptyDotType()
    {
      int productId = DotTypeCache.GetRegistrationProductId(string.Empty, 1, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetRegistrationProductId_InvalidDotType()
    {
      int productId = DotTypeCache.GetRegistrationProductId("blah", 1, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetRegistrationProductId_NullDotType()
    {
      int productId = DotTypeCache.GetRegistrationProductId(null, 1, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetRegistrationProductId_InvalidRegistrationLength()
    {
      int productId = DotTypeCache.GetRegistrationProductId("com", 20, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetRegistrationProductId_LargeDomainCount()
    {
      int productId = DotTypeCache.GetRegistrationProductId("com", 1, 10000);
      Assert.AreNotEqual(0, productId);
    }

    [TestMethod]
    public void N_GetRegistrationProductId_InvalidDomainCount()
    {
      int productId = DotTypeCache.GetRegistrationProductId("com", 1, -1);
      Assert.AreNotEqual(0, productId);
    }


    #endregion

    #endregion

    #region GetTransferProductId
    /*
    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetTransferProductId_MinimumTransferLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);
      if (info.HasTransferIds)
      {
        int productId = DotTypeCache.GetTransferProductId(dotType, info.MinTransferLength, 1);
        Assert.IsTrue(productId > 0, "ProductID < 0 for dotType: " + dotType);
      }
    }

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetTransferProductId_MaximumTransferLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);
      if (info.HasTransferIds)
      {
        int productId = DotTypeCache.GetTransferProductId(dotType, info.MaxTransferLength, 1);
        Assert.IsTrue(productId > 0, "ProductID < 0 for dotType: " + dotType);
      }
    }

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetTransferProductId_BulkProductId()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);
      if (info.HasTransferIds)
      {
        int productId = DotTypeCache.GetTransferProductId(dotType, info.MinTransferLength, 100);
        Assert.IsTrue(productId > 0, "ProductID < 0 for dotType: " + dotType);
      }
    }
    */
    #region Negative Tests

    [TestMethod]
    public void N_GetTransferProductId_EmptyDotType()
    {
      int productId = DotTypeCache.GetTransferProductId(string.Empty, 1, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetTransferProductId_InvalidDotType()
    {
      int productId = DotTypeCache.GetTransferProductId("blah", 1, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetTransferProductId_NullDotType()
    {
      int productId = DotTypeCache.GetTransferProductId(null, 1, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetTransferProductId_InvalidTransferLength()
    {
      int productId = DotTypeCache.GetTransferProductId("com", 20, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetTransferProductId_LargeDomainCount()
    {
      int productId = DotTypeCache.GetTransferProductId("com", 1, 10000);
      Assert.AreNotEqual(0, productId);
    }

    #endregion

    #endregion

    #region GetRenewalProductId

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetRenewalProductId_MinimumRenewalLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);

      List<int> renewalList = info.GetValidRenewalProductIdList(1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (renewalList.Count > 0)
      {
        int productId = DotTypeCache.GetRenewalProductId(dotType, info.MinRenewalLength, 1);
        Assert.IsTrue(productId > 0, "ProductID < 0 for dotType: " + dotType);
      }
    }


    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetRenewalProductId_MaximumRenewalLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);
      List<int> renewalList = info.GetValidRenewalProductIdList(1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (renewalList.Count > 0)
      {
        int productId = DotTypeCache.GetRenewalProductId(dotType, info.MaxRenewalLength, 1);
        Assert.IsTrue(productId > 0, "ProductID < 0 for dotType: " + dotType);
      }
    }

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetRenewalProductId_BulkProductId()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);
      List<int> renewalList = info.GetValidRenewalProductIdList(1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (renewalList.Count > 0)
      {
        int productId = DotTypeCache.GetRenewalProductId(dotType, info.MinRenewalLength, 100);
        Assert.IsTrue(productId > 0, "ProductID < 0 for dotType: " + dotType);
      }
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetRenewalProductId_EmptyDotType()
    {
      int productId = DotTypeCache.GetRenewalProductId(string.Empty, 1, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetRenewalProductId_InvalidDotType()
    {
      int productId = DotTypeCache.GetRenewalProductId("blah", 1, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetRenewalProductId_NullDotType()
    {
      int productId = DotTypeCache.GetRenewalProductId(null, 1, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetRenewalProductId_InvalidRenewalLength()
    {
      int productId = DotTypeCache.GetRenewalProductId("com", 20, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetRenewalProductId_LargeDomainCount()
    {
      int productId = DotTypeCache.GetRenewalProductId("com", 1, 10000);
      Assert.AreNotEqual(0, productId);
    }

    #endregion

    #endregion

    #region GetMinPreRegLength

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetMinPreRegLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      int preRegLength = DotTypeCache.GetMinPreRegLength(dotType, LaunchPhases.GeneralAvailability);
      Assert.AreEqual((string)testContextInstance.DataRow["MinPreRegLength"], preRegLength.ToString(),
          "MinPreRegLength is not as expected for dotType: " + dotType);
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetMinPreRegLength_Empty()
    {
      int preRegLength = DotTypeCache.GetMinPreRegLength(string.Empty, LaunchPhases.GeneralAvailability);
      Assert.AreEqual(0, preRegLength);
    }

    [TestMethod]
    public void N_GetMinPreRegLength_Invalid()
    {
      int preRegLength = DotTypeCache.GetMinPreRegLength("blah", LaunchPhases.GeneralAvailability);
      Assert.AreEqual(0, preRegLength);
    }

    [TestMethod]
    public void N_GetMinPreRegLength_Null()
    {
      int preRegLength = DotTypeCache.GetMinPreRegLength(null, LaunchPhases.GeneralAvailability);
      Assert.AreEqual(0, preRegLength);
    }

    #endregion

    #endregion

    #region GetMaxPreRegLength

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetMaxPreRegLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      int preRegLength = DotTypeCache.GetMaxPreRegLength(dotType, LaunchPhases.GeneralAvailability);
      Assert.AreEqual((string)testContextInstance.DataRow["MaxPreRegLength"], preRegLength.ToString(),
          "MaxPreRegLength is not as expected for dotType: " + dotType);
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetMaxPreRegLength_Empty()
    {
      int preRegLength = DotTypeCache.GetMaxPreRegLength(string.Empty, LaunchPhases.GeneralAvailability);
      Assert.AreEqual(0, preRegLength);
    }

    [TestMethod]
    public void N_GetMaxPreRegLength_Invalid()
    {
      int preRegLength = DotTypeCache.GetMaxPreRegLength("blah", LaunchPhases.GeneralAvailability);
      Assert.AreEqual(0, preRegLength);
    }

    [TestMethod]
    public void N_GetMaxPreRegLength_Null()
    {
      int preRegLength = DotTypeCache.GetMaxPreRegLength(null, LaunchPhases.GeneralAvailability);
      Assert.AreEqual(0, preRegLength);
    }

    #endregion

    #endregion

    #region GetMinExpiredAuctionRegistrationLength

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetMinExpiredAuctionRegistrationLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);

      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);
      List<int> expiredList = info.GetValidExpiredAuctionRegProductIdList(1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (expiredList.Count > 0)
      {
        int regLength = DotTypeCache.GetMinExpiredAuctionRegLength(dotType);
        Assert.AreEqual((string)testContextInstance.DataRow["MinExpAuctionRegLength"], regLength.ToString(),
            "MinExpAuctionRegLength is not as expected for dotType: " + dotType);
      }
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetMinExpiredAuctionRegistrationLength_Empty()
    {
      int regLength = DotTypeCache.GetMinExpiredAuctionRegLength(string.Empty);
      Assert.AreEqual(1, regLength);
    }

    [TestMethod]
    public void N_GetMinExpiredAuctionRegistrationLength_Invalid()
    {
      int regLength = DotTypeCache.GetMinExpiredAuctionRegLength("blah");
      Assert.AreEqual(1, regLength);
    }

    [TestMethod]
    public void N_GetMinExpiredAuctionRegistrationLength_Null()
    {
      int regLength = DotTypeCache.GetMinExpiredAuctionRegLength(null);
      Assert.AreEqual(1, regLength);
    }

    #endregion

    #endregion

    #region GetMaxExpiredAuctionRegistrationLength

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetMaxExpiredAuctionRegistrationLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);

      IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(dotType);
      List<int> expiredList = info.GetValidExpiredAuctionRegProductIdList(1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (expiredList.Count > 0)
      {
        int regLength = DotTypeCache.GetMaxExpiredAuctionRegLength(dotType);
        Assert.AreEqual((string)testContextInstance.DataRow["MaxExpAuctionRegLength"], regLength.ToString(),
            "MaxExpAuctionRegLength is not as expected for dotType: " + dotType);
      }
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetMaxExpiredAuctionRegistrationLength_Empty()
    {
      int regLength = DotTypeCache.GetMaxExpiredAuctionRegLength(string.Empty);
      Assert.AreEqual(10, regLength);
    }

    [TestMethod]
    public void N_GetMaxExpiredAuctionRegistrationLength_Invalid()
    {
      int regLength = DotTypeCache.GetMaxExpiredAuctionRegLength("blah");
      Assert.AreEqual(10, regLength);
    }

    [TestMethod]
    public void N_GetMaxExpiredAuctionRegistrationLength_Null()
    {
      int regLength = DotTypeCache.GetMaxExpiredAuctionRegLength(null);
      Assert.AreEqual(10, regLength);
    }

    #endregion

    #endregion

    #region GetMinRegistrationLength

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetMinRegistrationLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      int regLength = DotTypeCache.GetMinRegistrationLength(dotType);
      Assert.AreEqual((string)testContextInstance.DataRow["MinRegistrationLength"], regLength.ToString(),
          "MinRegistrationLength is not as expected for dotType: " + dotType);
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetMinRegistrationLength_Empty()
    {
      int regLength = DotTypeCache.GetMinRegistrationLength(string.Empty);
      Assert.AreEqual(1, regLength);
    }

    [TestMethod]
    public void N_GetMinRegistrationLength_Invalid()
    {
      int regLength = DotTypeCache.GetMinRegistrationLength("blah");
      Assert.AreEqual(1, regLength);
    }

    [TestMethod]
    public void N_GetMinRegistrationLength_Null()
    {
      int regLength = DotTypeCache.GetMinRegistrationLength(null);
      Assert.AreEqual(1, regLength);
    }

    #endregion

    #endregion

    #region GetMaxRegistrationLength

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetMaxRegistrationLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      int regLength = DotTypeCache.GetMaxRegistrationLength(dotType);
      Assert.AreEqual((string)testContextInstance.DataRow["MaxRegistrationLength"], regLength.ToString(),
          "MaxRegistrationLength is not as expected for dotType: " + dotType);
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetMaxRegistrationLength_Empty()
    {
      int regLength = DotTypeCache.GetMaxRegistrationLength(string.Empty);
      Assert.AreEqual(10, regLength);
    }

    [TestMethod]
    public void N_GetMaxRegistrationLength_Invalid()
    {
      int regLength = DotTypeCache.GetMaxRegistrationLength("blah");
      Assert.AreEqual(10, regLength);
    }

    [TestMethod]
    public void N_GetMaxRegistrationLength_Null()
    {
      int regLength = DotTypeCache.GetMaxRegistrationLength(null);
      Assert.AreEqual(10, regLength);
    }

    #endregion

    #endregion

    #region GetMinTransferLength

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetMinTransferLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      int transferLength = DotTypeCache.GetMinTransferLength(dotType);
      Assert.AreEqual((string)testContextInstance.DataRow["MinTransferLength"], transferLength.ToString(),
          "MinTransferLength is not as expected for dotType: " + dotType);
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetMinTransferLength_Empty()
    {
      int transferLength = DotTypeCache.GetMinTransferLength(string.Empty);
      Assert.AreEqual(1, transferLength);
    }

    [TestMethod]
    public void N_GetMinTransferLength_Invalid()
    {
      int transferLength = DotTypeCache.GetMinTransferLength("blah");
      Assert.AreEqual(1, transferLength);
    }

    [TestMethod]
    public void N_GetMinTransferLength_Null()
    {
      int transferLength = DotTypeCache.GetMinTransferLength(null);
      Assert.AreEqual(1, transferLength);
    }

    #endregion

    #endregion

    #region GetMaxTransferLength

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetMaxTransferLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      int transferLength = DotTypeCache.GetMaxTransferLength(dotType);
      Assert.AreEqual((string)testContextInstance.DataRow["MaxTransferLength"], transferLength.ToString(),
          "MaxTransferLength is not as expected for dotType: " + dotType);
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetMaxTransferLength_Empty()
    {
      int transferLength = DotTypeCache.GetMaxTransferLength(string.Empty);
      Assert.AreEqual(10, transferLength);
    }

    [TestMethod]
    public void N_GetMaxTransferLength_Invalid()
    {
      int transferLength = DotTypeCache.GetMaxTransferLength("blah");
      Assert.AreEqual(10, transferLength);
    }

    [TestMethod]
    public void N_GetMaxTransferLength_Null()
    {
      int transferLength = DotTypeCache.GetMaxTransferLength(null);
      Assert.AreEqual(10, transferLength);
    }

    #endregion

    #endregion

    #region GetMinRenewalLength

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
      DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
      TestMethod]
    public void GetMinRenewalLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      int renewalLength = DotTypeCache.GetMinRenewalLength(dotType);
      Assert.AreEqual((string)testContextInstance.DataRow["MinRenewalLength"], renewalLength.ToString(),
          "MinRenewalLength is not as expected for dotType: " + dotType);
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetMinRenewalLength_Empty()
    {
      int renewalLength = DotTypeCache.GetMinRenewalLength(string.Empty);
      Assert.AreEqual(1, renewalLength);
    }

    [TestMethod]
    public void N_GetMinRenewalLength_Invalid()
    {
      int renewalLength = DotTypeCache.GetMinRenewalLength("blah");
      Assert.AreEqual(1, renewalLength);
    }

    [TestMethod]
    public void N_GetMinRenewalLength_Null()
    {
      int renewalLength = DotTypeCache.GetMinRenewalLength(null);
      Assert.AreEqual(1, renewalLength);
    }


    #endregion

    #endregion

    #region GetMaxRenewalLength

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("atlantis.config"), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
    DeploymentItem("Interop.gdDataCacheLib.dll"),
        TestMethod]
    public void GetMaxRenewalLength()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      int renewalLength = DotTypeCache.GetMaxRenewalLength(dotType);
      Assert.AreEqual((string)testContextInstance.DataRow["MaxRenewalLength"], renewalLength.ToString(),
          "MaxRenewalLength is not as expected for dotType: " + dotType);
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetMaxRenewalLength_Empty()
    {
      int renewalLength = DotTypeCache.GetMaxRenewalLength(string.Empty);
      Assert.AreEqual(10, renewalLength);
    }

    [TestMethod]
    public void N_GetMaxRenewalLength_Invalid()
    {
      int renewalLength = DotTypeCache.GetMaxRenewalLength("blah");
      Assert.AreEqual(10, renewalLength);
    }

    [TestMethod]
    public void N_GetMaxRenewalLength_Null()
    {
      int renewalLength = DotTypeCache.GetMaxRenewalLength(null);
      Assert.AreEqual(10, renewalLength);
    }

    #endregion

    #endregion

    [TestMethod]
    public void IsOfferedCheckForCom()
    {
      bool flag = DotTypeProvider.GetTLDDataForRegistration.IsOffered("com");
      Assert.IsTrue(flag);
    }

    [TestMethod]
    public void GetRegistryLanguagesTLDMLOrg()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("ORG");
      IEnumerable<RegistryLanguage> registryLanguages = dotType.RegistryLanguages;

      Assert.IsTrue(registryLanguages != null && registryLanguages.Any());
    }

    [TestMethod]
    public void GetRegistryLanguagesCom()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("COM");
      IEnumerable<RegistryLanguage> registryLanguages = dotType.RegistryLanguages;

      Assert.IsTrue(registryLanguages != null && registryLanguages.Any());
    }

    [TestMethod]
    public void GetRegistryLanguagesInvalid()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("RAJj");
      IEnumerable<RegistryLanguage> registryLanguages = dotType.RegistryLanguages;

      Assert.IsTrue(registryLanguages == null);
    }

    [TestMethod]
    public void GetRegistryLanguageByNameOrg()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("ORG");
      RegistryLanguage registryLanguage = dotType.GetLanguageByName("belarusian");

      Assert.IsTrue(registryLanguage != null);
    }

    [TestMethod]
    public void GetRegistryLanguageByIdOrg()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("ORG");
      RegistryLanguage registryLanguage = dotType.GetLanguageById(16);

      Assert.IsTrue(registryLanguage != null);
    }

    [TestMethod]
    public void CanRenewTests()
    {
      bool canRenew1, canRenew2, canRenew3, canRenew4, canRenew5;
      int maxValidRenewlength, maxValidRenewlength1, maxValidRenewlength2, maxValidRenewlength3, maxValidRenewlength4, maxValidRenewlength5;
      DateTime expDate = DateTime.Now;

      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("ORG");
      bool canRenew = dotType.CanRenew(expDate, out maxValidRenewlength);

      dotType = DotTypeCache.GetDotTypeInfo("COM.AU");
      canRenew1 = dotType.CanRenew(expDate, out maxValidRenewlength1);

      dotType = DotTypeCache.GetDotTypeInfo("COM");
      canRenew2 = dotType.CanRenew(expDate, out maxValidRenewlength2);

      dotType = DotTypeCache.GetDotTypeInfo("ASIA");
      canRenew3 = dotType.CanRenew(expDate, out maxValidRenewlength3);

      dotType = DotTypeCache.GetDotTypeInfo("AM");
      canRenew4 = dotType.CanRenew(expDate, out maxValidRenewlength4);

      dotType = DotTypeCache.GetDotTypeInfo("AT");
      canRenew5 = dotType.CanRenew(expDate, out maxValidRenewlength5);

      Assert.IsTrue((canRenew && maxValidRenewlength > 0) && (!canRenew1 && maxValidRenewlength1 <= 0) || (canRenew2 && maxValidRenewlength2 > 0) ||
                    (canRenew3 && maxValidRenewlength3 > 0) || (canRenew4 && maxValidRenewlength4 > 0) || (canRenew5 && maxValidRenewlength5 > 0));
    }

    [TestMethod]
    public void GetTLDDataForInvalidProductTypes()
    {
      ITLDDataImpl invalid = DotTypeProvider.GetTLDDataForInvalid;
      Assert.IsTrue(invalid.OfferedTLDsList.Count == 0);
    }

    [TestMethod]
    public void GetTLDDataForRegistration()
    {
      ITLDDataImpl reg = DotTypeProvider.GetTLDDataForRegistration;
      Assert.IsTrue(reg.OfferedTLDsList.Count > 0);
    }

    [TestMethod]
    public void GetTLDDataForTransfer()
    {
      ITLDDataImpl reg = DotTypeProvider.GetTLDDataForTransfer;
      Assert.IsTrue(reg.OfferedTLDsList.Count > 0);
    }

    [TestMethod]
    public void GetTLDDataForBulk()
    {
      ITLDDataImpl reg = DotTypeProvider.GetTLDDataForBulk;
      Assert.IsTrue(reg.OfferedTLDsList.Count > 0);
    }

    [TestMethod]
    public void GetTLDDataForBulkTransfer()
    {
      ITLDDataImpl reg = DotTypeProvider.GetTLDDataForBulkTransfer;
      Assert.IsTrue(reg.OfferedTLDsList.Count > 0);
    }

    [TestMethod]
    public void GetLaunchPhaseForTldmlBorg()
    {
      IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo("borg");
      ITLDLaunchPhase launchphase = dotTypeInfo.GetLaunchPhase(LaunchPhases.SunriseA);

      Assert.IsTrue(launchphase == null);
      //Assert.IsTrue(launchphase != null && !string.IsNullOrEmpty(launchphase.Type) && !string.IsNullOrEmpty(launchphase.SubType) && !string.IsNullOrEmpty(launchphase.Description));
    }

    [TestMethod]
    public void GetLaunchPhaseForInvalidTld()
    {
      IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo("raj");
      ITLDLaunchPhase launchphase = dotTypeInfo.GetLaunchPhase(LaunchPhases.SunriseA);
      Assert.IsTrue(launchphase == null);
    }

    [TestMethod]
    public void GetLaunchPhaseForStaticTld()
    {
      IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo("com");
      ITLDLaunchPhase launchphase = dotTypeInfo.GetLaunchPhase(LaunchPhases.SunriseA);
      Assert.IsTrue(launchphase == null);
    }

    [TestMethod]
    public void GetLaunchPhaseForStaticMultiRegTld()
    {
      IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo("co.uk");
      ITLDLaunchPhase launchphase = dotTypeInfo.GetLaunchPhase(LaunchPhases.SunriseA);
      Assert.IsTrue(launchphase == null);
    }

    [TestMethod]
    public void GetDotTypeLandingPageUrl()
    {
      IDotTypeInfo inValid = DotTypeProvider.GetDotTypeInfo("nowaythisiseverinthere");
      Assert.AreEqual(InvalidDotType.Instance, inValid);
      Assert.AreEqual(string.Empty, inValid.ApplicationControl.LandingPageUrl);
      Assert.AreEqual(string.Empty, inValid.ApplicationControl.DotTypeDescription);
      Assert.AreEqual(false, inValid.ApplicationControl.IsMultiRegistry);
    }

    [TestMethod]
    public void GetAllRegTLDs()
    {
      var nongtldList = new Dictionary<string, IDotTypeInfo>();
      var gtldList = new Dictionary<string, IDotTypeInfo>();

      ITLDDataImpl reg = DotTypeProvider.GetTLDDataForRegistration;
      foreach (var tldItem in reg.OfferedTLDsList)
      {
        var info = DotTypeCache.GetDotTypeInfo(tldItem);
        if (info.IsGtld)
        {
          gtldList.Add(tldItem, info);
        }
        else
        {
          nongtldList.Add(tldItem, info);
        }
      }
      Assert.IsTrue(gtldList.Count > 0); Assert.IsTrue(nongtldList.Count > 0);
    }

    [TestMethod]
    public void GetDotinfoForBorg()
    {
      IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo("e.borg");

      Assert.IsTrue(dotTypeInfo.IsGtld);
    
    }

    [TestMethod]
    public void GetDotinfoForCO()
    {
      IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo("CO");
      Assert.IsTrue(String.Equals(dotTypeInfo.ApplicationControl.DotTypeDescription, "Global, credible and recognizable!"));
    }

    [TestMethod]
    public void GetAllClientRequestLaunchPhasesFBorg()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("f.borg");

      var launchPhaseGroupCollectionActive = dotTypeInfo.GetAllLaunchPhaseGroups();

      ITLDLaunchPhaseGroup launchPhaseGroup;

      Assert.IsTrue(!launchPhaseGroupCollectionActive.TryGetGroup(LaunchPhaseGroupTypes.Sunrise, out launchPhaseGroup));
      Assert.IsTrue(!launchPhaseGroupCollectionActive.TryGetGroup(LaunchPhaseGroupTypes.Landrush, out launchPhaseGroup));

      Assert.IsTrue(launchPhaseGroupCollectionActive.TryGetGroup(LaunchPhaseGroupTypes.GeneralAvailability, out launchPhaseGroup));
      Assert.IsTrue(launchPhaseGroup.Phases.Count == 1);

      var launchPhaseGroupCollectionAll = dotTypeInfo.GetAllLaunchPhaseGroups(false);

      Assert.IsTrue(launchPhaseGroupCollectionAll.TryGetGroup(LaunchPhaseGroupTypes.Sunrise, out launchPhaseGroup));
      Assert.IsTrue(launchPhaseGroup.Phases.Count == 1);
      
      Assert.IsTrue(launchPhaseGroupCollectionAll.TryGetGroup(LaunchPhaseGroupTypes.Landrush, out launchPhaseGroup));
      Assert.IsTrue(launchPhaseGroup.Phases.Count == 1);

      Assert.IsTrue(launchPhaseGroupCollectionAll.TryGetGroup(LaunchPhaseGroupTypes.GeneralAvailability, out launchPhaseGroup));
      Assert.IsTrue(launchPhaseGroup.Phases.Count == 1);
    }

    [TestMethod]
    public void GetAllLaunchPhasesForTldml()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("l4.borg");

      var launchPhaseGroupCollectionActive = dotTypeInfo.GetAllLaunchPhaseGroups();

      ITLDLaunchPhaseGroup launchPhaseGroup;

      Assert.IsTrue(!launchPhaseGroupCollectionActive.TryGetGroup(LaunchPhaseGroupTypes.Sunrise, out launchPhaseGroup));
      Assert.IsTrue(!launchPhaseGroupCollectionActive.TryGetGroup(LaunchPhaseGroupTypes.Landrush, out launchPhaseGroup));
      
      Assert.IsTrue(launchPhaseGroupCollectionActive.TryGetGroup(LaunchPhaseGroupTypes.GeneralAvailability, out launchPhaseGroup));
      Assert.IsTrue(launchPhaseGroup.Phases.Count == 1);

      var launchPhaseGroupCollectionAll = dotTypeInfo.GetAllLaunchPhaseGroups(false);

      Assert.IsTrue(!launchPhaseGroupCollectionAll.TryGetGroup(LaunchPhaseGroupTypes.Sunrise, out launchPhaseGroup));
      Assert.IsTrue(!launchPhaseGroupCollectionAll.TryGetGroup(LaunchPhaseGroupTypes.Landrush, out launchPhaseGroup));

      Assert.IsTrue(launchPhaseGroupCollectionAll.TryGetGroup(LaunchPhaseGroupTypes.GeneralAvailability, out launchPhaseGroup));
      Assert.IsTrue(launchPhaseGroup.Phases.Count == 1);
    }

    [TestMethod]
    public void GetAllLaunchPhasesForInvalidTld()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("nate");

      var launchPhaseGroupCollection = dotTypeInfo.GetAllLaunchPhaseGroups();

      Assert.IsTrue(launchPhaseGroupCollection == null);
    }

    [TestMethod]
    public void GetAllLaunchPhasesForStaticTld()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("com");

      var launchPhaseGroupCollection = dotTypeInfo.GetAllLaunchPhaseGroups();

      ITLDLaunchPhaseGroup launchPhaseGroup;

      Assert.IsTrue(launchPhaseGroupCollection.TryGetGroup(LaunchPhaseGroupTypes.GeneralAvailability, out launchPhaseGroup));
      Assert.IsTrue(launchPhaseGroup.Phases[0].LivePeriod.IsActive);
    }

    [TestMethod]
    public void GetAllLaunchPhasesForStaticMultiRegTld()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("co.uk");

      var launchPhaseGroupCollection = dotTypeInfo.GetAllLaunchPhaseGroups();

      ITLDLaunchPhaseGroup launchPhaseGroup;

      Assert.IsTrue(launchPhaseGroupCollection.TryGetGroup(LaunchPhaseGroupTypes.GeneralAvailability, out launchPhaseGroup));
      Assert.IsTrue(launchPhaseGroup.Phases[0].LivePeriod.IsActive);
    }

    public void IsTldInLivePhase()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("l4.borg");
      
      ITLDLaunchPhase launchPhase = dotTypeInfo.GetLaunchPhase(LaunchPhases.GeneralAvailability);

      Assert.IsTrue(launchPhase.LivePeriod.IsActive);
    }

    [TestMethod]
    public void TldHasPreRegPhases()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("l4.borg");
      var hasPreregPhases = dotTypeInfo.IsPreRegPhaseActive;
      Assert.IsTrue(hasPreregPhases);
    }

    [TestMethod]
    public void TldGetProductIdStaticCom()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("com");
      
      IDomainProductLookup domainProductLookup = DomainProductLookup.Create(4, 1, LaunchPhases.GeneralAvailability, TLDProductTypes.Registration);

      int productId = dotTypeInfo.GetProductId(domainProductLookup);
      Assert.IsTrue(productId == 104);
    }

    [TestMethod]
    public void TldGetProductIdTldmlO2Borg4Years()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("o2.borg");
      
      IDomainProductLookup domainProductLookup = DomainProductLookup.Create(4, 3, LaunchPhases.GeneralAvailability, TLDProductTypes.Registration, 1);

      int productId = dotTypeInfo.GetProductId(domainProductLookup);
      Assert.IsTrue(productId == 37671);
    }

    [TestMethod]
    public void TldGetProductIdListTldmlO2Borg()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("o2.borg");
      
      IDomainProductListLookup domainProductListLookup = DomainProductListLookup.Create(new[] { 3, 6 }, 3, LaunchPhases.GeneralAvailability,TLDProductTypes.Registration, 1);
    
      var productIdList = dotTypeInfo.GetProductIdList(domainProductListLookup);
      Assert.IsTrue(productIdList.Contains(37670) && productIdList.Contains(37673));
    }

    [TestMethod]
    public void TldGetProductIdTldmlO2Borg2Years()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("o2.borg");
    
      IDomainProductLookup domainProductLookup = DomainProductLookup.Create(2, 1, LaunchPhases.GeneralAvailability, TLDProductTypes.Registration, 1);

      int productId = dotTypeInfo.GetProductId(domainProductLookup);
      Assert.IsTrue(productId == 37669);
    }

    [TestMethod]
    public void TldGetProductIdTldmlO2BorgBulkReg()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("o2.borg");
    
      IDomainProductLookup domainProductLookup = DomainProductLookup.Create(2, 7, LaunchPhases.GeneralAvailability, TLDProductTypes.Registration, 1);

      int productId = dotTypeInfo.GetProductId(domainProductLookup);
      Assert.IsTrue(productId == 37669);
    }

    [TestMethod]
    public void TldGetProductIdTldmlMenuInvalidPhase()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("reviews");

      IDomainProductLookup domainProductLookup = DomainProductLookup.Create(dotTypeInfo.MinRegistrationLength, 1, LaunchPhases.GeneralAvailability, TLDProductTypes.Registration);

      int productId = dotTypeInfo.GetProductId(domainProductLookup);
      Assert.IsTrue(productId > 0);
    }

    [TestMethod]
    public void TldGetApplicationProductIdListTldmlBuild()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("build");

      var productIdList = dotTypeInfo.GetPhaseApplicationProductIdList(LaunchPhases.Landrush);
      Assert.IsTrue(productIdList.Any());
    }

    [TestMethod]
    public void TldGetTuiFormTypesSunriseAUno()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("uno");

      var tuiFormTypes = dotTypeInfo.GetTuiFormTypes(LaunchPhases.SunriseA);
      Assert.IsTrue(tuiFormTypes != null && tuiFormTypes.Any());
    }

    [TestMethod]
    public void TldGetTuiFormTypesLandrushUno()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("uno");

      var tuiFormTypes = dotTypeInfo.GetTuiFormTypes(LaunchPhases.Landrush);

      var currentDate = DateTime.Now;
      if (currentDate >= new DateTime(2014, 4, 21) && currentDate <= new DateTime(2014, 6, 19))
      {
        Assert.IsTrue(tuiFormTypes != null && tuiFormTypes.Count > 0);
      }
      else
      {
        Assert.IsTrue(tuiFormTypes != null && tuiFormTypes.Count == 0);
      }
    }

    [TestMethod]
    public void TldGetTuiFormTypeSystemsSunRiseA()
    {
      var request = new MockHttpRequest("http://siteadmin.debug.intranet.gdg/default.aspx");
      MockHttpContext.SetFromWorkerRequest(request);

      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("systems");
      var tuiFormTypes = dotTypeInfo.GetTuiFormTypes(LaunchPhases.SunriseA);
      Assert.IsTrue(tuiFormTypes.Any());
    }

    [TestMethod]
    public void TestTrusteeRequiredTestSG()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("SG");

      Assert.IsTrue(dotTypeInfo.Product.Trustee.IsRequired);
    }

    [TestMethod]
    public void TrusteeNegativeStaticTest()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("COM");
      
      Assert.IsFalse(dotTypeInfo.Product.Trustee.IsRequired);
    }

    [TestMethod]
    public void RequiresTuiFormTestCl()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("CL");

      Assert.IsTrue(dotTypeInfo.RequiresTuiForm(LaunchPhases.GeneralAvailability));
    }

    [TestMethod]
    public void RequiresTuiFormTestSg()
    {
      var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("SG");

      Assert.IsTrue(dotTypeInfo.RequiresTuiForm(LaunchPhases.GeneralAvailability));
    }

    [TestMethod]
    public void GetDotTypeProductIdForInvalid()
    {
      int productId = DotTypeCache.GetRegistrationProductId("SUNRISE-0930", 1, 1);
      Assert.IsTrue(productId == 0);
    }

    [TestMethod]
    public void GetDotTypeLauchPhaseCodeFromLaunchPhase()
    {
      string phaseCode = LaunchPhaseMappings.GetCode(LaunchPhases.Landrush);
      Assert.IsTrue(!string.IsNullOrEmpty(phaseCode));
    }

    [TestMethod]
    public void GetTrusteeProductIdForRegistration()
    {
        var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("SG");
        List<int> trusteeProductIds = dotTypeInfo.GetTrusteeProductId(TLDProductTypes.Registration);
        Assert.IsTrue(trusteeProductIds.Any());
    }

    [TestMethod]
    public void GetTrusteeProductIdForRenewal()
    {
        var dotTypeInfo = DotTypeProvider.GetDotTypeInfo("SG");
        List<int> trusteeProductIds = dotTypeInfo.GetTrusteeProductId(TLDProductTypes.Renewal);
        Assert.IsTrue(trusteeProductIds.Any());
    }

  }
}
