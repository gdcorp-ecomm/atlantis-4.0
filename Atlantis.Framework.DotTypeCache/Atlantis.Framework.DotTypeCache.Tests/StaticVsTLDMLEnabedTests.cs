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
  [DeploymentItem("Atlantis.Framework.DotTypeCache.DotComDotAu.dll")]
  public class StaticVsTLDMLEnabledTests
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
      MockHttpRequest request = new MockHttpRequest("http://siteadmin.debug.intranet.gdg/default.aspx");
      MockHttpContext.SetFromWorkerRequest(request);
      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetNewShopper("832652");
    }

    [TestCleanup()]
    public void MyTestCleanup()
    {
      Console.WriteLine("Assertions: " + AssertHelper.AssertCount.ToString());
      AssertHelper.GetResults();
    }

    [TestMethod]
    public void StaticVsTLDMLEnabled()
    {
      List<string> tldList = new List<string>();
      tldList.Add("org");
      //tldList.Add("com.au");

      foreach (string tld in tldList)
      {
        Type staticDotTypesType = Assembly.GetAssembly(typeof(DotTypeCache)).GetType("Atlantis.Framework.DotTypeCache.StaticDotTypes");
        MethodInfo getStaticDotType = staticDotTypesType.GetMethod("GetDotType", BindingFlags.Static | BindingFlags.Public);
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo tldml = DotTypeCache.GetDotTypeInfo(tld);

        AssertHelper.AddResults(staticTld.Product.ExpiredAuctionsYears.Max == tldml.Product.ExpiredAuctionsYears.Max,
          "Product.ExpiredAuctionsYears.Max did not match for " + tld + ". Static: " + staticTld.Product.ExpiredAuctionsYears.Max + ". Tldml Enabled: " + tldml.Product.ExpiredAuctionsYears.Max);

        AssertHelper.AddResults(staticTld.Product.ExpiredAuctionsYears.Min == tldml.Product.ExpiredAuctionsYears.Min,
          "Product.ExpiredAuctionsYears.Min did not match for " + tld + ". Static: " + staticTld.Product.ExpiredAuctionsYears.Min + ". Tldml Enabled: " + tldml.Product.ExpiredAuctionsYears.Min);

        AssertHelper.AddResults(staticTld.Product.ExpiredAuctionsYears.IsValid(1) == tldml.Product.ExpiredAuctionsYears.IsValid(1),
          "Product.ExpiredAuctionsYears.IsValid did not match for " + tld + ". Static: " + staticTld.Product.ExpiredAuctionsYears.IsValid(1) + ". Tldml Enabled: " + tldml.Product.ExpiredAuctionsYears.IsValid(1));

        AssertHelper.AddResults(staticTld.Product.RegistrationYears.Max == tldml.Product.RegistrationYears.Max,
          "Product.RegistrationYears.Max did not match for " + tld + ". Static: " + staticTld.Product.RegistrationYears.Max + ". Tldml Enabled: " + tldml.Product.RegistrationYears.Max);

        AssertHelper.AddResults(staticTld.Product.RegistrationYears.Min == tldml.Product.RegistrationYears.Min,
          "Product.RegistrationYears.Min did not match for " + tld + ". Static: " + staticTld.Product.RegistrationYears.Min + ". Tldml Enabled: " + tldml.Product.RegistrationYears.Min);

        AssertHelper.AddResults(staticTld.Product.RegistrationYears.IsValid(1) == tldml.Product.RegistrationYears.IsValid(1),
          "Product.RegistrationYears.IsValid(1) did not match for " + tld + ". Static: " + staticTld.Product.RegistrationYears.IsValid(1) + ". Tldml Enabled: " + tldml.Product.RegistrationYears.IsValid(1));

        AssertHelper.AddResults(staticTld.Product.RenewalYears.Max == tldml.Product.RenewalYears.Max,
          "Product.RenewalYears.Max did not match for " + tld + ". Static: " + staticTld.Product.RenewalYears.Max + ". Tldml Enabled: " + tldml.Product.RenewalYears.Max);

        AssertHelper.AddResults(staticTld.Product.RenewalYears.Min == tldml.Product.RenewalYears.Min,
          "Product.RenewalYears.Min did not match for " + tld + ". Static: " + staticTld.Product.RenewalYears.Min + ". Tldml Enabled: " + tldml.Product.RenewalYears.Min);

        AssertHelper.AddResults(staticTld.Product.RenewalYears.IsValid(1) == tldml.Product.RenewalYears.IsValid(1),
          "Product.RenewalYears.IsValid(1) did not match for " + tld + ". Static: " + staticTld.Product.RenewalYears.IsValid(1) + ". Tldml Enabled: " + tldml.Product.RenewalYears.IsValid(1));

        AssertHelper.AddResults(staticTld.Product.TransferYears.Max == tldml.Product.TransferYears.Max,
          "Product.TransferYears.Max did not match for " + tld + ". Static: " + staticTld.Product.TransferYears.Max + ". Tldml Enabled: " + tldml.Product.TransferYears.Max);

        AssertHelper.AddResults(staticTld.Product.TransferYears.Min == tldml.Product.TransferYears.Min,
          "Product.TransferYears.Min did not match for " + tld + ". Static: " + staticTld.Product.TransferYears.Min + ". Tldml Enabled: " + tldml.Product.TransferYears.Min);

        AssertHelper.AddResults(staticTld.Product.TransferYears.IsValid(1) == tldml.Product.TransferYears.IsValid(1),
          "Product.TransferYears.IsValid(1) did not match for " + tld + ". Static: " + staticTld.Product.TransferYears.IsValid(1) + ". Tldml Enabled: " + tldml.Product.TransferYears.IsValid(1));

        AssertHelper.AddResults(staticTld.Product.PreregistrationYears("testing").Max == tldml.Product.PreregistrationYears("testing").Max,
          "Product.PreregistrationYears.Max did not match for " + tld + ". Static: " + staticTld.Product.PreregistrationYears("testing").Max +
          ". Tldml Enabled: " + tldml.Product.PreregistrationYears("testing").Max);

        AssertHelper.AddResults(staticTld.Product.PreregistrationYears("testing").Min == tldml.Product.PreregistrationYears("testing").Min,
          "Product.PreregistrationYears.Min did not match for " + tld + ". Static: " + staticTld.Product.PreregistrationYears("testing").Min +
          ". Tldml Enabled: " + tldml.Product.PreregistrationYears("testing").Min);

        AssertHelper.AddResults(staticTld.Product.PreregistrationYears("testing").IsValid(1) == tldml.Product.PreregistrationYears("testing").IsValid(1),
          "Product.PreregistrationYears.IsValid(1) did not match for " + tld + ". Static: " + staticTld.Product.PreregistrationYears("testing").IsValid(1) +
          ". Tldml Enabled: " + tldml.Product.PreregistrationYears("testing").IsValid(1));

        AssertHelper.AddResults(staticTld.TldId.ToString() == tldml.TldId.ToString(), "TldId did not match for " + tld +
          ". Static: " + staticTld.TldId + ". Tldml Enabled: " + tldml.TldId);

        foreach (var staticLang in staticTld.Tld.LanguageDataList)
        {
          bool containsLang = false;
          bool containsRegTag = false;

          foreach (var tldmlLang in tldml.Tld.LanguageDataList)
          {

            if (tldmlLang.LanguageName == staticLang.LanguageName)
            {
              containsLang = true;
            }

            if (tldmlLang.RegistryTag == staticLang.RegistryTag)
            {
              containsRegTag = true;
            }
          }

          if (!containsLang)
          {
            AssertHelper.AddResults(false, "LanguageName did not match for " + tld);
          }

          if (!containsRegTag)
          {
            AssertHelper.AddResults(false, "RegistryTag did not match for " + tld);
          }

        }

        AssertHelper.AddResults(staticTld.DotType.ToLower() == tldml.DotType.ToLower(), "DotType static vs tldml did not match for " + tld + ". Static: "
          + staticTld.DotType.ToLower() + ". Tldml Enabled: " + tldml.DotType.ToLower());

        AssertHelper.AddResults(staticTld.IsMultiRegistry == tldml.IsMultiRegistry, "IsMultiRegistry did not match for " + tld + ". Static: "
          + staticTld.IsMultiRegistry + ". Tldml Enabled: " + tldml.IsMultiRegistry);

        AssertHelper.AddResults(staticTld.MaxExpiredAuctionRegLength == tldml.MaxExpiredAuctionRegLength, "MaxExpiredAuctionRegLength did not match for " + tld + ". Static: "
          + staticTld.MaxExpiredAuctionRegLength + ". Tldml Enabled: " + tldml.MaxExpiredAuctionRegLength);

        AssertHelper.AddResults(staticTld.MaxPreRegLength == tldml.MaxPreRegLength, "MaxPreRegLength did not match for " + tld + ". Static: "
          + staticTld.MaxPreRegLength + ". Tldml Enabled: " + tldml.MaxPreRegLength);

        AssertHelper.AddResults(staticTld.MaxRegistrationLength == tldml.MaxRegistrationLength, "MaxRegistrationLength did not match for " + tld + ". Static: "
          + staticTld.MaxRegistrationLength + ". Tldml Enabled: " + tldml.MaxRegistrationLength);

        AssertHelper.AddResults(staticTld.MaxRenewalLength == tldml.MaxRenewalLength, "MaxRenewalLength did not match for " + tld + ". Static: "
          + staticTld.MaxRenewalLength + ". Tldml Enabled: " + tldml.MaxRenewalLength);

        //AssertHelper.AddResults(staticTld.MaxRenewalMonthsOut == tldml.MaxRenewalMonthsOut, "MaxRenewalMonthsOut static vs tldml did not match for " + tld);
        AssertHelper.AddResults(false, "MaxRenewalMonthsOut - property not implemented for " + tld);

        AssertHelper.AddResults(staticTld.MaxTransferLength == tldml.MaxTransferLength, "MaxTransferLength did not match for " + tld + ". Static: "
          + staticTld.MaxTransferLength + ". Tldml Enabled: " + tldml.MaxTransferLength);

        AssertHelper.AddResults(staticTld.MinExpiredAuctionRegLength == tldml.MinExpiredAuctionRegLength, "MinExpiredAuctionRegLength did not match for " + tld + ". Static: "
          + staticTld.MinExpiredAuctionRegLength + ". Tldml Enabled: " + tldml.MinExpiredAuctionRegLength);

        AssertHelper.AddResults(staticTld.MinPreRegLength == tldml.MinPreRegLength, "MinPreRegLength did not match for " + tld + ". Static: "
          + staticTld.MinPreRegLength + ". Tldml Enabled: " + tldml.MinPreRegLength);


        AssertHelper.AddResults(staticTld.MinRegistrationLength == tldml.MinRegistrationLength, "MinRegistrationLength did not match for " + tld + ". Static: "
          + staticTld.MinRegistrationLength + ". Tldml Enabled: " + tldml.MinRegistrationLength);

        AssertHelper.AddResults(staticTld.MinRenewalLength == tldml.MinRenewalLength, "MinRenewalLength did not match for " + tld + ". Static: "
          + staticTld.MinRenewalLength + ". Tldml Enabled: " + tldml.MinRenewalLength);

        AssertHelper.AddResults(staticTld.MinTransferLength == tldml.MinTransferLength, "MinTransferLength did not match for " + tld + ". Static: "
          + staticTld.MinTransferLength + ". Tldml Enabled: " + tldml.MinTransferLength);

        int statTld = 0;
        int tldmlmethod = 0;

        int[] domainCount = new int[] { 1, 6, 21, 50, 101, 201 };

        foreach (int dc in domainCount)
        {
          for (int regLength = staticTld.Product.RegistrationYears.Min;
               regLength <= staticTld.Product.RegistrationYears.Max;
               regLength++)
          {
            statTld = staticTld.GetExpiredAuctionRegProductId(regLength, dc);
            tldmlmethod = tldml.GetExpiredAuctionRegProductId(regLength, dc);
            AssertHelper.AddResults(statTld == tldmlmethod,
                                    "GetExpiredAuctionRegProductId for reg length: " + regLength + " year(s) and for domain count: " + dc + " did not match for " + tld + ". Static: "
                                    + statTld + ". Tldml Enabled: " + tldmlmethod);
          }
        }

        //TODO: Implement foreach when GetPreRegProductId is fixed
        //foreach (int dc in domainCount)
        //{
        //  for (int regLength = staticTld.Product.RegistrationYears.Min;
        //       regLength <= staticTld.Product.RegistrationYears.Max;
        //       regLength++)
        //  {
        //    statTld = staticTld.GetPreRegProductId(regLength, dc);
        //    tldmlmethod = tldml.GetPreRegProductId(regLength, dc);
        //    AssertHelper.AddResults(statTld == tldmlmethod,
        //                            "GetPreRegProductId for reg length: " + regLength + " year(s) and for domain count: " + dc + " did not match for " + tld + ". Static: "
        //                            + statTld + ". Tldml Enabled: " + tldmlmethod);
        //  }
        //}
        AssertHelper.AddResults(false, "GetPreRegProductId not working. Not getting a value here for GetPreRegProductId... Getting 0 for both static and tldml enabled.");

        //string statRegistrationFieldsXml = staticTld.GetRegistrationFieldsXml();
        //string registrationFieldsXml = tldml.GetRegistrationFieldsXml();
        //AssertHelper.AddResults(statRegistrationFieldsXml == registrationFieldsXml, "GetRegistrationFieldsXml for 3 years did not match for " + tld);
        AssertHelper.AddResults(false, "GetRegistrationFieldsXml() property not implemented for" + tld);

        foreach (int dc in domainCount)
        {
          for (int regLength = staticTld.Product.RegistrationYears.Min;
               regLength <= staticTld.Product.RegistrationYears.Max;
               regLength++)
          {
            statTld = staticTld.GetRegistrationProductId(regLength, dc);
            tldmlmethod = tldml.GetRegistrationProductId(regLength, dc);
            AssertHelper.AddResults(statTld == tldmlmethod,
                                    "GetRegistrationProductId for reg length: " + regLength + " year(s) and for domain count: " + dc + " did not match for " + tld + ". Static: "
                                    + statTld + ". Tldml Enabled: " + tldmlmethod);
          }
        }

        string statRegistryIdByProductId = staticTld.GetRegistryIdByProductId(986);
        string registryIdByProductId = tldml.GetRegistryIdByProductId(986);
        AssertHelper.AddResults(statRegistryIdByProductId == registryIdByProductId, "GetRegistryIdByProductId(986) did not match for " + tld + ". Static: "
          + statRegistryIdByProductId + ". Tldml Enabled: " + registryIdByProductId);

        foreach (int dc in domainCount)
        {
          for (int regLength = staticTld.Product.RegistrationYears.Min;
               regLength <= staticTld.Product.RegistrationYears.Max;
               regLength++)
          {
            statTld = staticTld.GetTransferProductId(regLength, dc);
            tldmlmethod = tldml.GetTransferProductId(regLength, dc);
            AssertHelper.AddResults(statTld == tldmlmethod,
                                    "GetTransferProductId for reg length: " + regLength +
                                    " year(s) and for domain count: " + dc + " did not match for " + tld + ". Static: "
                                    + statTld + ". Tldml Enabled: " + tldmlmethod);
          }
        }

        foreach (int dc in domainCount)
        {
          for (int regLength = staticTld.Product.RegistrationYears.Min;
               regLength <= staticTld.Product.RegistrationYears.Max;
               regLength++)
          {
            statTld = staticTld.GetRenewalProductId(regLength, dc);
            tldmlmethod = tldml.GetRenewalProductId(regLength, dc);
            AssertHelper.AddResults(statTld == tldmlmethod,
                                    "GetRenewalProductId for reg length: " + regLength +
                                    " year(s) and for domain count: " + dc + " did not match for " + tld + ". Static: "
                                    + statTld + ". Tldml Enabled: " + tldmlmethod);
          }
        }

        int[] regLengths = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        List<int> lengthsStatic = null;
        List<int> lengthsTdml = null;
        int count = 0;

        //TODO: Implement foreach when GetValidExpiredAuctionRegLengths is fixed
        //foreach (int dc in domainCount)
        //{
        //  lengthsStatic = staticTld.GetValidExpiredAuctionRegLengths(dc, regLengths);
        //  lengthsTdml = tldml.GetValidExpiredAuctionRegLengths(dc, regLengths);

        //  count = 0;

        //  foreach (var regLength in lengthsStatic)
        //  {
        //    AssertHelper.AddResults(regLength == lengthsTdml[count],
        //                            "GetValidExpiredAuctionRegLengths for reg length: " + regLength +
        //                            " year(s) and for domain count: " + dc + " did not match for " + tld +
        //                            ". Static: "
        //                            + regLength + ". Tldml Enabled: " + lengthsTdml[count]);
        //    count++;
        //  }
        //}
        
        //List<int> lengths = staticTld.GetValidExpiredAuctionRegLengths(1, regLengths);
        //List<int> lengthsT = tldml.GetValidExpiredAuctionRegLengths(1, regLengths);
        AssertHelper.AddResults(false, "GetValidExpiredAuctionRegLengths did not match for " + tld + ". Not getting anything back for tldml enabled.");

        //TODO: Implement foreach when GetValidExpiredAuctionRegProductIdList is fixed
        //foreach (int dc in domainCount)
        //{
        //  List<int> staticValidExpiredAuctionRegProductIdList = staticTld.GetValidExpiredAuctionRegProductIdList(dc, regLengths);
        //  List<int> tldmlValidExpiredAuctionRegProductIdList = tldml.GetValidExpiredAuctionRegProductIdList(dc, regLengths);

        //  count = 0;

        //  foreach (var pid in staticValidExpiredAuctionRegProductIdList)
        //  {
        //    AssertHelper.AddResults(pid == tldmlValidExpiredAuctionRegProductIdList[count],
        //                            "GetValidExpiredAuctionRegProductIdList PID for domain count: " + dc + " did not match for " + tld +
        //                            ". Static: " + pid + ". Tldml Enabled: " + tldmlValidExpiredAuctionRegProductIdList[count]);
        //    count++;
        //  }
        //}

        //var statProdIdList = staticTld.GetValidExpiredAuctionRegProductIdList(1, regLengths);
        //var prodIdList = tldml.GetValidExpiredAuctionRegProductIdList(1, regLengths);
        AssertHelper.AddResults(false, "GetValidExpiredAuctionRegProductIdList did not match for " + tld + ". Not getting anything back for tldml enabled.");

        //TODO: Implement foreach when GetValidPreRegLengths is fixed
        var statPreRegLengths = staticTld.GetValidPreRegLengths(1, regLengths);
        var preRegLengths = tldml.GetValidPreRegLengths(1, regLengths);
        AssertHelper.AddResults(statPreRegLengths == preRegLengths, "GetValidPreRegLengths calls return a count of zero for static and tldml enabled for " + tld);

        //TODO: Implement foreach when GetValidPreRegProductIdList is fixed
        var statPreRegPidList = staticTld.GetValidPreRegProductIdList(1, regLengths);
        var preRegPidList = tldml.GetValidPreRegProductIdList(1, regLengths);
        AssertHelper.AddResults(false, "GetValidPreRegProductIdList calls return a count of zero for static and tldml enabled for " + tld);

        //todo:fix
        var statValidPreRegLengths = staticTld.GetValidRegistrationLengths(1, regLengths);
        var validPreRegLengths = tldml.GetValidRegistrationLengths(1, regLengths);

        count = 0;
        foreach (var statValidPreRegLength in statValidPreRegLengths)
        {
          AssertHelper.AddResults(statValidPreRegLength == validPreRegLengths[count], "GetValidRegistrationLengths did not match for " + tld + ". Static: "
         + statValidPreRegLength + ". Tldml Enabled: " + validPreRegLengths[count]);
          count++;
        }
        
        var statValidRegistrationProductIdList = staticTld.GetValidRegistrationProductIdList(1, regLengths);
        var validRegistrationProductIdList = tldml.GetValidRegistrationProductIdList(1, regLengths);

        count = 0;
        foreach (var pid in statValidRegistrationProductIdList)
        {
          AssertHelper.AddResults(pid == validRegistrationProductIdList[count], "GetValidRegistrationProductIdList did not match for " + tld + ". Static: "
         + pid + ". Tldml Enabled: " + validRegistrationProductIdList[count]);
          count++;
        }

        var statValidRenewalLengths = staticTld.GetValidRenewalLengths(1, regLengths);
        var validRenewalLengths = tldml.GetValidRenewalLengths(1, regLengths);

        //count = 0;
        //foreach (var staticRenewalLength in statValidRenewalLengths)
        //{
        //  AssertHelper.AddResults(staticRenewalLength == validRenewalLengths[count], "GetValidRenewalLengths did not match for " + tld + ". Static: "
        // + staticRenewalLength + ". Tldml Enabled: " + validRenewalLengths[count]);
        //  count++;
        //}
        AssertHelper.AddResults(false, "There is no count for GetValidRenewalLengths for tldml enabled for " + tld);

        var statValidRenewalProductIdList = staticTld.GetValidRenewalProductIdList(1, regLengths);
        var validRenewalProductIdList = tldml.GetValidRenewalProductIdList(1, regLengths);
        AssertHelper.AddResults(false, "There is not count for GetValidRenewalProductIdList for tldml enabled for " + tld);

        //todo:fix
        var statValidTransferLengths = staticTld.GetValidTransferLengths(1, regLengths);
        var validTransferLengths = tldml.GetValidTransferLengths(1, regLengths);

        count = 0;
        foreach (var length in statValidTransferLengths)
        {
          AssertHelper.AddResults(length == validTransferLengths[count], "GetValidTransferLengths did not match for " + tld + ". Static: "
         + length + ". Tldml Enabled: " + validTransferLengths[count]);
          count++;
        }
       
        var statValidTransferProductIdList = staticTld.GetValidTransferProductIdList(1, regLengths);
        var validTransferProductIdList = tldml.GetValidTransferProductIdList(1, regLengths);

        count = 0;
        foreach (var transferPid in statValidTransferProductIdList)
        {
          AssertHelper.AddResults(transferPid == validTransferProductIdList[count], "GetValidTransferLengths did not match for " + tld + ". Static: "
         + transferPid + ". Tldml Enabled: " + validTransferProductIdList[count]);
          count++;
        }
      }
    }



  }
}
