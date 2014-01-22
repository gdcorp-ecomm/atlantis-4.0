using System.CodeDom.Compiler;
using System.Linq;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Automation.Framework.TestSetUpAndSettings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Automation.Framework.TLDML;

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
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Products.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DomainSearch.Impl.dll")]
  public class DotTypeCacheTestsForTldmlEnabledTlds
  {
    private List<string> tlds;
    private int[] domainCount;
    private int[] standardRegLengths;

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
    }

    public IDotTypeProvider DotTypeProvider
    {
      get { return HttpProviderContainer.Instance.Resolve<IDotTypeProvider>(); }
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

      //tlds = TLDML.TLDMLDocument.GetTLDMLSupportedTLDs();
      tlds = new List<string>();
      tlds.Add("DIAMONDS");
      //tlds.Add("ENTERPRISES");
      //tlds.Add("TIPS");
      //tlds.Add("VOYAGE");

      domainCount = new[] { 1 };
      standardRegLengths = new[] { 1 };
    }

    [TestCleanup()]
    public void MyTestCleanup()
    {
      Console.WriteLine("Assertions: " + AssertHelper.AssertCount.ToString());
      AssertHelper.GetResults();
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetDotType()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);
        AssertHelper.AddResults(dotTypeInfo != null, "GetDotTypeInfo is null for: " + tld);
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetTransferProductId()
    {
      foreach (string tld in tlds)
      {
        foreach (int dc in domainCount)
        {
          foreach (int reglength in standardRegLengths)
          {
            TLDMLPhase phase = new TLDMLPhase();
            phase.PhaseName = "Transfer";
            phase.DomainName = "blahblahblah." + tld;

            Automation.Framework.TLDML.DomainSearch domainSearch = new Automation.Framework.TLDML.DomainSearch(phase);
            int tier = domainSearch.GetPremiumTier();

            var dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);
            IDomainProductLookup domainProductLookup = DomainProductLookup.Create(reglength, dc, LaunchPhases.GeneralAvailability, TLDProductTypes.Transfer, tier);
            int dotTypeCacheGetTransferProductId = dotTypeInfo.GetProductId(domainProductLookup);

            int prodIdFromTldml = Convert.ToInt32(TLDMLProduct.GetPFID(tld, reglength, phase, dc));

            AssertHelper.AddResults(
              dotTypeCacheGetTransferProductId == prodIdFromTldml && dotTypeCacheGetTransferProductId != 0,
              "GetTransferProductId - Transfer product ids do not match or are zero for: " + tld + ". Reg length: " +
              reglength + " year(s) and domain count: " + dc);
          }
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetRegistrationProductId()
    {
      foreach (string tld in tlds)
      {
        bool gaPhaseCurrent = TLDMLProduct.IsTldInPhase(tld, "GA");

        foreach (int dc in domainCount)
        {
          TLDMLPhase phase = new TLDMLPhase();
          phase.DomainName = "blahblahblah." + tld;

          if (gaPhaseCurrent)
          {
            phase.PhaseName = "General Availability";
          }
          else
          {
            phase.PhaseName = "Registration";
          }

          Automation.Framework.TLDML.DomainSearch domainSearch = new Automation.Framework.TLDML.DomainSearch(phase);
          int tier = domainSearch.GetPremiumTier();

          List<int> reglengths;

          if (gaPhaseCurrent)
          {
            reglengths = TLDMLProduct.GetAllEnabledGeneralAvailabilityLengths(tld, 1, tier);
          }
          else
          {
            reglengths = TLDMLProduct.GetAllEnabledRegistrationLengths(tld);
          }

          foreach (int reglength in reglengths)
          {
            var dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);
            IDomainProductLookup domainProductLookup = DomainProductLookup.Create(reglength, dc, LaunchPhases.GeneralAvailability, TLDProductTypes.Registration, tier);
            int dotTypeCacheGetRegistrationProductId = dotTypeInfo.GetProductId(domainProductLookup);




            int prodIdFromDR = Convert.ToInt32(TLDMLProduct.GetPFID(tld, reglength, phase, dc));

            AssertHelper.AddResults(dotTypeCacheGetRegistrationProductId == prodIdFromDR && prodIdFromDR != 0,
                                    "GetRegistrationProductId - Domain reg prod ids did not match or are zero for: " +
                                    tld + ". Reg length: " + reglength + " year(s) and domain count: " + dc);
          }
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetValidRegistrationProductIdListWithIndRegLength()
    {
      foreach (string tld in tlds)
      {
        bool gaPhaseCurrent = TLDMLProduct.IsTldInPhase(tld, "GA");
        List<int> reglengths;

        TLDMLPhase phase = new TLDMLPhase();

        if (gaPhaseCurrent)
        {
          phase.PhaseName = "General Availability";
        }
        else
        {
          phase.PhaseName = "Registration";
        }

        phase.DomainName = "blahblahblah." + tld;

        Automation.Framework.TLDML.DomainSearch domainSearch = new Automation.Framework.TLDML.DomainSearch(phase);
        int tier = domainSearch.GetPremiumTier();

        foreach (int dc in domainCount)
        {

          if (gaPhaseCurrent)
          {
            reglengths = TLDMLProduct.GetAllEnabledGeneralAvailabilityLengths(tld, dc, tier);
          }
          else
          {
            reglengths = TLDMLProduct.GetAllEnabledRegistrationLengths(tld);
          }

          foreach (int reglength in reglengths)
          {
            var dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);
            IDomainProductListLookup domainProductListLookup = DomainProductListLookup.Create(reglengths.ToArray(), dc, LaunchPhases.GeneralAvailability, TLDProductTypes.Registration, tier);
            List<int> dotTypeCacheProductIdList = dotTypeInfo.GetProductIdList(domainProductListLookup);

            int prodIdFromTdml = Convert.ToInt32(TLDMLProduct.GetPFID(tld, reglength, phase, dc));

            AssertHelper.AddResults(dotTypeCacheProductIdList.Contains(prodIdFromTdml),
                                    "Product id from tdml was not found in the registrationProductId list for: " +
                                    tld + ". Expected in list: " +
                                    prodIdFromTdml + ". Reg length: " + reglength +
                                    " year(s) and domain count: " + dc);
          }
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetValidTransferProductIds()
    {
      foreach (string tld in tlds)
      {
        List<int> reglengths = TLDMLProduct.GetAllEnabledTransferLengths(tld);

        foreach (int dc in domainCount)
        {
          foreach (int reglength in reglengths)
          {

            var dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);
            var transferProductIds = dotTypeInfo.GetValidTransferProductIdList(dc, reglengths.ToArray());

            TLDMLPhase phase = new TLDMLPhase();
            phase.PhaseName = "Transfer";
            int prodIdFromTdml = Convert.ToInt32(TLDMLProduct.GetPFID(tld, reglength, phase, dc));

            AssertHelper.AddResults(transferProductIds.Contains(prodIdFromTdml),
                                    "Product id from tdml was not found in the transferProductIds list for: " + tld +
                                    ". Expected in list: " +
                                    prodIdFromTdml + ". Reg length: " + reglength +
                                    " year(s) and domain count: " + dc);
          }
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetDotTypeProductIds2()
    {
      foreach (string tld in tlds)
      {
        var dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);

        List<int> regLengths = TLDMLProduct.GetAllEnabledRegistrationLengths(tld);

        foreach (int dc in domainCount)
        {
          List<int> registrationProductIds = dotTypeInfo.GetValidRegistrationProductIdList(dc, regLengths.ToArray());
          List<int> transferProductIds = dotTypeInfo.GetValidTransferProductIdList(dc, regLengths.ToArray());
          List<int> renewProductIds = dotTypeInfo.GetValidRenewalProductIdList(dc, regLengths.ToArray());
          List<int> renewProductIds2 = dotTypeInfo.GetValidRenewalProductIdList("1", dc, regLengths.ToArray());
          List<int> renewProductIds3 = dotTypeInfo.GetValidRenewalProductIdList("2", dc, regLengths.ToArray());

          foreach (int regLength in regLengths)
          {
            TLDMLPhase phase = new TLDMLPhase();
            phase.PhaseName = "Renewal";
            phase.DomainName = "blahblahblah." + tld;

            Automation.Framework.TLDML.DomainSearch domainSearch = new Automation.Framework.TLDML.DomainSearch(phase);
            int tier = domainSearch.GetPremiumTier();

            IDomainProductLookup domainProductLookup = DomainProductLookup.Create(regLength, dc, LaunchPhases.GeneralAvailability, TLDProductTypes.Renewal, tier);
            int renewProductId = dotTypeInfo.GetProductId(domainProductLookup);

            int renewalProdId = Convert.ToInt32(TLDMLProduct.GetPFID(tld, regLength, phase, dc));


            AssertHelper.AddResults(renewProductId == renewalProdId,
                                    "Renewal product ids do not match for: " + tld + ". Expected in tldml: " +
                                    renewalProdId + ". Actual: " + renewProductId + " . Reg length: " + regLength +
                                    " year(s) and domain count: " + dc);

            AssertHelper.AddResults(
              (registrationProductIds.Count * transferProductIds.Count * renewProductIds.Count * renewProductIds2.Count *
               renewProductIds3.Count * renewProductId) != 0,
              "GetDotTypeProductIds2 - A product id was zero for: " + tld + ". Reg length: " + regLength +
              " year(s) and domain count: " + dc);
          }
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetDotTypePreRegProductId()
    {
      foreach (string tld in tlds)
      {
        var dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);

        if (dotTypeInfo.Product.PreregistrationYears("GA").Min > 0 && dotTypeInfo.Product.PreregistrationYears("GA").Max > 0)
        {
          for (int regLength = dotTypeInfo.Product.PreregistrationYears("GA").Min;
               regLength <= dotTypeInfo.Product.PreregistrationYears("GA").Max;
               regLength++)
          {
            foreach (int dc in domainCount)
            {
              TLDMLPhase phase = new TLDMLPhase();
              phase.PhaseName = "General Availability";

              int generalAvailabilityProdId = Convert.ToInt32(TLDMLProduct.GetPFID(tld, regLength, phase, dc));
              int productId = dotTypeInfo.GetPreRegProductId(LaunchPhases.GeneralAvailability, regLength, dc);
              AssertHelper.AddResults(generalAvailabilityProdId == productId && productId != 0,
                                      "GetDotTypePreRegProductId - A pre reg product id was zero for: " + tld +
                                      ". Reg length: " + regLength + " year(s) and domain count: " + dc);
            }
          }
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void RegistrationYearsPropertiesAreValid()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);

        List<int> regLengths = TLDMLProduct.GetAllEnabledRegistrationLengths(tld);

        AssertHelper.AddResults(regLengths.Max() == dotTypeInfo.Product.RegistrationYears.Max,
                                "Product.RegistrationYears.Max did not match for: " + tld +
                                ". Expected: " + regLengths.Max() + ". Actual: " +
                                dotTypeInfo.Product.RegistrationYears.Max);

        AssertHelper.AddResults(regLengths.Min() == dotTypeInfo.Product.RegistrationYears.Min,
                                "Product.RegistrationYears.Min did not match for: " + tld +
                                ". Expected: " + regLengths.Min() + ". Actual: " +
                                dotTypeInfo.Product.RegistrationYears.Min);

        AssertHelper.AddResults(dotTypeInfo.Product.RegistrationYears.IsValid(1),
                                "Product.RegistrationYears.IsValid(1) is false for:  " + tld);
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void RenewalYearsPropertiesAreValid()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);

        List<int> renewalLengths = TLDMLProduct.GetAllEnabledRenewalLengths(tld);

        AssertHelper.AddResults(dotTypeInfo.Product.RenewalYears.Max == renewalLengths.Max(),
                                "Product.RenewalYears.Max did not match for: " + tld +
                                ". Expected: " + renewalLengths.Max() + ". Actual: " +
                                dotTypeInfo.Product.RenewalYears.Max);

        AssertHelper.AddResults(dotTypeInfo.Product.RenewalYears.Min == renewalLengths.Min(),
                                "Product.RenewalYears.Min did not match for: " + tld +
                                ". Expected: " + renewalLengths.Min() + ". Actual: " +
                                dotTypeInfo.Product.RenewalYears.Min);

        AssertHelper.AddResults(dotTypeInfo.Product.RenewalYears.IsValid(1),
                                "Product.RenewalYears.IsValid(1) is false for: " + tld);
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void ProductTransferYearsPropertiesAreValid()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);

        List<int> transferLengths = TLDMLProduct.GetAllEnabledTransferLengths(tld);

        AssertHelper.AddResults(dotTypeInfo.Product.TransferYears.Max == transferLengths.Max(),
                                "Product.TransferYears.Max did not match for " + tld +
                                ". Expected: " + transferLengths.Max() + ". Actual: " +
                                dotTypeInfo.Product.TransferYears.Max);

        AssertHelper.AddResults(dotTypeInfo.Product.TransferYears.Min == transferLengths.Min(),
                                "Product.TransferYears.Min did not match for " + tld +
                                ". Expected: " + transferLengths.Min() + ". Actual: " +
                                dotTypeInfo.Product.TransferYears.Min);

        AssertHelper.AddResults(dotTypeInfo.Product.TransferYears.IsValid(1),
                                "Product.TransferYears.IsValid(1) is false for: " + tld);
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void DotTypeCacheIsValid()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);

        tldml tldmlObj = TLDMLDocument.GetTLDMLObjectByTLD(tld);

        int id = 0;
        try
        {
          id = Convert.ToInt32(tldmlObj.tld.tld.id);
        }
        catch
        {
        }

        AssertHelper.AddResults(dotTypeInfo.TldId == id && id != 0, "TldId did not match or is zero for: " + tld);

        AssertHelper.AddResults(dotTypeInfo.DotType.ToLower() == tld.ToLower(),
                                "DotTypeCache.DotType.ToLower() does not match for: " + tld +
                                ". Expected: " + dotTypeInfo.DotType.ToLower() + ". Actual: " + tld.ToLower());
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void IsMultiRegistryFalse()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);

        AssertHelper.AddResults(!dotTypeInfo.IsMultiRegistry, "IsMultiRegistry was not false for: " + tld);
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void MaxRegistrationLengthPropertyIsValid()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);
        List<int> regLengths = TLDMLProduct.GetAllEnabledRegistrationLengths(tld);

        AssertHelper.AddResults(dotTypeInfo.MaxRegistrationLength == regLengths.Max(),
                                "MaxRegistrationLength did not match for " + tld +
                                ". Expected: " + regLengths.Max() + ". Actual: " + dotTypeInfo.MaxRegistrationLength);
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void RenewalLengthPropertiesAreValid()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);
        List<int> renewalLengths = TLDMLProduct.GetAllEnabledRenewalLengths(tld);

        AssertHelper.AddResults(dotTypeInfo.MaxRenewalLength == renewalLengths.Max(),
                                "MaxRenewalLength did not match for " + tld +
                                ". Expected: " + renewalLengths.Max() + ". Actual: " + dotTypeInfo.MaxRenewalLength);

        AssertHelper.AddResults(dotTypeInfo.MinRenewalLength == renewalLengths.Min(),
                                "MinRenewalLength did not match for " + tld +
                                ". Expected: " + renewalLengths.Min() + ". Actual: " + dotTypeInfo.MinRenewalLength);
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void TransferLengthPropertiesAreValid()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);
        List<int> transferLengths = TLDMLProduct.GetAllEnabledTransferLengths(tld);

        AssertHelper.AddResults(dotTypeInfo.MaxTransferLength == transferLengths.Max(),
                                "MaxTransferLength did not match for " + tld +
                                ". Expected: " + transferLengths.Max() + ". Actual: " + dotTypeInfo.MaxTransferLength);

        AssertHelper.AddResults(dotTypeInfo.MinTransferLength == transferLengths.Min(),
                                "MinTransferLength did not match for " + tld +
                                ". Expected: " + transferLengths.Min() + ". Actual: " + dotTypeInfo.MinTransferLength);
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void CanRenew()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);

        int outValueTldml;

        bool canRenewTldml = dotTypeInfo.CanRenew(DateTime.Now.AddYears(-5), out outValueTldml);

        AssertHelper.AddResults(canRenewTldml, "Can renew returned false for: " + tld);
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetPreRegProductIds()
    {
      foreach (string tld in tlds)
      {
        List<int> preRegLengths = TLDMLProduct.GetAllEnabledPreRegistrationLengths(tld);

        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);

        int tldmlmethod = 0;

        foreach (int dc in domainCount)
        {
          foreach (int preRegLength in preRegLengths)
          {
            tldmlmethod = dotTypeInfo.GetPreRegProductId(LaunchPhases.GeneralAvailability, preRegLength, dc);

            AssertHelper.AddResults(tldmlmethod != 0,
                                    "GetPreRegProductId for pre reg length: " + preRegLength +
                                    " year(s) and for domain count: " + dc + " was zero for " + tld);
          }
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetRenewalProductId()
    {
      foreach (string tld in tlds)
      {
        foreach (int dc in domainCount)
        {
          List<int> reglengths = TLDMLProduct.GetAllEnabledRenewalLengths(tld);

          foreach (int reglength in reglengths)
          {
            TLDMLPhase phase = new TLDMLPhase();
            phase.PhaseName = "Renewal";
            phase.DomainName = "blahblahblah." + tld;

            Automation.Framework.TLDML.DomainSearch domainSearch = new Automation.Framework.TLDML.DomainSearch(phase);
            int tier = domainSearch.GetPremiumTier();

            var dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);
            IDomainProductLookup domainProductLookup = DomainProductLookup.Create(reglength, dc, LaunchPhases.GeneralAvailability, TLDProductTypes.Renewal, tier);
            int dotTypeCacheGetRenewalProductId = dotTypeInfo.GetProductId(domainProductLookup);


            int prodIdFromTldml = Convert.ToInt32(TLDMLProduct.GetPFID(tld, reglength, phase, dc));

            AssertHelper.AddResults(
              dotTypeCacheGetRenewalProductId == prodIdFromTldml && dotTypeCacheGetRenewalProductId != 0,
              "GetRenewalProductId - ids do not match or are zero for: " + tld + ". Reg length: " + reglength +
              " year(s) and domain count: " + dc);
          }
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetRenewalProductId2()
    {
      foreach (string tld in tlds)
      {
        foreach (int dc in domainCount)
        {
          List<int> reglengths = TLDMLProduct.GetAllEnabledRenewalLengths(tld);

          foreach (int reglength in reglengths)
          {
            TLDMLPhase phase = new TLDMLPhase();
            phase.PhaseName = "Renewal";

            int dotTypeCacheGetRenewalProductId = DotTypeCache.GetRenewalProductId(tld, "1", reglength, dc);
            int prodIdFromTldml = Convert.ToInt32(TLDMLProduct.GetPFID(tld, reglength, phase, dc));
            AssertHelper.AddResults(
              dotTypeCacheGetRenewalProductId == prodIdFromTldml && dotTypeCacheGetRenewalProductId != 0,
              "GetRenewalProductId2 - ids do not match or are zero for: " + tld + ". Reg length: " + reglength +
              " year(s) and domain count: " + dc);
          }
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetValidRegistrationLengths()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          List<int> validPreRegLengths = dotTypeInfo.GetValidRegistrationLengths(dc, standardRegLengths);

          AssertHelper.AddResults(validPreRegLengths.Count > 0, "GetValidRegistrationLengths did not return a count for " + tld + ". And for this domain count " + dc);
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetValidRegistrationProductIdList()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          List<int> pids = dotTypeInfo.GetValidRegistrationProductIdList(dc, standardRegLengths);

          AssertHelper.AddResults(pids.Count > 0, "GetValidRegistrationProductIdList did not match for " + tld + ". And for this domain count " + dc);
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetValidRenewalLengths()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          List<int> renewalLengths = dotTypeInfo.GetValidRenewalLengths(dc, standardRegLengths);

          AssertHelper.AddResults(renewalLengths.Count > 0,
                                  "GetValidRenewalLengths did not return a count for " + tld +
                                  ". And for this domain count " + dc);
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetValidRenewalProductIdList()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          List<int> renewalPidList = dotTypeInfo.GetValidRenewalProductIdList(dc, standardRegLengths);

          AssertHelper.AddResults(renewalPidList.Count > 0, "GetValidRenewalProductIdList count not greater then zero for "
            + tld + ". And for this domain count " + dc);
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetValidTransferLengths()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          List<int> tldlLength = dotTypeInfo.GetValidTransferLengths(dc, standardRegLengths);

          AssertHelper.AddResults(tldlLength.Count > 0,
                                  "GetValidTransferLengths not greater then zero for " + tld + ". And for this domain count " + dc);
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetValidTransferProductId()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeInfo = DotTypeProvider.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          List<int> transList = dotTypeInfo.GetValidTransferProductIdList(dc, standardRegLengths);
          AssertHelper.AddResults(transList.Count > 0, "GetValidTransferLengths not greater then zero for  " + tld + ". And for this domain count " + dc);
        }
      }
    }






  }
}
