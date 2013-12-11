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
      tlds.Add("TODAY");
    
      domainCount = new[] { 1 };//, 6, 21, 50, 101, 201 };
      standardRegLengths = new[] { 1 }; //, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
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
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);
        AssertHelper.AddResults(dotTypeCache != null, "GetDotTypeInfo is null for: " + tld);
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

            int dotTypeCacheGetTransferProductId = DotTypeCache.GetTransferProductId(tld, "1", reglength, dc);
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
        foreach (int dc in domainCount)
        {
          List<int> reglengths = TLDMLProduct.GetAllEnabledRegistrationLengths(tld);

          foreach (int reglength in reglengths)
          {
            TLDMLPhase phase = new TLDMLPhase();
            phase.PhaseName = "Registration";

            int dotTypeCacheGetRegistrationProductId = DotTypeCache.GetRegistrationProductId(tld, "2", reglength, dc);
            int prodIdFromDR =
              Convert.ToInt32(TLDMLProduct.GetPFID(tld, reglength, phase, dc));

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
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);
        List<int> reglengths = TLDMLProduct.GetAllEnabledRegistrationLengths(tld);

        foreach (int reglength in reglengths)
        {
          foreach (int dc in domainCount)
          {
            TLDMLPhase phase = new TLDMLPhase();
            phase.PhaseName = "Registration";

            List<int> registrationProductIds = dotTypeCache.GetValidRegistrationProductIdList(dc, reglength);

            int prodIdFromTdml = Convert.ToInt32(TLDMLProduct.GetPFID(tld, reglength, phase, dc));

            AssertHelper.AddResults(registrationProductIds.Contains(prodIdFromTdml),
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
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);
        List<int> reglengths = TLDMLProduct.GetAllEnabledTransferLengths(tld);

        foreach (int reglength in reglengths)
        {
          foreach (int dc in domainCount)
          {
            TLDMLPhase phase = new TLDMLPhase();
            phase.PhaseName = "Transfer";

            List<int> transferProductIds = dotTypeCache.GetValidTransferProductIdList(dc, reglength);

            int prodIdFromTdml =
              Convert.ToInt32(TLDMLProduct.GetPFID(tld, reglength, phase, dc));

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
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        List<int> regLengths = TLDMLProduct.GetAllEnabledRegistrationLengths(tld);

        foreach (int dc in domainCount)
        {
          List<int> registrationProductIds = dotTypeCache.GetValidRegistrationProductIdList(dc, regLengths.ToArray());
          List<int> transferProductIds = dotTypeCache.GetValidTransferProductIdList(dc, regLengths.ToArray());
          List<int> renewProductIds = dotTypeCache.GetValidRenewalProductIdList(dc, regLengths.ToArray());
          List<int> renewProductIds2 = dotTypeCache.GetValidRenewalProductIdList("1", dc, regLengths.ToArray());
          List<int> renewProductIds3 = dotTypeCache.GetValidRenewalProductIdList("2", dc, regLengths.ToArray());

          foreach (int regLength in regLengths)
          {
            TLDMLPhase phase = new TLDMLPhase();
            phase.PhaseName = "Renewal";

            int renewalProdId = Convert.ToInt32(TLDMLProduct.GetPFID(tld, regLength, phase, dc));
            int renewProductId = dotTypeCache.GetRenewalProductId("1", regLength, dc);

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
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        if (dotTypeCache.Product.PreregistrationYears("GA").Min > 0 && dotTypeCache.Product.PreregistrationYears("GA").Max > 0)
        {
          for (int regLength = dotTypeCache.Product.PreregistrationYears("GA").Min;
               regLength <= dotTypeCache.Product.PreregistrationYears("GA").Max;
               regLength++)
          {
            foreach (int dc in domainCount)
            {
              TLDMLPhase phase = new TLDMLPhase();
              phase.PhaseName = "Registration";

              int renewalProdId = Convert.ToInt32(TLDMLProduct.GetPFID(tld, regLength, phase, dc));
              int productId = DotTypeCache.GetPreRegProductId(tld, LaunchPhases.GeneralAvailability, regLength, dc);
              AssertHelper.AddResults(productId != 0,
                                      "GetDotTypePreRegProductId - A pre reg product id was zero for: " + tld +
                                      ". Reg length: " + regLength + " year(s) and domain count: " + dc);
            }
          }
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetDotTypeProductId()
    {
      foreach (string tld in tlds)
      {
        TLDMLPhase phase = new TLDMLPhase();
        phase.PhaseName = "Registration";

        int prodId = Convert.ToInt32(TLDMLProduct.GetPFID(tld, 1, phase));

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);
        string registryId = dotTypeCache.GetRegistryIdByProductId(prodId);

        AssertHelper.AddResults(!string.IsNullOrEmpty(registryId), "GetRegistryIdByProductId not found for: : " + tld);
      }
    }

    //[TestMethod]
    //public void TldmlAttributesExists()
    //{
    //  foreach (string tld in tlds)
    //  {
    //    Dictionary<string, Dictionary<string, string>> dotTypeAttributesDictionary = DataCache.DataCache.GetExtendedTLDData(tld);
    //    Dictionary<string, string> dotTypeAttributes = dotTypeAttributesDictionary[tld];

    //    AssertHelper.AddResults(dotTypeAttributes.ContainsKey(TLDMLDotTypes.TLDMLSupportedFlag),
    //                            "Key not found for: " + tld);
    //  }
    //}

    //[TestMethod]
    //public void ProductExpiredAuctionsYearsPropertiesAreValid()
    //{
    //  foreach (string tld in tlds)
    //  {
    //    IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

    //    List<int> expiredAuctionLengths = TLDML.TLDMLProduct.GetAllEnabledExpiredAuctionLengths(tld);

    //    AssertHelper.AddResults(dotTypeCache.Product.ExpiredAuctionsYears.Max == expiredAuctionLengths.Max(),
    //                            "Product.ExpiredAuctionsYears.Max is not correct for: " +
    //                            tld + ". Expected: " + expiredAuctionLengths.Max() + ". Actual: " +
    //                            dotTypeCache.Product.ExpiredAuctionsYears.Max);

    //    AssertHelper.AddResults(dotTypeCache.Product.ExpiredAuctionsYears.Min == expiredAuctionLengths.Min(),
    //                            "Product.ExpiredAuctionsYears.Min is not correct for: " +
    //                            tld + ". Expected: " + expiredAuctionLengths.Min() + ". Actual: " +
    //                            dotTypeCache.Product.ExpiredAuctionsYears.Min);

    //    AssertHelper.AddResults(dotTypeCache.Product.ExpiredAuctionsYears.IsValid(1),
    //                            "ExpiredAuctionsYears.IsValid(1) returned false for: " + tld);
    //  }
    //}

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void RegistrationYearsPropertiesAreValid()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        List<int> regLengths = TLDMLProduct.GetAllEnabledRegistrationLengths(tld);

        AssertHelper.AddResults(regLengths.Max() == dotTypeCache.Product.RegistrationYears.Max,
                                "Product.RegistrationYears.Max did not match for: " + tld +
                                ". Expected: " + regLengths.Max() + ". Actual: " +
                                dotTypeCache.Product.RegistrationYears.Max);

        AssertHelper.AddResults(regLengths.Min() == dotTypeCache.Product.RegistrationYears.Min,
                                "Product.RegistrationYears.Min did not match for: " + tld +
                                ". Expected: " + regLengths.Min() + ". Actual: " +
                                dotTypeCache.Product.RegistrationYears.Min);

        AssertHelper.AddResults(dotTypeCache.Product.RegistrationYears.IsValid(1),
                                "Product.RegistrationYears.IsValid(1) is false for:  " + tld);
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void RenewalYearsPropertiesAreValid()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        List<int> renewalLengths = TLDMLProduct.GetAllEnabledRenewalLengths(tld);

        AssertHelper.AddResults(dotTypeCache.Product.RenewalYears.Max == renewalLengths.Max(),
                                "Product.RenewalYears.Max did not match for: " + tld +
                                ". Expected: " + renewalLengths.Max() + ". Actual: " +
                                dotTypeCache.Product.RenewalYears.Max);

        AssertHelper.AddResults(dotTypeCache.Product.RenewalYears.Min == renewalLengths.Min(),
                                "Product.RenewalYears.Min did not match for: " + tld +
                                ". Expected: " + renewalLengths.Min() + ". Actual: " +
                                dotTypeCache.Product.RenewalYears.Min);

        AssertHelper.AddResults(dotTypeCache.Product.RenewalYears.IsValid(1),
                                "Product.RenewalYears.IsValid(1) is false for: " + tld);
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void ProductTransferYearsPropertiesAreValid()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        List<int> transferLengths = TLDMLProduct.GetAllEnabledTransferLengths(tld);

        AssertHelper.AddResults(dotTypeCache.Product.TransferYears.Max == transferLengths.Max(),
                                "Product.TransferYears.Max did not match for " + tld +
                                ". Expected: " + transferLengths.Max() + ". Actual: " +
                                dotTypeCache.Product.TransferYears.Max);

        AssertHelper.AddResults(dotTypeCache.Product.TransferYears.Min == transferLengths.Min(),
                                "Product.TransferYears.Min did not match for " + tld +
                                ". Expected: " + transferLengths.Min() + ". Actual: " +
                                dotTypeCache.Product.TransferYears.Min);

        AssertHelper.AddResults(dotTypeCache.Product.TransferYears.IsValid(1),
                                "Product.TransferYears.IsValid(1) is false for: " + tld);
      }
    }

    //[TestMethod, TestCategory("TLDMLEnabled")]
    //public void ProductPreregistrationYearsPropertiesAreValid()
    //{
    //  foreach (string tld in tlds)
    //  {
    //    IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

    //    List<int> preRegLengths = TLDML.TLDMLProduct.GetAllEnabledPreRegistrationLengths(tld);

    //    AssertHelper.AddResults(dotTypeCache.Product.PreregistrationYears("GA").Max == preRegLengths.Max(),
    //                            "Product.PreregistrationYears.Max did not match for " + tld +
    //                            ". Expected: " + preRegLengths.Max() + ". Actual: " +
    //                            dotTypeCache.Product.PreregistrationYears("GA").Max);

    //    AssertHelper.AddResults(dotTypeCache.Product.PreregistrationYears("GA").Min == preRegLengths.Min(),
    //                            "Product.PreregistrationYears.Min did not match for " + tld +
    //                            ". Expected: " + preRegLengths.Min() + ". Actual: " +
    //                            dotTypeCache.Product.PreregistrationYears("GA").Min);

    //    AssertHelper.AddResults(dotTypeCache.Product.PreregistrationYears("GA").IsValid(1),
    //                            "Product.PreregistrationYears.IsValid(1) is false for " + tld);
    //  }
    //}

    //[TestMethod, TestCategory("TLDMLEnabled")]
    //public void PreRegLengthPropertiesAreValid()
    //{
    //  foreach (string tld in tlds)
    //  {
    //    IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

    //    List<int> preRegLengths = TLDML.TLDMLProduct.GetAllEnabledPreRegistrationLengths(tld);

    //    AssertHelper.AddResults(dotTypeCache.GetMaxPreRegLength(PreRegPhases.GeneralAvailability) == preRegLengths.Max(),
    //                            "MaxPreRegLength did not match for " + tld +
    //                            ". Expected: " + preRegLengths.Max() + ". Actual: " + dotTypeCache.GetMaxPreRegLength(PreRegPhases.GeneralAvailability));

    //    AssertHelper.AddResults(dotTypeCache.GetMinPreRegLength(PreRegPhases.GeneralAvailability) == preRegLengths.Min(),
    //                            "MinPreRegLength did not match for " + tld +
    //                            ". Expected: " + preRegLengths.Min() + ". Actual: " + dotTypeCache.GetMinPreRegLength(PreRegPhases.GeneralAvailability));
    //  }
    //}

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void DotTypeCacheIsValid()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);
        tldml tldmlObj = TLDMLDocument.GetTLDMLObjectByTLD(tld);

        int id = 0;
        try
        {
          id = Convert.ToInt32(tldmlObj.tld.tld.id);
        }
        catch
        {
        }

        AssertHelper.AddResults(dotTypeCache.TldId == id && id != 0, "TldId did not match or is zero for: " + tld);

        AssertHelper.AddResults(dotTypeCache.DotType.ToLower() == tld.ToLower(),
                                "DotTypeCache.DotType.ToLower() does not match for: " + tld +
                                ". Expected: " + dotTypeCache.DotType.ToLower() + ". Actual: " + tld.ToLower());
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void IsMultiRegistryFalse()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        AssertHelper.AddResults(!dotTypeCache.IsMultiRegistry, "IsMultiRegistry was not false for: " + tld);
      }
    }

    //[TestMethod]
    //public void ExpiredAuctionRegLengthPropertiesAreValid()
    //{
    //  foreach (string tld in tlds)
    //  {
    //    IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

    //    List<int> expiredRegLengths = TLDML.TLDMLProduct.GetAllEnabledExpiredAuctionLengths(tld);

    //    AssertHelper.AddResults(dotTypeCache.MaxExpiredAuctionRegLength == expiredRegLengths.Max(),
    //                            "MaxExpiredAuctionRegLength did not match for " + tld +
    //                            ". Expected: " + expiredRegLengths.Max() + ". Actual: " +
    //                            dotTypeCache.MaxExpiredAuctionRegLength);

    //    AssertHelper.AddResults(dotTypeCache.MinExpiredAuctionRegLength == expiredRegLengths.Min(),
    //                            "MinExpiredAuctionRegLength did not match for " + tld +
    //                            ". Expected: " + expiredRegLengths.Min() + ". Actual: " +
    //                            dotTypeCache.MinExpiredAuctionRegLength);
    //  }
    //}

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void MaxRegistrationLengthPropertyIsValid()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);
        List<int> regLengths = TLDMLProduct.GetAllEnabledRegistrationLengths(tld);

        AssertHelper.AddResults(dotTypeCache.MaxRegistrationLength == regLengths.Max(),
                                "MaxRegistrationLength did not match for " + tld +
                                ". Expected: " + regLengths.Max() + ". Actual: " + dotTypeCache.MaxRegistrationLength);
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void RenewalLengthPropertiesAreValid()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);
        List<int> renewalLengths = TLDMLProduct.GetAllEnabledRenewalLengths(tld);

        AssertHelper.AddResults(dotTypeCache.MaxRenewalLength == renewalLengths.Max(),
                                "MaxRenewalLength did not match for " + tld +
                                ". Expected: " + renewalLengths.Max() + ". Actual: " + dotTypeCache.MaxRenewalLength);

        AssertHelper.AddResults(dotTypeCache.MinRenewalLength == renewalLengths.Min(),
                                "MinRenewalLength did not match for " + tld +
                                ". Expected: " + renewalLengths.Min() + ". Actual: " + dotTypeCache.MinRenewalLength);
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void TransferLengthPropertiesAreValid()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);
        List<int> transferLengths = TLDMLProduct.GetAllEnabledTransferLengths(tld);

        AssertHelper.AddResults(dotTypeCache.MaxTransferLength == transferLengths.Max(),
                                "MaxTransferLength did not match for " + tld +
                                ". Expected: " + transferLengths.Max() + ". Actual: " + dotTypeCache.MaxTransferLength);

        AssertHelper.AddResults(dotTypeCache.MinTransferLength == transferLengths.Min(),
                                "MinTransferLength did not match for " + tld +
                                ". Expected: " + transferLengths.Min() + ". Actual: " + dotTypeCache.MinTransferLength);
      }
    }

    //[TestMethod, TestCategory("TLDMLEnabled")]
    //public void RenewProhibitedPeriodForExpirationPropertiesAreValid()
    //{
    //  foreach (string tld in tlds)
    //  {
    //    IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

    //    int temp = dotTypeCache.Tld.RenewProhibitedPeriodForExpiration;
    //    string temp2 = dotTypeCache.Tld.RenewProhibitedPeriodForExpirationUnit;

    //    AssertHelper.AddResults(false, "Not getting anything back from RenewProhibitedPeriodForExpiration and RenewProhibitedPeriodForExpirationUnit.");
    //  }
    //}

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void CanRenew()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        int outValueTldml;

        bool canRenewTldml = dotTypeCache.CanRenew(DateTime.Now.AddYears(-5), out outValueTldml);

        AssertHelper.AddResults(canRenewTldml, "Can renew returned false for: " + tld);
      }
    }

    //[TestMethod]
    //public void GetExpiredAuctionRegProductIds()
    //{
    //  foreach (string tld in tlds)
    //  {
    //    IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

    //    List<int> regLengths = TLDML.TLDMLProduct.GetAllEnabledExpiredAuctionLengths(tld);

    //    foreach (int dc in domainCount)
    //    {
    //      foreach (int regLength in regLengths)
    //      {
    //        int prodId = Convert.ToInt32(TLDML.TLDMLProduct.GetPFID(tld, 1, productfamily.Registration));

    //        int pid = dotTypeCache.GetExpiredAuctionRegProductId(regLength, dc);

    //        AssertHelper.AddResults(pid != 0,
    //                                "GetExpiredAuctionRegProductId for reg length: " + regLength +
    //                                " year(s) and for domain count: " + dc + " was zero");
    //      }
    //    }
    //  }
    //}

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetPreRegProductIds()
    {
      foreach (string tld in tlds)
      {
        List<int> preRegLengths = TLDMLProduct.GetAllEnabledPreRegistrationLengths(tld);

        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        int tldmlmethod = 0;

        foreach (int dc in domainCount)
        {
          foreach (int preRegLength in preRegLengths)
          {
            tldmlmethod = dotTypeCache.GetPreRegProductId(LaunchPhases.GeneralAvailability, preRegLength, dc);

            AssertHelper.AddResults(tldmlmethod != 0,
                                    "GetPreRegProductId for pre reg length: " + preRegLength +
                                    " year(s) and for domain count: " + dc + " was zero for " + tld);
          }
        }
      }
    }

    //[TestMethod, TestCategory("TLDMLEnabled")]
    //public void GetRegistrationFieldsXml()
    //{
    //  foreach (string tld in tlds)
    //  {
    //    IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);
    //    string registrationFieldsXml = dotTypeCache.GetRegistrationFieldsXml();

    //    AssertHelper.AddResults(!string.IsNullOrEmpty(registrationFieldsXml),
    //                            "GetRegistrationFieldsXml was not populated for " + tld);
    //  }
    //}

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

            int dotTypeCacheGetRenewalProductId = DotTypeCache.GetRenewalProductId(tld, reglength, dc);
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

    //[TestMethod, TestCategory("TLDMLEnabled")]
    //public void GetValidExpiredAuctionRegLengths()
    //{
    //  foreach (string tld in tlds)
    //  {
    //    IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

    //    foreach (int dc in domainCount)
    //    {
    //      List<int> dotTypeCacheGetRenewalProductIds = dotTypeCache.GetValidExpiredAuctionRegLengths(dc,
    //                                                                                                 standardRegLengths);
    //      AssertHelper.AddResults(dotTypeCacheGetRenewalProductIds.Count > 0,
    //                              "GetRenewalProductIds does not contain pfids for: " + tld + ". And domain count: " +
    //                              dc);
    //    }
    //  }
    //}

    //[TestMethod, TestCategory("TLDMLEnabled")]
    //public void GetValidExpiredAuctionRegProductIdList()
    //{
    //  foreach (string tld in tlds)
    //  {
    //    IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

    //    foreach (int dc in domainCount)
    //    {
    //      List<int> dotTypeCacheGetValidExpiredAuctionRegProductIdList =
    //        dotTypeCache.GetValidExpiredAuctionRegProductIdList(dc, standardRegLengths);

    //      AssertHelper.AddResults(dotTypeCacheGetValidExpiredAuctionRegProductIdList.Count > 0,
    //                              "GetValidExpiredAuctionRegProductIdList does not contain pfids for: " + tld +
    //                              ". And domain count: " + dc);
    //    }
    //  }
    //}

    //[TestMethod, TestCategory("TLDMLEnabled")]
    //public void GetValidPreRegLengths()
    //{
    //  foreach (string tld in tlds)
    //  {
    //    IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

    //    foreach (int dc in domainCount)
    //    {
    //      List<int> dotTypeCacheGetValidPreRegLengths = dotTypeCache.GetValidPreRegLengths(PreRegPhases.GeneralAvailability, dc, standardRegLengths);

    //      AssertHelper.AddResults(dotTypeCacheGetValidPreRegLengths.Count > 0,
    //        "GetValidPreRegLengths does not contain pfids for: " + tld + ". And domain count: " + dc);
    //    }
    //  }
    //}

    //[TestMethod, TestCategory("TLDMLEnabled")]
    //public void GetValidPreRegProductIdList()
    //{
    //  foreach (string tld in tlds)
    //  {
    //    IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);
    //    foreach (int dc in domainCount)
    //    {
    //      List<int> preRegPidList = dotTypeCache.GetValidPreRegProductIdList(PreRegPhases.GeneralAvailability, dc, standardRegLengths);

    //      AssertHelper.AddResults(preRegPidList.Count > 0,
    //                              "GetValidPreRegProductIdList calls return a count of zero" + tld);
    //    }
    //  }
    //}

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetValidRegistrationLengths()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          List<int> validPreRegLengths = dotTypeCache.GetValidRegistrationLengths(dc, standardRegLengths);

          AssertHelper.AddResults(validPreRegLengths.Count > 0, "GetValidRegistrationLengths did not return a count for " + tld + ". And for this domain count " + dc);
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetValidRegistrationProductIdList()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          List<int> pids = dotTypeCache.GetValidRegistrationProductIdList(dc, standardRegLengths);

          AssertHelper.AddResults(pids.Count > 0, "GetValidRegistrationProductIdList did not match for " + tld + ". And for this domain count " + dc);
        }
      }
    }

    [TestMethod, TestCategory("TLDMLEnabled")]
    public void GetValidRenewalLengths()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          List<int> renewalLengths = dotTypeCache.GetValidRenewalLengths(dc, standardRegLengths);

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
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          List<int> renewalPidList = dotTypeCache.GetValidRenewalProductIdList(dc, standardRegLengths);

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
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          List<int> tldlLength = dotTypeCache.GetValidTransferLengths(dc, standardRegLengths);

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
        IDotTypeInfo dotTypeCache = DotTypeCache.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          List<int> transList = dotTypeCache.GetValidTransferProductIdList(dc, standardRegLengths);
          AssertHelper.AddResults(transList.Count > 0, "GetValidTransferLengths not greater then zero for  " + tld + ". And for this domain count " + dc);
        }
      }
    }






  }
}
