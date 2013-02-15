using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.TLDDataCache.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using TestSetUpAndSettings;

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
  public class StaticVsTLDMLEnabedTests
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
      HttpProviderContainer.Instance.RegisterProvider<ISiteContext, MockSiteContext>();
      HttpProviderContainer.Instance.RegisterProvider<IShopperContext, MockShopperContext>();
      HttpProviderContainer.Instance.RegisterProvider<IManagerContext, MockNoManagerContext>();
      HttpProviderContainer.Instance.RegisterProvider<IDotTypeProvider, DotTypeProvider>();
      MockHttpContext.SetMockHttpContext("default.aspx", "http://siteadmin.debug.intranet.gdg/default.aspx", string.Empty);
      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetNewShopper("832652");
    }
    
    [TestMethod]
    public void StaticVsTLDMLEnabled()
    {
      List<string> tldList = new List<string>();
      tldList.Add("org"); //com.au

      foreach (string tld in tldList)
      {
        Type staticDotTypesType = Assembly.GetAssembly(typeof(DotTypeCache)).GetType("Atlantis.Framework.DotTypeCache.StaticDotTypes");
        MethodInfo getStaticDotType = staticDotTypesType.GetMethod("GetDotType", BindingFlags.Static | BindingFlags.Public);
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo tldml = DotTypeCache.GetDotTypeInfo(tld);

        AssertHelper.AddResults(staticTld.Product.ToString() == tldml.Product.ToString(), "Product did not match for " + tld);
        AssertHelper.AddResults(staticTld.TldId.ToString() == tldml.TldId.ToString(), "TldId did not match for " + tld);
        AssertHelper.AddResults(staticTld.Tld.ToString() == tldml.Tld.ToString(), "ITLDTld.Tld. did not match for " + tld);

        AssertHelper.AddResults(staticTld.DotType.ToLower() == tldml.DotType.ToLower(), "DotType static vs tldml did not match for " + tld);
        AssertHelper.AddResults(staticTld.IsMultiRegistry == tldml.IsMultiRegistry, "IsMultiRegistry did not match for " + tld);
        AssertHelper.AddResults(staticTld.MaxExpiredAuctionRegLength == tldml.MaxExpiredAuctionRegLength, "MaxExpiredAuctionRegLength did not match for " + tld);
        AssertHelper.AddResults(staticTld.MaxPreRegLength == tldml.MaxPreRegLength, "MaxPreRegLength did not match for " + tld);
        AssertHelper.AddResults(staticTld.MaxRegistrationLength == tldml.MaxRegistrationLength, "MaxRegistrationLength did not match for " + tld);
        AssertHelper.AddResults(staticTld.MaxRenewalLength == tldml.MaxRenewalLength, "MaxRenewalLength did not match for " + tld);

        //todo:fix
        if (tldml.MaxRenewalMonthsOut == null)
        {
          AssertHelper.AddResults(false,
                                 "tldml.MaxRenewalMonthsOut is null for " + tld);
        }
        else
        {
          AssertHelper.AddResults(staticTld.MaxRenewalMonthsOut == tldml.MaxRenewalMonthsOut,
                                  "MaxRenewalMonthsOut static vs tldml did not match for " + tld);
        }

        AssertHelper.AddResults(staticTld.MaxTransferLength == tldml.MaxTransferLength, "MaxTransferLength did not match for " + tld);
        AssertHelper.AddResults(staticTld.MinExpiredAuctionRegLength == tldml.MinExpiredAuctionRegLength, "MinExpiredAuctionRegLength did not match for " + tld);
        AssertHelper.AddResults(staticTld.MinPreRegLength == tldml.MinPreRegLength, "MinPreRegLength did not match for " + tld);
        AssertHelper.AddResults(staticTld.MinRegistrationLength == tldml.MinRegistrationLength, "MinRegistrationLength did not match for " + tld);
        AssertHelper.AddResults(staticTld.MinRenewalLength == tldml.MinRenewalLength, "MinRenewalLength did not match for " + tld);
        AssertHelper.AddResults(staticTld.MinTransferLength == tldml.MinTransferLength, "MinTransferLength did not match for " + tld);

        int statTld = staticTld.GetExpiredAuctionRegProductId(3, 1);
        int tldmlmethod = tldml.GetExpiredAuctionRegProductId(3, 1);
        AssertHelper.AddResults(statTld == tldmlmethod, "GetExpiredAuctionRegProductId for 3 years did not match for " + tld);

        statTld = staticTld.GetPreRegProductId(3, 1);
        tldmlmethod = tldml.GetPreRegProductId(3, 1);
        AssertHelper.AddResults(statTld == tldmlmethod, "GetPreRegProductId for 3 years did not match for " + tld);

        //todo:fix
        string statString = staticTld.GetRegistrationFieldsXml();
        //string tldmlmethodToString = tldml.GetRegistrationFieldsXml();
        AssertHelper.AddResults(statString == "blah", "GetRegistrationFieldsXml for 3 years did not match for " + tld);

        statTld = staticTld.GetRegistrationProductId(3, 1);
        tldmlmethod = tldml.GetRegistrationProductId(3, 1);
        AssertHelper.AddResults(statTld == tldmlmethod, "GetRegistrationProductId for 3 years did not match for " + tld);

        //todo:fix
        statString = staticTld.GetRegistryIdByProductId(986);
        string tldmlmethodToString = tldml.GetRegistryIdByProductId(986);
        AssertHelper.AddResults(statString == "blah", "GetRegistryIdByProductId did not match for " + tld);

        

        statTld = staticTld.GetTransferProductId(3, 1);
        tldmlmethod = tldml.GetTransferProductId(3, 1);
        AssertHelper.AddResults(statTld == tldmlmethod, "GetTransferProductId for 3 years did not match for " + tld);

        
        statTld = staticTld.GetRenewalProductId(3, 1);
        tldmlmethod = tldml.GetRenewalProductId(3, 1);
        AssertHelper.AddResults(statTld == tldmlmethod, "GetRenewalProductId for 3 years did not match for " + tld);
        
        int[] pparams = new int[] { 1, 2, 3, 4, 5, 6 };
        List<int> lengths = staticTld.GetValidExpiredAuctionRegLengths(1, pparams);
        List<int> lengthsT = tldml.GetValidExpiredAuctionRegLengths(1, pparams);
        AssertHelper.AddResults(lengths == lengthsT, "GetValidExpiredAuctionRegLengths did not match for " + tld);
        
        var statProdIdList = staticTld.GetValidExpiredAuctionRegProductIdList(1, pparams);
        var prodIdList = tldml.GetValidExpiredAuctionRegProductIdList(1, pparams);
        AssertHelper.AddResults(statProdIdList == prodIdList, "GetValidExpiredAuctionRegProductIdList did not match for " + tld);
        
        var statPreRegLengths = staticTld.GetValidPreRegLengths(2, pparams);
        var preRegLengths = tldml.GetValidPreRegLengths(1, pparams);
        AssertHelper.AddResults(statPreRegLengths == preRegLengths, "GetValidPreRegLengths did not match for " + tld);

        //todo:fix
        var statPreRegPidList = staticTld.GetValidPreRegProductIdList(1, pparams);
        var preRegPidList = tldml.GetValidPreRegProductIdList(1, pparams);
        AssertHelper.AddResults(false, "GetValidPreRegProductIdList did not match for " + tld);
        
        //todo:fix
        var statValidPreRegLengths = staticTld.GetValidRegistrationLengths(1, pparams);
        var validPreRegLengths = tldml.GetValidRegistrationLengths(1, pparams);
        AssertHelper.AddResults(false, "GetValidRegistrationLengths did not match for " + tld);
        
        var statValidRegistrationProductIdList = staticTld.GetValidRegistrationProductIdList(1, pparams);
        var validRegistrationProductIdList = tldml.GetValidRegistrationProductIdList(1, pparams);
        AssertHelper.AddResults(statValidRegistrationProductIdList == validRegistrationProductIdList, "GetValidRegistrationProductIdList did not match for " + tld);

        //todo:fix
        var statValidRenewalLengths = staticTld.GetValidRenewalLengths(1, pparams);
        var validRenewalLengths = tldml.GetValidRenewalLengths(1, pparams);
        int count = 0;
        foreach (var statValidRenewalLength in statValidRenewalLengths)
        {
          AssertHelper.AddResults(statValidRenewalLength == validRenewalLengths[count], "GetValidRenewalLengths did not match for " + tld);
          count++;
        }

        //todo:fix
        var statValidRenewalProductIdList = staticTld.GetValidRenewalProductIdList(1, pparams);
        var validRenewalProductIdList = tldml.GetValidRenewalProductIdList(1, pparams);
        AssertHelper.AddResults(statValidRenewalProductIdList == validRenewalProductIdList, "GetValidRenewalProductIdList did not match for " + tld);

        //todo:fix
        var statValidTransferLengths = staticTld.GetValidTransferLengths(1, pparams);
        var validTransferLengths = tldml.GetValidTransferLengths(1, pparams);
        AssertHelper.AddResults(statValidTransferLengths == validTransferLengths, "GetValidTransferLengths did not match for " + tld);
        
        //todo:fix
        var statValidTransferProductIdList = staticTld.GetValidTransferProductIdList(1, pparams);
        var validTransferProductIdList = tldml.GetValidTransferProductIdList(1, pparams);
        AssertHelper.AddResults(statValidTransferProductIdList == validTransferProductIdList, "GetValidTransferLengths did not match for " + tld);
      }
    }



  }
}
