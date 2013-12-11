using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.DotTypeCache.Static;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.TLDDataCache.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Automation.Framework.TestSetUpAndSettings;
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
  [DeploymentItem("Atlantis.Framework.DomainContactFields.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DotTypeCache.StaticTypes.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]
  public class StaticVsTLDMLEnabledTests
  {
    private TestContext testContextInstance;
    private List<string> tlds;
    private Type staticDotTypesType;
    private MethodInfo getStaticDotType;
    private int[] domainCount;
    private int[] regLengths;
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
      tlds = new List<string>();
      tlds.Add("org");
      domainCount = new int[] { 1, 6, 21, 50, 101, 201 };
      regLengths = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
      staticDotTypesType = typeof(StaticDotTypes);
      getStaticDotType = staticDotTypesType.GetMethod("GetDotType", BindingFlags.Static | BindingFlags.Public);
    }

    [TestCleanup()]
    public void MyTestCleanup()
    {
      Console.WriteLine("Assertions: " + AssertHelper.AssertCount.ToString());
      AssertHelper.GetResults();
    }

    [TestMethod]
    public void ExpiredAuctionsYearsStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        AssertHelper.AddResults(staticTld.Product.ExpiredAuctionsYears.Max == dotTypeCache.Product.ExpiredAuctionsYears.Max,
                                "Product.ExpiredAuctionsYears.Max did not match for " + tld + ". Static: " +
                                staticTld.Product.ExpiredAuctionsYears.Max + ". Tldml Enabled: " +
                                dotTypeCache.Product.ExpiredAuctionsYears.Max);

        AssertHelper.AddResults(staticTld.Product.ExpiredAuctionsYears.Min == dotTypeCache.Product.ExpiredAuctionsYears.Min,
                                "Product.ExpiredAuctionsYears.Min did not match for " + tld + ". Static: " +
                                staticTld.Product.ExpiredAuctionsYears.Min + ". Tldml Enabled: " +
                                dotTypeCache.Product.ExpiredAuctionsYears.Min);

        AssertHelper.AddResults(
          staticTld.Product.ExpiredAuctionsYears.IsValid(1) == dotTypeCache.Product.ExpiredAuctionsYears.IsValid(1),
          "Product.ExpiredAuctionsYears.IsValid did not match for " + tld + ". Static: " +
          staticTld.Product.ExpiredAuctionsYears.IsValid(1) + ". Tldml Enabled: " +
          dotTypeCache.Product.ExpiredAuctionsYears.IsValid(1));
      }
    }

    [TestMethod]
    public void RegistrationYearsStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        AssertHelper.AddResults(staticTld.Product.RegistrationYears.Max == dotTypeCache.Product.RegistrationYears.Max,
                                "Product.RegistrationYears.Max did not match for " + tld + ". Static: " +
                                staticTld.Product.RegistrationYears.Max + ". Tldml Enabled: " +
                                dotTypeCache.Product.RegistrationYears.Max);

        AssertHelper.AddResults(staticTld.Product.RegistrationYears.Min == dotTypeCache.Product.RegistrationYears.Min,
                                "Product.RegistrationYears.Min did not match for " + tld + ". Static: " +
                                staticTld.Product.RegistrationYears.Min + ". Tldml Enabled: " +
                                dotTypeCache.Product.RegistrationYears.Min);

        AssertHelper.AddResults(
          staticTld.Product.RegistrationYears.IsValid(1) == dotTypeCache.Product.RegistrationYears.IsValid(1),
          "Product.RegistrationYears.IsValid(1) did not match for " + tld + ". Static: " +
          staticTld.Product.RegistrationYears.IsValid(1) + ". Tldml Enabled: " +
          dotTypeCache.Product.RegistrationYears.IsValid(1));
      }
    }

    [TestMethod]
    public void RenewalYearsStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        AssertHelper.AddResults(staticTld.Product.RenewalYears.Max == dotTypeCache.Product.RenewalYears.Max,
                                "Product.RenewalYears.Max did not match for " + tld + ". Static: " +
                                staticTld.Product.RenewalYears.Max + ". Tldml Enabled: " +
                                dotTypeCache.Product.RenewalYears.Max);

        AssertHelper.AddResults(staticTld.Product.RenewalYears.Min == dotTypeCache.Product.RenewalYears.Min,
                                "Product.RenewalYears.Min did not match for " + tld + ". Static: " +
                                staticTld.Product.RenewalYears.Min + ". Tldml Enabled: " +
                                dotTypeCache.Product.RenewalYears.Min);

        AssertHelper.AddResults(staticTld.Product.RenewalYears.IsValid(1) == dotTypeCache.Product.RenewalYears.IsValid(1),
                                "Product.RenewalYears.IsValid(1) did not match for " + tld + ". Static: " +
                                staticTld.Product.RenewalYears.IsValid(1) + ". Tldml Enabled: " +
                                dotTypeCache.Product.RenewalYears.IsValid(1));
      }
    }

    [TestMethod]
    public void TransferYearsStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        AssertHelper.AddResults(staticTld.Product.TransferYears.Max == dotTypeCache.Product.TransferYears.Max,
                                "Product.TransferYears.Max did not match for " + tld + ". Static: " +
                                staticTld.Product.TransferYears.Max + ". Tldml Enabled: " +
                                dotTypeCache.Product.TransferYears.Max);

        AssertHelper.AddResults(staticTld.Product.TransferYears.Min == dotTypeCache.Product.TransferYears.Min,
                                "Product.TransferYears.Min did not match for " + tld + ". Static: " +
                                staticTld.Product.TransferYears.Min + ". Tldml Enabled: " +
                                dotTypeCache.Product.TransferYears.Min);

        AssertHelper.AddResults(staticTld.Product.TransferYears.IsValid(1) == dotTypeCache.Product.TransferYears.IsValid(1),
                                "Product.TransferYears.IsValid(1) did not match for " + tld + ". Static: " +
                                staticTld.Product.TransferYears.IsValid(1) + ". Tldml Enabled: " +
                                dotTypeCache.Product.TransferYears.IsValid(1));
      }
    }

    [TestMethod]
    public void ProductPreregistrationYearsStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        AssertHelper.AddResults(
          staticTld.Product.PreregistrationYears("GA").Max == dotTypeCache.Product.PreregistrationYears("GA").Max,
          "Product.PreregistrationYears.Max did not match for " + tld + ". Static: " +
          staticTld.Product.PreregistrationYears("GA").Max +
          ". Tldml Enabled: " + dotTypeCache.Product.PreregistrationYears("GA").Max);

        AssertHelper.AddResults(
          staticTld.Product.PreregistrationYears("GA").Min == dotTypeCache.Product.PreregistrationYears("GA").Min,
          "Product.PreregistrationYears.Min did not match for " + tld + ". Static: " +
          staticTld.Product.PreregistrationYears("GA").Min +
          ". Tldml Enabled: " + dotTypeCache.Product.PreregistrationYears("GA").Min);

        AssertHelper.AddResults(
          staticTld.Product.PreregistrationYears("GA").IsValid(1) ==
          dotTypeCache.Product.PreregistrationYears("GA").IsValid(1),
          "Product.PreregistrationYears.IsValid(1) did not match for " + tld + ". Static: " +
          staticTld.Product.PreregistrationYears("GA").IsValid(1) +
          ". Tldml Enabled: " + dotTypeCache.Product.PreregistrationYears("GA").IsValid(1));
      }
    }

    [TestMethod]
    public void DotTypeCacheStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        AssertHelper.AddResults(staticTld.TldId.ToString() == dotTypeCache.TldId.ToString(),
                                "TldId did not match for " + tld +
                                ". Static: " + staticTld.TldId + ". Tldml Enabled: " + dotTypeCache.TldId);

        AssertHelper.AddResults(staticTld.DotType.ToLower() == dotTypeCache.DotType.ToLower(),
                                "DotType static vs tldml did not match for " + tld + ". Static: "
                                + staticTld.DotType.ToLower() + ". Tldml Enabled: " + dotTypeCache.DotType.ToLower());
      }
    }

    [TestMethod]
    public void IsMultiRegistryStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        AssertHelper.AddResults(staticTld.IsMultiRegistry == dotTypeCache.IsMultiRegistry,
                                "IsMultiRegistry did not match for " + tld + ". Static: "
                                + staticTld.IsMultiRegistry + ". Tldml Enabled: " + dotTypeCache.IsMultiRegistry);
      }
    }

    //[TestMethod]
    //public void PreRegLengthStaticVsTLDMLEnabled()
    //{
    //  foreach (string tld in tlds)
    //  {
    //    object[] methodParms = new object[1] { tld };

    //    IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

    //    IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

    //    AssertHelper.AddResults(staticTld.GetMaxPreRegLength(PreRegPhases.GeneralAvailability) == dotTypeCache.GetMaxPreRegLength,
    //                            "MaxPreRegLength did not match for " + tld + ". Static: "
    //                            + staticTld.MaxPreRegLength + ". Tldml Enabled: " + dotTypeCache.MaxPreRegLength);

    //    AssertHelper.AddResults(staticTld.MinPreRegLength == dotTypeCache.MinPreRegLength,
    //                           "MinPreRegLength did not match for " + tld + ". Static: "
    //                           + staticTld.MinPreRegLength + ". Tldml Enabled: " + dotTypeCache.MinPreRegLength);
    //  }
    //}

    [TestMethod]
    public void RegistrationLengthStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        AssertHelper.AddResults(staticTld.MaxRegistrationLength == dotTypeCache.MaxRegistrationLength,
                                "MaxRegistrationLength did not match for " + tld + ". Static: "
                                + staticTld.MaxRegistrationLength + ". Tldml Enabled: " + dotTypeCache.MaxRegistrationLength);

        AssertHelper.AddResults(staticTld.MinRegistrationLength == dotTypeCache.MinRegistrationLength,
                                "MinRegistrationLength did not match for " + tld + ". Static: "
                                + staticTld.MinRegistrationLength + ". Tldml Enabled: " + dotTypeCache.MinRegistrationLength);
      }
    }

    [TestMethod]
    public void RenewalLengthStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        AssertHelper.AddResults(staticTld.MaxRenewalLength == dotTypeCache.MaxRenewalLength,
          "MaxRenewalLength did not match for " + tld + ". Static: "
          + staticTld.MaxRenewalLength + ". Tldml Enabled: " + dotTypeCache.MaxRenewalLength);

        AssertHelper.AddResults(staticTld.MinRenewalLength == dotTypeCache.MinRenewalLength,
          "MinRenewalLength did not match for " + tld + ". Static: "
         + staticTld.MinRenewalLength + ". Tldml Enabled: " + dotTypeCache.MinRenewalLength);
      }
    }

    [TestMethod]
    public void TransferLengthStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        AssertHelper.AddResults(staticTld.MaxTransferLength == dotTypeCache.MaxTransferLength,
                                "MaxTransferLength did not match for " + tld + ". Static: "
                                + staticTld.MaxTransferLength + ". Tldml Enabled: " + dotTypeCache.MaxTransferLength);

        AssertHelper.AddResults(staticTld.MinTransferLength == dotTypeCache.MinTransferLength,
                                "MinTransferLength did not match for " + tld + ". Static: "
                                + staticTld.MinTransferLength + ". Tldml Enabled: " + dotTypeCache.MinTransferLength);
      }
    }

    [
      TestMethod]
    public void ExpiredAuctionRegLengthStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        AssertHelper.AddResults(staticTld.MaxExpiredAuctionRegLength == dotTypeCache.MaxExpiredAuctionRegLength,
                                "MaxExpiredAuctionRegLength did not match for " + tld + ". Static: "
                                + staticTld.MaxExpiredAuctionRegLength + ". Tldml Enabled: " +
                                dotTypeCache.MaxExpiredAuctionRegLength);

        AssertHelper.AddResults(staticTld.MinExpiredAuctionRegLength == dotTypeCache.MinExpiredAuctionRegLength,
                                "MinExpiredAuctionRegLength did not match for " + tld + ". Static: "
                                + staticTld.MinExpiredAuctionRegLength + ". Tldml Enabled: " +
                                dotTypeCache.MinExpiredAuctionRegLength);
      }
    }

    [TestMethod]
    public void RenewProhibitedStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);


        AssertHelper.AddResults(
          staticTld.Tld.RenewProhibitedPeriodForExpiration == dotTypeCache.Tld.RenewProhibitedPeriodForExpiration,
          "RenewProhibitedPeriodForExpiration did not match for " + tld + ". Static: "
          + staticTld.Tld.RenewProhibitedPeriodForExpiration + ". Tldml Enabled: " +
          dotTypeCache.Tld.RenewProhibitedPeriodForExpiration);

        AssertHelper.AddResults(
          staticTld.Tld.RenewProhibitedPeriodForExpirationUnit == dotTypeCache.Tld.RenewProhibitedPeriodForExpirationUnit,
          "RenewProhibitedPeriodForExpirationUnit did not match for " + tld + ". Static: "
          + staticTld.Tld.RenewProhibitedPeriodForExpirationUnit + ". Tldml Enabled: " +
          dotTypeCache.Tld.RenewProhibitedPeriodForExpirationUnit);
      }
    }

    [TestMethod]
    public void CanRenewStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        int OutValueStatic = 0;
        int outValueTldml = 0;

        bool canRenewStatic = staticTld.CanRenew(DateTime.Now.AddYears(-5), out OutValueStatic);
        bool canRenewTldml = dotTypeCache.CanRenew(DateTime.Now.AddYears(-5), out outValueTldml);

        AssertHelper.AddResults(canRenewStatic == canRenewTldml,
                                "CanRenew did not match for " + tld + ". Static: " + canRenewStatic +
                                ". Out Value = " + OutValueStatic + ". Tldml Enabled: " + canRenewTldml +
                                ". Out Value = " + outValueTldml);
      }
    }

    [TestMethod]
    public void GetExpiredAuctionRegProductIdStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        int statTld = 0;
        int tldmlmethod = 0;

        foreach (int dc in domainCount)
        {
          for (int regLength = staticTld.Product.RegistrationYears.Min;
               regLength <= staticTld.Product.RegistrationYears.Max;
               regLength++)
          {
            statTld = staticTld.GetExpiredAuctionRegProductId(regLength, dc);
            tldmlmethod = dotTypeCache.GetExpiredAuctionRegProductId(regLength, dc);
            AssertHelper.AddResults(statTld == tldmlmethod,
                                    "GetExpiredAuctionRegProductId for reg length: " + regLength + " year(s) and for domain count: " + dc + " did not match for " + tld + ". Static: "
                                    + statTld + ". Tldml Enabled: " + tldmlmethod);
          }
        }

        AssertHelper.AddResults(statTld == tldmlmethod, "GetExpiredAuctionRegProductId not working for tldml enabled");
      }
    }

    //[TestMethod]
    //public void GetPreRegProductIdStaticVsTLDMLEnabled()
    //{
    //  foreach (string tld in tlds)
    //  {
    //    object[] methodParms = new object[1] { tld };

    //    IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

    //    IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

    //    int statTld = 0;
    //    int tldmlmethod = 0;

    //    foreach (int dc in domainCount)
    //    {
    //      for (int regLength = staticTld.Product.RegistrationYears.Min;
    //           regLength <= staticTld.Product.RegistrationYears.Max;
    //           regLength++)
    //      {
    //        statTld = staticTld.GetPreRegProductId(regLength, dc);
    //        tldmlmethod = dotTypeCache.GetPreRegProductId(regLength, dc);
    //        AssertHelper.AddResults(statTld == tldmlmethod && tldmlmethod != 0,
    //                                "GetPreRegProductId for reg length: " + regLength +
    //                                " year(s) and for domain count: " + dc + " did not match for or are both zero for " + tld + ". Static: "
    //                                + statTld + ". Tldml Enabled: " + tldmlmethod);
    //      }
    //    }
    //  }
    //}

    [TestMethod]
    public void GetRegistrationFieldsXmlStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        string statRegistrationFieldsXml = staticTld.GetRegistrationFieldsXml();
        string registrationFieldsXml = dotTypeCache.GetRegistrationFieldsXml();
        AssertHelper.AddResults(statRegistrationFieldsXml == registrationFieldsXml,
                                "GetRegistrationFieldsXml did not match for " + tld);
      }
    }

    [TestMethod]
    public void GetRegistrationProductIdStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        int statTld = 0;
        int tldmlmethod = 0;

        foreach (int dc in domainCount)
        {
          for (int regLength = staticTld.Product.RegistrationYears.Min;
               regLength <= staticTld.Product.RegistrationYears.Max;
               regLength++)
          {
            statTld = staticTld.GetRegistrationProductId(regLength, dc);
            tldmlmethod = dotTypeCache.GetRegistrationProductId(regLength, dc);
            AssertHelper.AddResults(statTld == tldmlmethod,
                                    "GetRegistrationProductId for reg length: " + regLength +
                                    " year(s) and for domain count: " + dc + " did not match for " + tld +
                                    ". Static: "
                                    + statTld + ". Tldml Enabled: " + tldmlmethod);
          }
        }
      }
    }
    
    [TestMethod]
    public void ProductRegistrationYearsStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        int statTld = 0;
        int tldmlmethod = 0;

        foreach (int dc in domainCount)
        {
          for (int regLength = staticTld.Product.RegistrationYears.Min;
               regLength <= staticTld.Product.RegistrationYears.Max;
               regLength++)
          {
            statTld = staticTld.GetRenewalProductId(regLength, dc);
            tldmlmethod = dotTypeCache.GetRenewalProductId(regLength, dc);
            AssertHelper.AddResults(statTld == tldmlmethod,
                                    "GetRenewalProductId for reg length: " + regLength +
                                    " year(s) and for domain count: " + dc + " did not match for " + tld +
                                    ". Static: "
                                    + statTld + ". Tldml Enabled: " + tldmlmethod);
          }
        }
      }
    }

    [TestMethod]
    public void GetValidExpiredAuctionRegLengthsStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          int count = 0;
          List<int> lengthsStatic = staticTld.GetValidExpiredAuctionRegLengths(dc, regLengths);

          foreach (int lengthsTdml in dotTypeCache.GetValidExpiredAuctionRegLengths(dc, regLengths))
          {
            AssertHelper.AddResults(lengthsTdml == lengthsStatic[count],
                                    "GetValidExpiredAuctionRegLengths for domain count: " + dc + " did not match for " + tld +
                                    ". Static: "
                                    + lengthsStatic[count] + ". Tldml Enabled: " + lengthsTdml);
            count++;
          }
        }
      }
    }

    [TestMethod]
    public void GetValidExpiredAuctionRegProductIdListStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          List<int> staticValidExpiredAuctionRegProductIdList = staticTld.GetValidExpiredAuctionRegProductIdList(dc, regLengths);

          int count = 0;

          foreach (int tldmlPid in dotTypeCache.GetValidExpiredAuctionRegProductIdList(dc, regLengths))
          {
            AssertHelper.AddResults(tldmlPid == staticValidExpiredAuctionRegProductIdList[count],
                                    "GetValidExpiredAuctionRegProductIdList PID for domain count: " + dc + " did not match for " + tld +
                                    ". Static: " + staticValidExpiredAuctionRegProductIdList[count] + ". Tldml Enabled: " + tldmlPid);
            count++;
          }
        }

        AssertHelper.AddResults(false, "GetValidExpiredAuctionRegProductIdList did not match for " + tld + ". Not getting anything back for tldml enabled.");
      }
    }

    [TestMethod]
    public void GetValidPreRegLengthsStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          int count = 0;

          List<int> statPreRegLengths = staticTld.GetValidPreRegLengths(LaunchPhases.GeneralAvailability, dc, regLengths);

          foreach (int tldmlPid in dotTypeCache.GetValidPreRegLengths(LaunchPhases.GeneralAvailability, dc, regLengths))
          {
            AssertHelper.AddResults(tldmlPid == statPreRegLengths[count],
                                    "GetValidPreRegLengths PID for domain count: " + dc + " did not match for " + tld +
                                    ". Static: " + statPreRegLengths[count] + ". Tldml Enabled: " + tldmlPid);

            count++;
          }
        }
      }
    }

    [TestMethod]
    public void GetValidPreRegProductIdListStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        //TODO: Implement foreach when GetValidPreRegProductIdList is fixed
        List<int> statPreRegPidList = staticTld.GetValidPreRegProductIdList(LaunchPhases.GeneralAvailability, 1, regLengths);
        List<int> preRegPidList = dotTypeCache.GetValidPreRegProductIdList(LaunchPhases.GeneralAvailability, 1, regLengths);
        AssertHelper.AddResults(false, "GetValidPreRegProductIdList calls return a count of zero for static and tldml enabled for " + tld);
      }
    }

    [TestMethod]
    public void GetValidRegistrationLengthsStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          int count = 0;
          List<int> staticPreRegLengths = staticTld.GetValidRegistrationLengths(dc, regLengths);

          foreach (int tldmlValidPreRegLength in dotTypeCache.GetValidRegistrationLengths(dc, regLengths))
          {
            AssertHelper.AddResults(tldmlValidPreRegLength == staticPreRegLengths[count],
                                    "GetValidRegistrationLengths did not match for " + tld + ". And for this domain count " + dc + ". Static: "
                                    + staticPreRegLengths[count] + ". Tldml Enabled: " + tldmlValidPreRegLength);
            count++;
          }
        }
      }
    }

    [TestMethod]
    public void GetValidRegistrationProductIdListStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          List<int> statValidRegistrationProductIdList = staticTld.GetValidRegistrationProductIdList(dc, regLengths);

          int count = 0;

          foreach (int tldmlPid in dotTypeCache.GetValidRegistrationProductIdList(dc, regLengths))
          {
            AssertHelper.AddResults(tldmlPid == statValidRegistrationProductIdList[count],
                                    "GetValidRegistrationProductIdList did not match for " + tld + ". And for this domain count " + dc + ". Static: "
                                    + statValidRegistrationProductIdList[count] + ". Tldml Enabled: " + tldmlPid);
            count++;
          }
        }
      }
    }

    [TestMethod]
    public void GetValidRenewalLengthsStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          int count = 0;
          List<int> statValidRenewalLengths = staticTld.GetValidRenewalLengths(dc, regLengths);

          foreach (int tldlGetValidRenewalLength in dotTypeCache.GetValidRenewalLengths(dc, regLengths))
          {
            AssertHelper.AddResults(tldlGetValidRenewalLength == statValidRenewalLengths[count],
                                    "GetValidRenewalLengths did not match for " + tld + ". And for this domain count " + dc + ". Static: "
                                    + statValidRenewalLengths[count] + ". Tldml Enabled: " + tldlGetValidRenewalLength);
            count++;
          }
        }
      }
    }

    [TestMethod]
    public void GetValidRenewalProductIdListStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          int count = 0;

          List<int> statValidRenewalProductIdList = staticTld.GetValidRenewalProductIdList(dc, regLengths);

          foreach (int tldmlPid in dotTypeCache.GetValidRenewalProductIdList(dc, regLengths))
          {
            AssertHelper.AddResults(tldmlPid == statValidRenewalProductIdList[count],
                                    "GetValidRenewalProductIdList did not match for " + tld + ". And for this domain count " + dc + ". Static: "
                                    + statValidRenewalProductIdList[count] + ". Tldml Enabled: " + tldmlPid);
            count++;
          }
        }
      }
    }

    [TestMethod]
    public void GetValidTransferLengthsStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          List<int> statValidTransferLengths = staticTld.GetValidTransferLengths(dc, regLengths);
          int count = 0;

          foreach (int tldlLength in dotTypeCache.GetValidTransferLengths(dc, regLengths))
          {
            AssertHelper.AddResults(tldlLength == statValidTransferLengths[count],
                                    "GetValidTransferLengths did not match for " + tld + ". And for this domain count " + dc + ". Static: "
                                    + statValidTransferLengths[count] + ". Tldml Enabled: " + tldlLength);
            count++;
          }
        }
      }
    }

    [TestMethod]
    public void GetValidTransferProductIdListStaticVsTLDMLEnabled()
    {
      foreach (string tld in tlds)
      {
        object[] methodParms = new object[1] { tld };

        IDotTypeInfo staticTld = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          int count = 0;
          List<int> statValidTransferProductIdList = staticTld.GetValidTransferProductIdList(dc, regLengths);

          foreach (int tldmlTransferPid in dotTypeCache.GetValidTransferProductIdList(dc, regLengths))
          {
            AssertHelper.AddResults(tldmlTransferPid == statValidTransferProductIdList[count],
                                    "GetValidTransferLengths did not match for " + tld + ". And for this domain count " + dc + ". Static: "
                                    + statValidTransferProductIdList[count] + ". Tldml Enabled: " + tldmlTransferPid);
            count++;
          }
        }
      }
    }



  }
}
