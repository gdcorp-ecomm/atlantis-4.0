using System.Linq;
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
using TLDML;
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
  [DeploymentItem("Atlantis.Framework.DotTypeCache.StaticTypes.dll")]
  public class DotTypeCacheTestsForTldmlEnabledTlds
  {
    private List<string> tlds;
    private int[] domainCount;

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
      MockHttpRequest request = new MockHttpRequest("http://siteadmin.debug.intranet.gdg/default.aspx");
      MockHttpContext.SetFromWorkerRequest(request);

      IShopperContext shopperContext = HttpProviderContainer.Instance.Resolve<IShopperContext>();
      shopperContext.SetNewShopper("832652");
      tlds = TLDML.TLDMLDocument.GetTLDMLSupportedTLDs();
      tlds = new List<string>();
      tlds.Add("ORG");
      domainCount = new int[] { 1, 6, 21, 50, 101, 201 };
    }

    [TestCleanup()]
    public void MyTestCleanup()
    {
      Console.WriteLine("Assertions: " + AssertHelper.AssertCount.ToString());
      AssertHelper.GetResults();
    }
    
    [TestMethod]
    public void GetDotType()
    {
      foreach (var tld in tlds)
      {
        IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(tld);
        AssertHelper.AddResults(info != null, "GetDotTypeInfo is null for: " + tld);

        string description = DotTypeCache.GetAdditionalInfoValue(tld, "Description");
        AssertHelper.AddResults(!string.IsNullOrEmpty(description), "GetDotTypeInfo description is null or empty for:  " + tld);
      }
    }

    [TestMethod]
    public void GetDotTypeProductIds()
    {
      foreach (var tld in tlds)
      {
        IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(tld);

        foreach (int dc in domainCount)
        {
          List<int> reglengths = TLDML.TLDMLProduct.GetAllEnabledRegistrationLengths(tld);

          foreach (var reglength in reglengths)
          {
            int productIdDefault = DotTypeCache.GetRegistrationProductId(tld, reglength, dc);
            int prodId = Convert.ToInt32(TLDMLProduct.GetPFID(tld, reglength, productfamily.DomainRegistration, dc));
            AssertHelper.AddResults(productIdDefault == prodId, "Product ids do not match for: " + tld + "Expected in tldml: " + prodId + ". Actual: " + productIdDefault + " . Reg length: " + reglength + " year(s) and domain count: " + dc);

            int productIdFirstRegistrar = DotTypeCache.GetTransferProductId(tld, "1", reglength, dc);
            int productIdSecondRegistrar = DotTypeCache.GetRegistrationProductId(tld, "2", reglength, dc);
            AssertHelper.AddResults((productIdDefault * productIdFirstRegistrar * productIdSecondRegistrar) != 0,
                                    "GetDotTypeProductIds - A product id was zero for: " + tld + ". Reg length: " + reglength + " year(s) and domain count: " + dc);
          }
        }
      }
    }

    [TestMethod]
    public void GetValidRegistrationProductIds()
    {
      foreach (var tld in tlds)
      {
        IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(tld);
        List<int> reglengths = TLDML.TLDMLProduct.GetAllEnabledRegistrationLengths(tld);

        foreach (var reglength in reglengths)
        {
          foreach (int dc in domainCount)
          {
            List<int> registrationProductIds = info.GetValidRegistrationProductIdList(dc, reglength);

            foreach (int regLength in reglengths)
            {
              int prodIdFromTdml =
                Convert.ToInt32(TLDMLProduct.GetPFID(tld, regLength, productfamily.DomainRegistration, dc));

              AssertHelper.AddResults(registrationProductIds.Contains(prodIdFromTdml),
                                      "Product id from tdml was not found in the registrationProductId list for: " +
                                      tld + ". Expected in list: " +
                                      prodIdFromTdml + ". Reg length: " + regLength +
                                      " year(s) and domain count: " + dc);
            }
          }
        }
      }
    }

    [TestMethod]
    public void GetValidTransferProductIds()
    {
      foreach (var tld in tlds)
      {
        IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(tld);
        List<int> reglengths = TLDML.TLDMLProduct.GetAllEnabledTransferLengths(tld);

        foreach (int reglength in reglengths)
        {
          foreach (int dc in domainCount)
          {
            List<int> transferProductIds = info.GetValidTransferProductIdList(dc, reglength);

            int prodIdFromTdml =
              Convert.ToInt32(TLDMLProduct.GetPFID(tld, reglength, productfamily.DomainTransfer, dc));

            AssertHelper.AddResults(transferProductIds.Contains(prodIdFromTdml),
                                    "Product id from tdml was not found in the transferProductIds list for: " + tld +
                                    ". Expected in list: " +
                                    prodIdFromTdml + ". Reg length: " + reglength +
                                    " year(s) and domain count: " + dc);
          }
        }
      }
    }

    [TestMethod]
    public void GetDotTypeProductIds2()
    {
      foreach (var tld in tlds)
      {
        IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(tld);

        List<int> regLengths = TLDML.TLDMLProduct.GetAllEnabledRegistrationLengths(tld);
        
        foreach (int dc in domainCount)
        {
          List<int> registrationProductIds = info.GetValidRegistrationProductIdList(dc, regLengths.ToArray());
          List<int> transferProductIds = info.GetValidTransferProductIdList(dc, regLengths.ToArray());
          List<int> renewProductIds = info.GetValidRenewalProductIdList(dc, regLengths.ToArray());
          List<int> renewProductIds2 = info.GetValidRenewalProductIdList("1", dc, regLengths.ToArray());
          List<int> renewProductIds3 = info.GetValidRenewalProductIdList("2", dc, regLengths.ToArray());

          foreach (int regLength in regLengths)
          {
            int renewalProdId = Convert.ToInt32(TLDMLProduct.GetPFID(tld, regLength, productfamily.DomainRenewal, dc));
            int renewProductId = info.GetRenewalProductId("1", regLength, dc);
            AssertHelper.AddResults(renewProductId == renewalProdId, "Renewal product ids do not match for: " + tld + ". Expected in tldml: " + renewalProdId + ". Actual: " + renewProductId + " . Reg length: " + regLength + " year(s) and domain count: " + dc);

            AssertHelper.AddResults(
              (registrationProductIds.Count * transferProductIds.Count * renewProductIds.Count * renewProductIds2.Count *
               renewProductIds3.Count * renewProductId) != 0, "GetDotTypeProductIds2 - A product id was zero for: " + tld + ". Reg length: " + regLength + " year(s) and domain count: " + dc);
          }
        }
      }
    }

    //[TestMethod] - This is not working at this time
    public void GetDotTypePreRegProductId()
    {
      foreach (var tld in tlds)
      {
        IDotTypeInfo info = DotTypeCache.GetDotTypeInfo(tld);
        List<int> regLengths = new List<int>();

        for (int regLength = info.Product.PreregistrationYears("GA").Min;
             regLength <= info.Product.PreregistrationYears("GA").Max;
             regLength++)
        {
          foreach (int dc in domainCount)
          {
            int renewalProdId = Convert.ToInt32(TLDMLProduct.GetPFID(tld, regLength, productfamily.DomainRenewal, dc));
            int productId = DotTypeCache.GetPreRegProductId(tld, regLength, dc);
            AssertHelper.AddResults(productId != 0, "GetDotTypePreRegProductId - A pre reg product id was zero for: " + tld + ". Reg length: " + regLength + " year(s) and domain count: " + dc);
          }
        }
      }
    }

    [TestMethod]
    public void GetDotTypeProductId()
    {
      foreach (var tld in tlds)
      {
        int prodId = Convert.ToInt32(TLDML.TLDMLProduct.GetPFID(tld, 1, productfamily.DomainRegistration));

        IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo(tld);
        string registryId = dotType.GetRegistryIdByProductId(prodId);
        AssertHelper.AddResults(!string.IsNullOrEmpty(registryId), "GetRegistryIdByProductId not found for: : " + tld);
      }
    }

    [TestMethod]
    public void TLDMLAttributeExists()
    {
      foreach (var tld in tlds)
      {
        var dotTypeAttributesDictionary = DataCache.DataCache.GetExtendedTLDData(tld);
        var dotTypeAttributes = dotTypeAttributesDictionary[tld];
        AssertHelper.AddResults(dotTypeAttributes.ContainsKey(TLDMLDotTypes.TLDMLSupportedFlag), "Key not found for: " + tld);
      }
    }

    [TestMethod]
    public void PropertiesAreValid()
    {
      foreach (string tld in tlds)
      {
        IDotTypeInfo tldml = DotTypeCache.GetDotTypeInfo(tld);

        List<int> expiredAuctionLengths = TLDML.TLDMLProduct.GetAllEnabledExpiredAuctionLengths(tld);
        
        AssertHelper.AddResults(tldml.Product.ExpiredAuctionsYears.Max == expiredAuctionLengths.Max(), "Product.ExpiredAuctionsYears.Max is not correct for: " +
          tld + ". Expected: " + expiredAuctionLengths.Max() + ". Actual: " + tldml.Product.ExpiredAuctionsYears.Max);

        AssertHelper.AddResults(tldml.Product.ExpiredAuctionsYears.Min == expiredAuctionLengths.Min(), "Product.ExpiredAuctionsYears.Min is not correct for: " +
          tld + ". Expected: " + expiredAuctionLengths.Min() + ". Actual: " + tldml.Product.ExpiredAuctionsYears.Min);

        AssertHelper.AddResults(tldml.Product.ExpiredAuctionsYears.IsValid(1), "ExpiredAuctionsYears.IsValid(1) returned false for: " + tld);

        List<int> regLengths = TLDML.TLDMLProduct.GetAllEnabledRegistrationLengths(tld);

        AssertHelper.AddResults(regLengths.Max() == tldml.Product.RegistrationYears.Max, "Product.RegistrationYears.Max did not match for: " + tld +
          ". Expected: " + regLengths.Max() + ". Actual: " + tldml.Product.RegistrationYears.Max);

        AssertHelper.AddResults(regLengths.Min() == tldml.Product.RegistrationYears.Min, "Product.RegistrationYears.Min did not match for: " + tld +
          ". Expected: " + regLengths.Min() + ". Actual: " + tldml.Product.RegistrationYears.Min);
        
        AssertHelper.AddResults(tldml.Product.RegistrationYears.IsValid(1), "Product.RegistrationYears.IsValid(1) is false for:  " + tld);

        List<int> renewalLengths = TLDML.TLDMLProduct.GetAllEnabledRenewalLengths(tld);

        AssertHelper.AddResults(tldml.Product.RenewalYears.Max == renewalLengths.Max(), "Product.RenewalYears.Max did not match for: " + tld +
          ". Expected: " + renewalLengths.Max() + ". Actual: " + tldml.Product.RenewalYears.Max);

        AssertHelper.AddResults(tldml.Product.RenewalYears.Min == renewalLengths.Min(), "Product.RenewalYears.Min did not match for: " + tld +
            ". Expected: " + renewalLengths.Min() + ". Actual: " + tldml.Product.RenewalYears.Min);

        AssertHelper.AddResults(tldml.Product.RenewalYears.IsValid(1), "Product.RenewalYears.IsValid(1) is false for: " + tld);

        List<int> transferLengths = TLDML.TLDMLProduct.GetAllEnabledTransferLengths(tld);

        AssertHelper.AddResults(tldml.Product.TransferYears.Max == transferLengths.Max(), "Product.TransferYears.Max did not match for " + tld +
            ". Expected: " + transferLengths.Max() + ". Actual: " + tldml.Product.TransferYears.Max);

        AssertHelper.AddResults(tldml.Product.TransferYears.Min == transferLengths.Min(), "Product.TransferYears.Min did not match for " + tld +
            ". Expected: " + transferLengths.Min() + ". Actual: " + tldml.Product.TransferYears.Min);

        AssertHelper.AddResults(tldml.Product.TransferYears.IsValid(1), "Product.TransferYears.IsValid(1) is false for: " + tld);

        //List<int> preRegLengths = TLDML.TLDMLProduct. GetAllEnabledTransferLengths(tld);

        //AssertHelper.AddResults(tldml.Product.PreregistrationYears("GA").Max,
        //  "Product.PreregistrationYears.Max did not match for " + tld + ". Static: " + staticTld.Product.PreregistrationYears("GA").Max +
        //  ". Tldml Enabled: " + tldml.Product.PreregistrationYears("GA").Max);

        //AssertHelper.AddResults(staticTld.Product.PreregistrationYears("GA").Min == tldml.Product.PreregistrationYears("GA").Min,
        //  "Product.PreregistrationYears.Min did not match for " + tld + ". Static: " + staticTld.Product.PreregistrationYears("GA").Min +
        //  ". Tldml Enabled: " + tldml.Product.PreregistrationYears("GA").Min);

        //AssertHelper.AddResults(staticTld.Product.PreregistrationYears("GA").IsValid(1) == tldml.Product.PreregistrationYears("GA").IsValid(1),
        //  "Product.PreregistrationYears.IsValid(1) did not match for " + tld + ". Static: " + staticTld.Product.PreregistrationYears("GA").IsValid(1) +
        //  ". Tldml Enabled: " + tldml.Product.PreregistrationYears("GA").IsValid(1));

        //AssertHelper.AddResults(staticTld.TldId.ToString() == tldml.TldId.ToString(), "TldId did not match for " + tld +
        //  ". Static: " + staticTld.TldId + ". Tldml Enabled: " + tldml.TldId);

        //AssertHelper.AddResults(staticTld.DotType.ToLower() == tldml.DotType.ToLower(), "DotType static vs tldml did not match for " + tld + ". Static: "
        //  + staticTld.DotType.ToLower() + ". Tldml Enabled: " + tldml.DotType.ToLower());

        //AssertHelper.AddResults(staticTld.IsMultiRegistry == tldml.IsMultiRegistry, "IsMultiRegistry did not match for " + tld + ". Static: "
        //  + staticTld.IsMultiRegistry + ". Tldml Enabled: " + tldml.IsMultiRegistry);

        //AssertHelper.AddResults(staticTld.MaxExpiredAuctionRegLength == tldml.MaxExpiredAuctionRegLength, "MaxExpiredAuctionRegLength did not match for " + tld + ". Static: "
        //  + staticTld.MaxExpiredAuctionRegLength + ". Tldml Enabled: " + tldml.MaxExpiredAuctionRegLength);

        //AssertHelper.AddResults(staticTld.MaxPreRegLength == tldml.MaxPreRegLength, "MaxPreRegLength did not match for " + tld + ". Static: "
        //  + staticTld.MaxPreRegLength + ". Tldml Enabled: " + tldml.MaxPreRegLength);

        //AssertHelper.AddResults(staticTld.MaxRegistrationLength == tldml.MaxRegistrationLength, "MaxRegistrationLength did not match for " + tld + ". Static: "
        //  + staticTld.MaxRegistrationLength + ". Tldml Enabled: " + tldml.MaxRegistrationLength);

        //AssertHelper.AddResults(staticTld.MaxRenewalLength == tldml.MaxRenewalLength, "MaxRenewalLength did not match for " + tld + ". Static: "
        //  + staticTld.MaxRenewalLength + ". Tldml Enabled: " + tldml.MaxRenewalLength);

        //AssertHelper.AddResults(staticTld.MaxTransferLength == tldml.MaxTransferLength, "MaxTransferLength did not match for " + tld + ". Static: "
        //  + staticTld.MaxTransferLength + ". Tldml Enabled: " + tldml.MaxTransferLength);

        //AssertHelper.AddResults(staticTld.MinExpiredAuctionRegLength == tldml.MinExpiredAuctionRegLength, "MinExpiredAuctionRegLength did not match for " + tld + ". Static: "
        //  + staticTld.MinExpiredAuctionRegLength + ". Tldml Enabled: " + tldml.MinExpiredAuctionRegLength);

        //AssertHelper.AddResults(staticTld.MinPreRegLength == tldml.MinPreRegLength, "MinPreRegLength did not match for " + tld + ". Static: "
        //  + staticTld.MinPreRegLength + ". Tldml Enabled: " + tldml.MinPreRegLength);

        //AssertHelper.AddResults(staticTld.MinRegistrationLength == tldml.MinRegistrationLength, "MinRegistrationLength did not match for " + tld + ". Static: "
        //  + staticTld.MinRegistrationLength + ". Tldml Enabled: " + tldml.MinRegistrationLength);

        //AssertHelper.AddResults(staticTld.MinRenewalLength == tldml.MinRenewalLength, "MinRenewalLength did not match for " + tld + ". Static: "
        //  + staticTld.MinRenewalLength + ". Tldml Enabled: " + tldml.MinRenewalLength);

        //AssertHelper.AddResults(staticTld.MinTransferLength == tldml.MinTransferLength, "MinTransferLength did not match for " + tld + ". Static: "
        //  + staticTld.MinTransferLength + ". Tldml Enabled: " + tldml.MinTransferLength);

        //AssertHelper.AddResults(staticTld.Tld.RenewProhibitedPeriodForExpiration == tldml.Tld.RenewProhibitedPeriodForExpiration, "RenewProhibitedPeriodForExpiration did not match for " + tld + ". Static: "
        // + staticTld.Tld.RenewProhibitedPeriodForExpiration + ". Tldml Enabled: " + tldml.Tld.RenewProhibitedPeriodForExpiration);

        //AssertHelper.AddResults(staticTld.Tld.RenewProhibitedPeriodForExpirationUnit == tldml.Tld.RenewProhibitedPeriodForExpirationUnit, "RenewProhibitedPeriodForExpirationUnit did not match for " + tld + ". Static: "
        // + staticTld.Tld.RenewProhibitedPeriodForExpirationUnit + ". Tldml Enabled: " + tldml.Tld.RenewProhibitedPeriodForExpirationUnit);

      }
    }


















    [TestMethod]
    public void OrgStaticVsTLDMLEnabled()
    {
      foreach (var tld in tlds)
      {
        Type staticDotTypesType =
          Assembly.GetAssembly(typeof(DotTypeCache)).GetType("Atlantis.Framework.DotTypeCache.StaticDotTypes");
        MethodInfo getStaticDotType = staticDotTypesType.GetMethod("GetDotType",
                                                                   BindingFlags.Static | BindingFlags.Public);
        object[] methodParms = new object[1] { "org" };
        IDotTypeInfo staticOrg = getStaticDotType.Invoke(null, methodParms) as IDotTypeInfo;

        IDotTypeInfo tldmlOrg = DotTypeCache.GetDotTypeInfo("org");

        Assert.AreNotEqual(staticOrg, tldmlOrg);
      }
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

    /*
    [TestMethod]
    public void GetOfferedTLDFlagsForAll()
    {
      Dictionary<string, Dictionary<string, bool>> offeredTLDs = DotTypeProvider.GetOfferedTLDFlags(OfferedTLDProductTypes.Registration);
      Assert.IsTrue(offeredTLDs != null);
    }

    [TestMethod]
    public void GetOfferedTLDFlagsForOrg()
    {
      Dictionary<string, Dictionary<string, bool>> offeredTLDs = DotTypeProvider.GetOfferedTLDFlags(OfferedTLDProductTypes.Registration, new string[] { "org" });
      Assert.IsTrue(offeredTLDs != null);
    }

    [TestMethod]
    public void GetOfferedTLDFlagsForOrgAndCom()
    {
      Dictionary<string, Dictionary<string, bool>> offeredTLDs = DotTypeProvider.GetOfferedTLDFlags(OfferedTLDProductTypes.Registration, new string[] {"org", "com"});
      Assert.IsTrue(offeredTLDs != null);
    }*/

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
      Assert.AreEqual((string)testContextInstance.DataRow["MaxPreRegLength"], info.MaxPreRegLength.ToString(),
          "MaxPreRegLength does not match for: " + dotType.ToUpper() + ". Expected: " +
          (string)testContextInstance.DataRow["MaxPreRegLength"] + " Actual: " + info.MaxPreRegLength.ToString());
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
      Assert.AreEqual((string)testContextInstance.DataRow["MinPreRegLength"], info.MinPreRegLength.ToString(),
          "MinPreRegLength does not match for: " + dotType.ToUpper() + ". Expected: " +
          (string)testContextInstance.DataRow["MinPreRegLength"] + " Actual: " + info.MinPreRegLength.ToString());
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

      List<int> preRegList = info.GetValidPreRegProductIdList(1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (preRegList.Count > 0)
      {
        int productId = DotTypeCache.GetPreRegProductId(dotType, info.MinPreRegLength, 1);
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
      List<int> preRegList = info.GetValidPreRegProductIdList(1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (preRegList.Count > 0)
      {
        int productId = DotTypeCache.GetPreRegProductId(dotType, info.MaxPreRegLength, 1);
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
      List<int> preRegList = info.GetValidPreRegProductIdList(1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

      if (preRegList.Count > 0)
      {
        int productId = DotTypeCache.GetPreRegProductId(dotType, info.MinPreRegLength, 100);
        Assert.IsTrue(productId > 0, "ProductID < 0 for dotType: " + dotType);

        //verify it is different than domainCount = 1
        int productId1stTier = DotTypeCache.GetPreRegProductId(dotType, info.MinPreRegLength, 1);
        Assert.AreNotEqual(productId1stTier, productId, "ProductID for bulk tiers is the same for dotType: " + dotType);
      }
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetPreRegProductId_EmptyDotType()
    {
      int productId = DotTypeCache.GetPreRegProductId(string.Empty, 1, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetPreRegProductId_InvalidDotType()
    {
      int productId = DotTypeCache.GetPreRegProductId("blah", 1, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetPreRegProductId_NullDotType()
    {
      int productId = DotTypeCache.GetPreRegProductId(null, 1, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetPreRegProductId_InvalidRegistrationLength()
    {
      int productId = DotTypeCache.GetPreRegProductId("com", 20, 1);
      Assert.AreEqual(0, productId);
    }

    [TestMethod]
    public void N_GetPreRegProductId_InvalidDomainCount()
    {
      int productId = DotTypeCache.GetPreRegProductId("com", 1, 10000);
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
      int preRegLength = DotTypeCache.GetMinPreRegLength(dotType);
      Assert.AreEqual((string)testContextInstance.DataRow["MinPreRegLength"], preRegLength.ToString(),
          "MinPreRegLength is not as expected for dotType: " + dotType);
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetMinPreRegLength_Empty()
    {
      int preRegLength = DotTypeCache.GetMinPreRegLength(string.Empty);
      Assert.AreEqual(1, preRegLength);
    }

    [TestMethod]
    public void N_GetMinPreRegLength_Invalid()
    {
      int preRegLength = DotTypeCache.GetMinPreRegLength("blah");
      Assert.AreEqual(1, preRegLength);
    }

    [TestMethod]
    public void N_GetMinPreRegLength_Null()
    {
      int preRegLength = DotTypeCache.GetMinPreRegLength(null);
      Assert.AreEqual(1, preRegLength);
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
      int preRegLength = DotTypeCache.GetMaxPreRegLength(dotType);
      Assert.AreEqual((string)testContextInstance.DataRow["MaxPreRegLength"], preRegLength.ToString(),
          "MaxPreRegLength is not as expected for dotType: " + dotType);
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetMaxPreRegLength_Empty()
    {
      int preRegLength = DotTypeCache.GetMaxPreRegLength(string.Empty);
      Assert.AreEqual(10, preRegLength);
    }

    [TestMethod]
    public void N_GetMaxPreRegLength_Invalid()
    {
      int preRegLength = DotTypeCache.GetMaxPreRegLength("blah");
      Assert.AreEqual(10, preRegLength);
    }

    [TestMethod]
    public void N_GetMaxPreRegLength_Null()
    {
      int preRegLength = DotTypeCache.GetMaxPreRegLength(null);
      Assert.AreEqual(10, preRegLength);
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

    #region GetAdditionalInfoValue

    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\DotTypesData.xml", "DotType",
        DataAccessMethod.Sequential), DeploymentItem("Atlantis.Framework.DotTypeCache.Tests\\DotTypesData.xml"),
        TestMethod]
    public void GetAdditionalInfoValue()
    {
      string dotType = System.Convert.ToString(testContextInstance.DataRow["DotTypeName"]);
      string result = DotTypeCache.GetAdditionalInfoValue(dotType, "LandingPageUrl");
      Assert.AreNotEqual(0, result.Length, "LandingPageUrl not returned for dotType: " + dotType);
    }

    #region Negative Tests

    [TestMethod]
    public void N_GetAdditionalInfoValue_EmptyDotType()
    {
      string result = DotTypeCache.GetAdditionalInfoValue(string.Empty, "LandingPageUrl");
      Assert.AreEqual(0, result.Length);
    }

    [TestMethod]
    public void N_GetAdditionalInfoValue_InvalidDotType()
    {
      string result = DotTypeCache.GetAdditionalInfoValue("blah", "LandingPageUrl");
      Assert.AreEqual(0, result.Length);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void N_GetAdditionalInfoValue_NullDotType()
    {
      string result = DotTypeCache.GetAdditionalInfoValue(null, "LandingPageUrl");
      Assert.AreEqual(0, result.Length);
    }

    [TestMethod]
    public void N_GetAdditionalInfoValue_EmptyValue()
    {
      string result = DotTypeCache.GetAdditionalInfoValue("com", string.Empty);
      Assert.AreEqual(0, result.Length);
    }

    [TestMethod]
    public void N_GetAdditionalInfoValue_InvalidValue()
    {
      string result = DotTypeCache.GetAdditionalInfoValue("com", "blah");
      Assert.AreEqual(0, result.Length);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void N_GetAdditionalInfoValue_NullValue()
    {
      string result = DotTypeCache.GetAdditionalInfoValue("com", null);
      Assert.AreEqual(0, result.Length);
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
  }
}
