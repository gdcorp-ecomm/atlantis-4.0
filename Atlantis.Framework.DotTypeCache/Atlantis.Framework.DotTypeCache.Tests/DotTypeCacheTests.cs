﻿using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
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
  [DeploymentItem("Atlantis.Framework.DotTypeCache.DotCom.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.DotOrg.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.DotCoDotUk.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.DotXxx.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.DotNet.dll")]
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
      MockHttpContext.SetMockHttpContext("default.aspx", "http://siteadmin.debug.intranet.gdg/default.aspx", string.Empty);

      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetNewShopper("832652");
    }

    [TestMethod]
    public void GetDotTypeProductIds()
    {
      int productIdDefault = DotTypeCache.GetRegistrationProductId("co.uk", 2, 1);
      int productIdFirstRegistrar = DotTypeCache.GetTransferProductId("co.uk", "1", 2, 1);
      int productIdSecondRegistrar = DotTypeCache.GetRegistrationProductId("co.uk", "2", 2, 1);
      string description = DotTypeCache.GetAdditionalInfoValue("co.uk", "Description");
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
      Assert.IsTrue(renewProductId == 13313);
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
      int productId = DotTypeCache.GetPreRegProductId("XXX", 1, 1);
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
    public void InvalidDotType()
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
      bool result = TLDMLIsAvailable("COM.AU");
      Assert.IsTrue(result);
    }

    [TestMethod]
    public void TLDMLAvailableLowerCase()
    {
      bool result = TLDMLIsAvailable("com.au");
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
      var args = new object[1] { dotType };
      return (bool)TLDMLIsAvailableMethod.Invoke(null, args);
    }

    [TestMethod]
    public void TLDMLAttributeExists()
    {
      var dotTypeAttributesDictionary = DataCache.DataCache.GetExtendedTLDData("COM");
      var dotTypeAttributes = dotTypeAttributesDictionary["COM"];
      Assert.IsTrue(dotTypeAttributes.ContainsKey(TLDMLDotTypes.TLDMLSupportedFlag));
    }

    [TestMethod]
    public void OrgStaticVsTLDMLEnabled()
    {
      Type staticDotTypesType = Assembly.GetAssembly(typeof(DotTypeCache)).GetType("Atlantis.Framework.DotTypeCache.StaticDotTypes");
      MethodInfo getStaticDotType = staticDotTypesType.GetMethod("GetDotType", BindingFlags.Static | BindingFlags.Public);
      object[] methodParms = new object[1] { "org"};
      IDotTypeInfo staticOrg = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

      IDotTypeInfo tldmlOrg = DotTypeCache.GetDotTypeInfo("org");

      Assert.AreNotEqual(staticOrg, tldmlOrg);
    }

    [TestMethod]
    public void OrgStaticVsTLDMLEnabedProductIds()
    {
      Type staticDotTypesType = Assembly.GetAssembly(typeof(DotTypeCache)).GetType("Atlantis.Framework.DotTypeCache.StaticDotTypes");
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

    [TestMethod]
    public void GetOfferedTLDFlagsForAll()
    {
      Dictionary<string, Dictionary<string, bool>> offeredTLDs = DotTypeProvider.GetTLDDataForRegistration.GetDiagnosticsOfferedTLDFlags();
      Assert.IsTrue(offeredTLDs != null);
    }

    [TestMethod]
    public void GetOfferedTLDFlagsForOrg()
    {
      Dictionary<string, Dictionary<string, bool>> offeredTLDs = DotTypeProvider.GetTLDDataForRegistration.GetDiagnosticsOfferedTLDFlags(new string[] { "org" });
      Assert.IsTrue(offeredTLDs != null);
    }

    [TestMethod]
    public void GetOfferedTLDFlagsForOrgAndCom()
    {
      Dictionary<string, Dictionary<string, bool>> offeredTLDs = DotTypeProvider.GetTLDDataForRegistration.GetDiagnosticsOfferedTLDFlags(new string[] { "org", "com" });
      Assert.IsTrue(offeredTLDs != null);
    }

    [TestMethod]
    public void GetTldIdForComAu()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("com.au");
      int tldId = dotType.TldId;
      Assert.IsTrue(tldId > 0);
    }

    [TestMethod]
    public void GetTldIdForStaticCom()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("com");
      int tldId = dotType.TldId;
      Assert.IsTrue(tldId > 0);
    }

    [TestMethod]
    public void GetTldIdForInvalidDomain()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("raj");
      int tldId = dotType.TldId;
      Assert.IsTrue(tldId == 0);
    }

    [TestMethod]
    public void GetLanguageListForComAu()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("com.au");
      Assert.IsTrue(dotType.Tld.LanguageDataList != null);
    }

    [TestMethod]
    public void GetLanguageListForStaticCom()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("org");
      Assert.IsTrue(dotType.Tld.LanguageDataList != null);
    }

    [TestMethod]
    public void GetLanguageListForInvalid()
    {
      IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo("raj");
      Assert.IsTrue(dotType.Tld == null);
    }

    [TestMethod]
    public void GetOfferedTLDsSetForAllValidFlags()
    {
      string[] t = new string[] {"availcheckstatus", "mainpricebox"};
      HashSet<string> tlds = DotTypeProvider.GetTLDDataForRegistration.GetOfferedTLDsSetForAllFlags(t);
      Assert.IsTrue(tlds.Count > 0);
    }

    [TestMethod]
    public void GetOfferedTLDsSetForAllNoFlags()
    {
      string[] t = new string[] { };
      HashSet<string> tlds = DotTypeProvider.GetTLDDataForRegistration.GetOfferedTLDsSetForAllFlags(t);
      Assert.IsTrue(tlds.Count == 0);
    }

    [TestMethod]
    public void GetOfferedTLDsSetForAllInvalidFlags()
    {
      string[] t = new string[] { "hello"};
      HashSet<string> tlds = DotTypeProvider.GetTLDDataForRegistration.GetOfferedTLDsSetForAllFlags(t);
      Assert.IsTrue(tlds.Count == 0);
    }

    [TestMethod]
    public void GetOfferedTLDsSetForAnyValidFlags()
    {
      string[] t = new string[] { "availcheckstatus", "mainpricebox" };
      HashSet<string> tlds = DotTypeProvider.GetTLDDataForRegistration.GetOfferedTLDsSetForAnyFlags(t);
      var hl = new HashSet<string>(tlds);
      Assert.IsTrue(tlds.Count > 0);
    }

    [TestMethod]
    public void FilterNonOfferedTLDsAsList()
    {
      var t = new List<string>();
      t.Add("org");
      List<string> tlds = DotTypeProvider.GetTLDDataForRegistration.FilterNonOfferedTLDs(t);
      Assert.IsTrue(tlds.Count > 0);
    }

    [TestMethod]
    public void FilterNonOfferedTLDsAsHashSet()
    {
      var t = new HashSet<string>(StringComparer.OrdinalIgnoreCase); 
      t.Add("org");
      t.Add("com");
      HashSet<string> tlds = DotTypeProvider.GetTLDDataForRegistration.FilterNonOfferedTLDs(t);
      Assert.IsTrue(tlds.Count > 0);
    }

    [TestMethod]
    public void FilterNonOfferedTLDsForInvalid()
    {
      var t = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
      t.Add("raj");
      HashSet<string> tlds = DotTypeProvider.GetTLDDataForRegistration.FilterNonOfferedTLDs(t);
      Assert.IsTrue(tlds.Count == 0);
    }

    [TestMethod]
    public void IsOfferedCheckForOrg()
    {
      bool flag = DotTypeProvider.GetTLDDataForRegistration.IsOffered("org");
      Assert.IsTrue(flag);
    }

    [TestMethod]
    public void IsOfferedCheckForInvalidTld()
    {
      bool flag = DotTypeProvider.GetTLDDataForRegistration.IsOffered("raj");
      Assert.IsFalse(flag);
    }
  }
}
