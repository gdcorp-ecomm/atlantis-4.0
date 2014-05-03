using Atlantis.Framework.Localization.Interface;
using Atlantis.Framework.Providers.Localization.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Localization.Tests
{
  /// <summary>
  /// Summary description for CountrySitesActiveTests
  /// </summary>
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Localization.Impl.dll")]
  public class CountrySitesActiveTests
  {
    private const int _REQUEST_TYPE = 725;

    [TestMethod]
    public void CountrySitesActiveResponse_DefaultActiveCountrySites_IEnumerable()
    {
      CountrySitesActiveResponseData response = CountrySitesActiveResponseData.DefaultCountrySites;
      Assert.IsNotNull(response);

      int count = 0;
      foreach (ICountrySite cs in response.CountrySites)
      {
        count++;

        switch (cs.Id)
        {
          case "au":
            Assert.AreEqual("au", cs.Id);
            Assert.AreEqual("Australia", cs.Description);
            Assert.AreEqual(false, cs.IsInternalOnly);
            Assert.AreEqual("en-AU", cs.DefaultMarketId);
            Assert.AreEqual("AUD", cs.DefaultCurrencyType);
            break;
          case "ca":
            Assert.AreEqual("ca", cs.Id);
            Assert.AreEqual("Canada", cs.Description);
            Assert.AreEqual(false, cs.IsInternalOnly);
            Assert.AreEqual("en-CA", cs.DefaultMarketId);
            Assert.AreEqual("CAD", cs.DefaultCurrencyType);
            break;
          case "in":
            Assert.AreEqual("in", cs.Id);
            Assert.AreEqual("India", cs.Description);
            Assert.AreEqual(false, cs.IsInternalOnly);
            Assert.AreEqual("en-IN", cs.DefaultMarketId);
            Assert.AreEqual("INR", cs.DefaultCurrencyType);
            break;
          case "uk":
            Assert.AreEqual("uk", cs.Id);
            Assert.AreEqual("United Kingdom", cs.Description);
            Assert.AreEqual(false, cs.IsInternalOnly);
            Assert.AreEqual("en-GB", cs.DefaultMarketId);
            Assert.AreEqual("GBP", cs.DefaultCurrencyType);
            break;
          case "www":
            Assert.AreEqual("www", cs.Id);
            Assert.AreEqual("Global US", cs.Description);
            Assert.AreEqual(false, cs.IsInternalOnly);
            Assert.AreEqual("en-US", cs.DefaultMarketId);
            Assert.AreEqual("USD", cs.DefaultCurrencyType);
            break;
          default:
            Assert.Fail("Unknown default CountrySite {0}", cs.Id);
            break;
        }
      }
      Assert.AreEqual(5, count);
    }

    [TestMethod]
    public void CountrySitesActiveResponse_IndexedByCaseInsensitiveCountrySite()
    {
      CountrySitesActiveResponseData response = CountrySitesActiveResponseData.DefaultCountrySites;
      Assert.IsNotNull(response);

      ICountrySite cs;
      Assert.IsTrue(response.TryGetCountrySiteById("au", out cs));
      Assert.IsNotNull(cs);
      Assert.AreEqual("au", cs.Id);

      ICountrySite cs2;
      Assert.IsTrue(response.TryGetCountrySiteById("AU", out cs2));
      Assert.IsNotNull(cs);
      Assert.AreEqual("au", cs.Id);
      Assert.AreSame(cs, cs2);

      Assert.IsTrue(response.TryGetCountrySiteById("ca", out cs));
      Assert.IsNotNull(cs);
      Assert.AreEqual("ca", cs.Id);
    }

    [TestMethod]
    public void CountrySitesActiveResponse_UnknownCountrySite_ReturnsNull()
    {
      CountrySitesActiveResponseData response = CountrySitesActiveResponseData.DefaultCountrySites;
      Assert.IsNotNull(response);

      ICountrySite cs;
      Assert.IsFalse(response.TryGetCountrySiteById("xx", out cs));
    }

    [TestMethod]
    public void CountrySitesActiveResponse_IsValidCountrySite_ValidInput_ReturnsTrue()
    {
      CountrySitesActiveResponseData response = CountrySitesActiveResponseData.DefaultCountrySites;
      Assert.IsTrue(response.IsValidCountrySite("www", false));
      Assert.IsTrue(response.IsValidCountrySite("uk", false));
      Assert.IsTrue(response.IsValidCountrySite("au", false));
    }

    [TestMethod]
    public void CountrySitesActiveResponse_IsValidCountrySite_InvalidInput_ReturnsFalse()
    {
      CountrySitesActiveResponseData response = CountrySitesActiveResponseData.DefaultCountrySites;
      Assert.IsFalse(response.IsValidCountrySite("xx", false));
      Assert.IsFalse(response.IsValidCountrySite("", false));
      Assert.IsFalse(response.IsValidCountrySite(" ", false));
    }

    [TestMethod]
    public void CountrySiteActiveResponse_TaxCountry_IsNotNull()
    {
      const string xml = @"<data count=""3"">
          <item catalog_countrySite=""au"" catalog_priceGroupID=""0"" countrySiteDescription=""Australia"" isActive=""-1"" internalOnly=""0"" defaultMarketID=""en-AU"" defaultCurrencyType=""AUD""/>
          <item catalog_countrySite=""ca"" catalog_priceGroupID=""0"" countrySiteDescription=""Canada"" isActive=""-1"" internalOnly=""0"" defaultMarketID=""en-CA"" defaultCurrencyType=""CAD""/>
          <item catalog_countrySite=""ar"" catalog_priceGroupID=""0"" countrySiteDescription=""Argentina"" isActive=""-1"" internalOnly=""1"" defaultMarketID=""es-AR"" defaultCurrencyType=""ARP""/>
        </data>";

      var response = CountrySitesActiveResponseData.FromCacheDataXml(xml);
      ICountrySite countrySite;

      response.TryGetCountrySiteById("au", out countrySite);
      Assert.IsNotNull(countrySite.TaxCountry);
    }

    [TestMethod]
    public void CountrySiteActiveResponse_DisplayTaxAndFeesNotInData_Returns0()
    {
      const string xml = @"<data count=""3"">
          <item catalog_countrySite=""au"" catalog_priceGroupID=""0"" countrySiteDescription=""Australia"" isActive=""-1"" internalOnly=""0"" defaultMarketID=""en-AU"" defaultCurrencyType=""AUD""/>
          <item catalog_countrySite=""ca"" catalog_priceGroupID=""0"" countrySiteDescription=""Canada"" isActive=""-1"" internalOnly=""0"" defaultMarketID=""en-CA"" defaultCurrencyType=""CAD""/>
          <item catalog_countrySite=""ar"" catalog_priceGroupID=""0"" countrySiteDescription=""Argentina"" isActive=""-1"" internalOnly=""1"" defaultMarketID=""es-AR"" defaultCurrencyType=""ARP""/>
        </data>";

      var response = CountrySitesActiveResponseData.FromCacheDataXml(xml);
      ICountrySite countrySite;
      int displayTaxAndFeeOut;

      response.TryGetCountrySiteById("au", out countrySite);
      Assert.IsTrue(countrySite.DisplayTaxesAndFees == 0);
    }

    [TestMethod]
    public void CountrySiteActiveResponse_GetTaxCountry_ValidXml()
    {
      const string xml = @"<data count=""3"">
          <item catalog_countrySite=""au"" catalog_priceGroupID=""0"" countrySiteDescription=""Australia"" isActive=""-1"" internalOnly=""0"" defaultMarketID=""en-AU"" defaultCurrencyType=""AUD"" displayTaxesAndFees=""1"" taxCountry=""DE""/>
          <item catalog_countrySite=""ca"" catalog_priceGroupID=""0"" countrySiteDescription=""Canada"" isActive=""-1"" internalOnly=""0"" defaultMarketID=""en-CA"" defaultCurrencyType=""CAD""/>
          <item catalog_countrySite=""ar"" catalog_priceGroupID=""0"" countrySiteDescription=""Argentina"" isActive=""-1"" internalOnly=""1"" defaultMarketID=""es-AR"" defaultCurrencyType=""ARP""/>
        </data>";

      var response = CountrySitesActiveResponseData.FromCacheDataXml(xml);
      ICountrySite countrySite;

      response.TryGetCountrySiteById("au", out countrySite);
      Assert.IsTrue(countrySite.TaxCountry.Equals("DE"));
    }

    [TestMethod]
    public void CountrySiteActiveResponse_GetDisplayTaxAndFees_ValidXml()
    {
      const string xml = @"<data count=""3"">
          <item catalog_countrySite=""au"" catalog_priceGroupID=""0"" countrySiteDescription=""Australia"" isActive=""-1"" internalOnly=""0"" defaultMarketID=""en-AU"" defaultCurrencyType=""AUD"" displayTaxesAndFees=""1"" taxCountry=""DE""/>
          <item catalog_countrySite=""ca"" catalog_priceGroupID=""0"" countrySiteDescription=""Canada"" isActive=""-1"" internalOnly=""0"" defaultMarketID=""en-CA"" defaultCurrencyType=""CAD""/>
          <item catalog_countrySite=""ar"" catalog_priceGroupID=""0"" countrySiteDescription=""Argentina"" isActive=""-1"" internalOnly=""1"" defaultMarketID=""es-AR"" defaultCurrencyType=""ARP""/>
        </data>";

      var response = CountrySitesActiveResponseData.FromCacheDataXml(xml);
      ICountrySite countrySite;

      response.TryGetCountrySiteById("au", out countrySite);
      Assert.IsTrue(countrySite.DisplayTaxesAndFees == 1);
    }

    #region Miscellaneous tests

    [TestMethod]
    public void CountrySitesActiveResponse_DefaultCountrySiteInfo()
    {
      ICountrySite countrySite = CountrySitesActiveResponseData.DefaultCountrySiteInfo;
      Assert.IsNotNull(countrySite);
      Assert.AreEqual("www", countrySite.Id);
      Assert.AreEqual("en-US", countrySite.DefaultMarketId);
      Assert.AreEqual("USD", countrySite.DefaultCurrencyType);
      Assert.AreEqual(0, countrySite.PriceGroupId);
      Assert.IsFalse(countrySite.IsInternalOnly);
    }

    [TestMethod]
    public void CountrySitesActiveRequestDataConstructorGeneratesNewRequestDataObject()
    {
      var request = new CountrySitesActiveRequestData();
      Assert.IsNotNull(request);
    }

    [TestMethod]
    public void CountrySitesActiveRequestDataCacheKeySame()
    {
      var request1 = new CountrySitesActiveRequestData();
      var request2 = new CountrySitesActiveRequestData();
      Assert.AreNotEqual(request1, request2);
      Assert.IsNotNull(request1);
      Assert.IsNotNull(request2);
      Assert.AreEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void CountrySitesActiveRequestReturnsValidResponse()
    {
      var request = new CountrySitesActiveRequestData();
      var response = (CountrySitesActiveResponseData)Engine.Engine.ProcessRequest(request, _REQUEST_TYPE);
      Assert.IsNotNull(response);
    }

    [TestMethod]
    public void CountrySitesActiveResponseToXMLReturnsEmptyString()
    {
      var response = CountrySitesActiveResponseData.DefaultCountrySites;
      Assert.AreEqual(string.Empty, response.ToXML());
    }

    [TestMethod]
    public void CountrySitesActiveResponse_InvalidXml_ReturnsEmptyResponseData()
    {
      const string invalidXml = @"<data count=""2"">
          <item catalog_countrySite=""au"" catalog_priceGroupID=""0"" countrySiteDescription=""Australia"" isActive=""-1"" internalOnly=""0"" defaultMarketID=""en-AU"" defaultCurrencyType=""AUD""/>
          <item catalog_countrySite=""ca"" catalog_priceGroupID=""0"" countrySiteDescription=""Canada"" isActive=""-1"" internalOnly=""0"" defaultMarketID=""en-CA"" />
        </data>";

      CountrySitesActiveResponseData response = CountrySitesActiveResponseData.FromCacheDataXml(invalidXml);
      Assert.AreSame(CountrySitesActiveResponseData.DefaultCountrySites, response);

      response = CountrySitesActiveResponseData.FromCacheDataXml(null);
      Assert.AreSame(CountrySitesActiveResponseData.DefaultCountrySites, response);

      response = CountrySitesActiveResponseData.FromCacheDataXml("   ");
      Assert.AreSame(CountrySitesActiveResponseData.DefaultCountrySites, response);

      response = CountrySitesActiveResponseData.FromCacheDataXml(string.Empty);
      Assert.AreSame(CountrySitesActiveResponseData.DefaultCountrySites, response);
    }

    [TestMethod]
    public void CountrySitesActiveResponse_InvalidCountrySites_NotAddedToCollection()
    {
      const string invalidXml = @"<data count=""2"">
          <item catalog_countrySite="""" catalog_priceGroupID=""0"" countrySiteDescription=""Australia"" isActive=""-1"" internalOnly=""0"" defaultMarketID=""en-AU"" defaultCurrencyType=""AUD""/>
          <item catalog_priceGroupID=""0"" countrySiteDescription=""Canada"" isActive=""-1"" internalOnly=""0"" defaultMarketID=""en-CA"" />
        </data>";

      CountrySitesActiveResponseData response = CountrySitesActiveResponseData.FromCacheDataXml(invalidXml);
      Assert.AreEqual(0, response.Count);
    }

    [TestMethod]
    public void CountrySitesActiveResponse_IsValidCountrySite_AcceptsIncludeInternalOnlyArgument_ReturnsAsNeeded()
    {
      const string xml = @"<data count=""3"">
          <item catalog_countrySite=""au"" catalog_priceGroupID=""0"" countrySiteDescription=""Australia"" isActive=""-1"" internalOnly=""0"" defaultMarketID=""en-AU"" defaultCurrencyType=""AUD""/>
          <item catalog_countrySite=""ca"" catalog_priceGroupID=""0"" countrySiteDescription=""Canada"" isActive=""-1"" internalOnly=""0"" defaultMarketID=""en-CA"" defaultCurrencyType=""CAD""/>
          <item catalog_countrySite=""ar"" catalog_priceGroupID=""0"" countrySiteDescription=""Argentina"" isActive=""-1"" internalOnly=""1"" defaultMarketID=""es-AR"" defaultCurrencyType=""ARP""/>
        </data>";

      CountrySitesActiveResponseData response = CountrySitesActiveResponseData.FromCacheDataXml(xml);
      Assert.IsTrue(response.IsValidCountrySite("au", false));
      Assert.IsTrue(response.IsValidCountrySite("au", true));
      Assert.IsTrue(response.IsValidCountrySite("ca", false));
      Assert.IsTrue(response.IsValidCountrySite("ca", true));
      Assert.IsFalse(response.IsValidCountrySite("ar", false));
      Assert.IsTrue(response.IsValidCountrySite("ar", true));      
    }

    #endregion
  }
}
