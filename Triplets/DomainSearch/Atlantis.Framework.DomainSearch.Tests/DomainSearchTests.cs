using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.DomainSearch.Interface;
using Atlantis.Framework.Domains.Interface;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Atlantis.Framework.DomainSearch.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DomainSearch.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DomainSearch.interface.dll")]
  public class DomainSearchTests
  {
    private readonly IList<string> _databases = new [] {"similar", "premium", "auctions", "private", "cctld", "affix"};
    const int REQUESTID = 714;
    const string SHOPPER_ID = "840820";
    

    [TestMethod]
    public void TestExactMatch()
    {
      var request = new MockHttpRequest("http://www.spoonymac.com");
      MockHttpContext.SetFromWorkerRequest(request);

      var domainName = string.Format("my-randoms-{0}.com", Guid.NewGuid().ToString().Substring(0, 15));

      var requestData = new DomainSearchRequestData(SHOPPER_ID, string.Empty, string.Empty, string.Empty, 0)
                          {
                            RequestTimeout = TimeSpan.FromSeconds(10),
                            ClientIp = "172.16.172.211",
                            CountrySite = "WWW",
                            //DomainCount = 10,
                            IncludeSpins = true,
                            Language = "en",
                            PrivateLabelId = 1,
                            SearchPhrase = domainName,
                            ShopperStatus = ShopperStatusType.Public,
                            SourceCode = "mblDPPSearch",
                            Tlds = new List<string>(0),
                            DomainSearchDataBases = _databases,
                            ClientIpLatitude = 34.1,
                            ClientIpLongitude = -111.1,
                            ClientIpCity = "scottsdale",
                            ClientIpCountry = "us",
                            ClientIpRegion = "arizona",
                            SplitTestId = "01234",
                            SplitTestSideName = "A"
                          };

      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, REQUESTID);
      Assert.IsTrue(response != null);
      Assert.IsTrue(response.Domains.Count > 0
        && response.ExactMatchDomains.Count > 0 
        &&response.ExactMatchDomains[0].Domain.DomainName.ToLowerInvariant() == domainName);
    }

    [TestMethod]
    public void TestInvalidGeoData()
    {
      var request = new MockHttpRequest("http://www.spoonymac.com");
      MockHttpContext.SetFromWorkerRequest(request);

      var domainName = string.Format("my-randoms-{0}.com", Guid.NewGuid().ToString().Substring(0, 15));

      var requestData = new DomainSearchRequestData(SHOPPER_ID, string.Empty, string.Empty, string.Empty, 0)
      {
        RequestTimeout = TimeSpan.FromSeconds(10),
        ClientIp = "172.16.172.211",
        CountrySite = "WWW",
        //DomainCount = 10,
        IncludeSpins = true,
        Language = "en",
        PrivateLabelId = 1,
        SearchPhrase = domainName,
        ShopperStatus = ShopperStatusType.Public,
        SourceCode = "mblDPPSearch",
        Tlds = new List<string>(0),
        DomainSearchDataBases = _databases,
        ClientIpLatitude = 34.1,
        ClientIpLongitude = -111.1,
        ClientIpCity = null,
        ClientIpCountry = null,
        ClientIpRegion = null,
        SplitTestId = "01234",
        SplitTestSideName = "A"
      };

      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, REQUESTID);
      Assert.IsTrue(response != null);
      Assert.IsTrue(response.Domains.Count > 0
        && response.ExactMatchDomains.Count > 0
        && response.ExactMatchDomains[0].Domain.DomainName.ToLowerInvariant() == domainName);
    }

    [TestMethod]
    public void TestUnavailableDomains()
    {
      var request = new MockHttpRequest("http://www.spoonymac.com");
      MockHttpContext.SetFromWorkerRequest(request);

      var domainName = "google.com";// string.Format("my-random-godaddy-{0}.com", Guid.NewGuid().ToString().Substring(0, 15));

      var requestData = new DomainSearchRequestData(SHOPPER_ID, string.Empty, string.Empty, string.Empty, 0)
      {
        RequestTimeout = TimeSpan.FromSeconds(10),
        ClientIp = "172.16.172.211",
        CountrySite = "WWW",
        //DomainCount = 10,
        IncludeSpins = true,
        Language = "en",
        PrivateLabelId = 1,
        SearchPhrase = domainName,
        ShopperStatus = ShopperStatusType.Public,
        SourceCode = "mblDPPSearch",
        Tlds = new List<string>(0),
        DomainSearchDataBases = _databases
      };

      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, REQUESTID);
      Assert.IsTrue(response != null);
      Assert.IsTrue(response.Domains.Count > 0);
      Assert.IsTrue(response.ExactMatchDomains.Count > 0);

      var domain = response.ExactMatchDomains.FirstOrDefault(d => d.Domain.DomainName.ToLowerInvariant() == domainName);
      Assert.IsTrue(domain != null && !domain.IsAvailable);
    }

    [TestMethod]
    public void TestSimilarDomains()
    {
      var request = new MockHttpRequest("http://www.spoonymac.com");
      MockHttpContext.SetFromWorkerRequest(request);

      var domainName = string.Format("my-random-{0}.com", Guid.NewGuid().ToString().Substring(0, 15));

      var requestData = new DomainSearchRequestData(SHOPPER_ID, string.Empty, string.Empty, string.Empty, 0)
      {
        RequestTimeout = TimeSpan.FromSeconds(10),
        ClientIp = "172.16.172.211",
        CountrySite = "WWW",
        //DomainCount = 10,
        IncludeSpins = true,
        Language = "en",
        PrivateLabelId = 1,
        SearchPhrase = domainName,
        ShopperStatus = ShopperStatusType.Public,
        SourceCode = "mblDPPSearch",
        Tlds = new List<string>(0),
        DomainSearchDataBases = _databases
      };

      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, REQUESTID);
      Assert.IsTrue(response != null);
      Assert.IsTrue(response.Domains.Count > 0);

      var hasSimiliarDomains = response.Domains.Any(d => d.Domain.DomainName.ToLowerInvariant().Contains("random"));
      Assert.IsTrue(hasSimiliarDomains);
    }

    [TestMethod]
    public void TestPremiumDomains()
    {
      var request = new MockHttpRequest("http://www.spoonymac.com");
      MockHttpContext.SetFromWorkerRequest(request);

      const string domainName = "getMe-a-premium.com";

      var requestData = new DomainSearchRequestData(SHOPPER_ID, string.Empty, string.Empty, string.Empty, 0)
      {
        RequestTimeout = TimeSpan.FromSeconds(10),
        ClientIp = "172.16.172.211",
        CountrySite = "WWW",
        //DomainCount = 10,
        IncludeSpins = true,
        Language = "en",
        PrivateLabelId = 1,
        SearchPhrase = domainName,
        ShopperStatus = ShopperStatusType.Public,
        SourceCode = "mblDPPSearch",
        Tlds = new List<string>(0),
        DomainSearchDataBases = _databases
      };

      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, REQUESTID);
      Assert.IsTrue(response != null);
      Assert.IsTrue(response.Domains.Count > 0);

      var hasPremiums = response.Domains.Any(d => d.AuctionType == "premium");
      Assert.IsTrue(hasPremiums);
    }

    [TestMethod]
    public void TestRequestedTld()
    {
      var request = new MockHttpRequest("http://www.spoonymac.com");
      MockHttpContext.SetFromWorkerRequest(request);

      const string domainName = "getMe-a-tld, get-me-another-tld";

      var requestData = new DomainSearchRequestData(SHOPPER_ID, string.Empty, string.Empty, string.Empty, 0)
      {
        RequestTimeout = TimeSpan.FromSeconds(10),
        ClientIp = "172.16.172.211",
        CountrySite = "WWW",
        //DomainCount = 10,
        IncludeSpins = true,
        Language = "en",
        PrivateLabelId = 1,
        SearchPhrase = domainName,
        ShopperStatus = ShopperStatusType.Public,
        SourceCode = "mblDPPSearch",
        Tlds = new List<string>{"org"},
        DomainSearchDataBases = _databases
      };

      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, REQUESTID);
      Assert.IsTrue(response != null);
      Assert.IsTrue(response.Domains.Count > 0);

      var hasAllOrg = response.Domains.Any(d => d.Domain.Tld.ToLowerInvariant() == "org");
      Assert.IsTrue(hasAllOrg);
    }

    [TestMethod]
    public void TestMultipleRequestedTlds()
    {
      var request = new MockHttpRequest("http://www.spoonymac.com");
      MockHttpContext.SetFromWorkerRequest(request);

      const string domainName = "getMe-a-tld, get-me-another-tld";

      var requestData = new DomainSearchRequestData(SHOPPER_ID, string.Empty, string.Empty, string.Empty, 0)
      {
        RequestTimeout = TimeSpan.FromSeconds(10),
        ClientIp = "172.16.172.211",
        CountrySite = "WWW",
        //DomainCount = 10,
        IncludeSpins = true,
        Language = "en",
        PrivateLabelId = 1,
        SearchPhrase = domainName,
        ShopperStatus = ShopperStatusType.Public,
        SourceCode = "mblDPPSearch",
        Tlds = new List<string> { "net,org,me" },
        DomainSearchDataBases = _databases
      };

      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, REQUESTID);
      Assert.IsTrue(response != null);
      Assert.IsTrue(response.Domains.Count > 0);

      var hasTlds = response.Domains.Any(d => d.Domain.Tld == "net") && response.Domains.Any(d => d.Domain.Tld == "org") &&
        response.Domains.Any(d => d.Domain.Tld == "me");
      Assert.IsTrue(hasTlds);
    }

    [TestMethod]
    public void TestUnicodeDomainSearch()
    {
      var request = new MockHttpRequest("http://www.spoonymac.com");
      MockHttpContext.SetFromWorkerRequest(request);
      var domainName = "тестнарусскомязыке.com";
      var requestData = new DomainSearchRequestData(SHOPPER_ID, string.Empty, string.Empty, string.Empty, 0)
        {
          RequestTimeout = TimeSpan.FromSeconds(10),
          ClientIp = "172.16.172.211",
          CountrySite = "WWW",
          IncludeSpins = true,
          Language = "en",
          PrivateLabelId = 1,
          SearchPhrase = domainName,
          ShopperStatus = ShopperStatusType.Public,
          SourceCode = "mblDPPSearch",
          Tlds = new List<string>(0),
          DomainSearchDataBases = _databases
        };
      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, REQUESTID);
      Assert.IsTrue(response != null);
      Assert.IsTrue(response.ExactMatchDomains.Count > 0);
      Assert.IsTrue(response.ExactMatchDomains[0].Domain.DomainName.ToLowerInvariant() == domainName);
      //"ExactDomains":[{"Extension":"com","DomainName":"тестнарусскомязыке.com","PunyCodeName":"xn--80ajbhobmflsidahct6lyc.com","PunyCodeExtension":"","NameWithoutExtension":"тестнарусскомязыке","Keys":[],"Data":[{"Name":"isavailable","Data":"true"},{"Name":"availcheckstatus","Data":"full"},{"Name":"isvaliddomain","Data":"true"},{"Name":"idnscript","Data":"[{\"Name\":\"eng\",\"Data\":\"35\"}]"},{"Name":"database","Data":"similar"},{"Name":"cartattributes","Data":"[{\"Name\":\"isoingo\",\"Data\":\"10\"}]"}]}]
    }

    [TestMethod]
    public void TestPunyCodeDomainSearch()
    {
      var request = new MockHttpRequest("http://www.spoonymac.com");
      MockHttpContext.SetFromWorkerRequest(request);
      var domainName = "xn--80ajbhobmflsidahct6lyc.com";
      var requestData = new DomainSearchRequestData(SHOPPER_ID, string.Empty, string.Empty, string.Empty, 0)
      {
        RequestTimeout = TimeSpan.FromSeconds(10),
        ClientIp = "172.16.172.211",
        CountrySite = "WWW",
        IncludeSpins = true,
        Language = "en",
        PrivateLabelId = 1,
        SearchPhrase = domainName,
        ShopperStatus = ShopperStatusType.Public,
        SourceCode = "mblDPPSearch",
        Tlds = new List<string>(0),
        DomainSearchDataBases = _databases
      };
      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, REQUESTID);
      Assert.IsTrue(response != null);
      var json = response.ToJson();
      var xml = response.ToXML();
      Assert.IsTrue(!string.IsNullOrEmpty(json) && string.IsNullOrEmpty(xml));
      Assert.IsTrue(response.ExactMatchDomains.Count > 0);
      Assert.IsTrue(response.ExactMatchDomains[0].Domain.PunyCodeDomainName == domainName);
    }

    [TestMethod]
    public void InvalidDomainTest()
    {
      var d = new Domain(string.Empty, string.Empty);
      Assert.IsTrue(string.IsNullOrEmpty(d.Sld) 
        && string.IsNullOrEmpty(d.Tld) 
        && string.IsNullOrEmpty(d.PunyCodeTld) 
        && string.IsNullOrEmpty(d.PunyCodeSld) 
        && string.IsNullOrEmpty(d.PunyCodeDomainName) 
        && string.IsNullOrEmpty(d.DomainName));
    }

    [TestMethod]
    public void DomainTest()
    {
      var d = new Domain("domain", "com");
      Assert.IsTrue(d.Sld == "domain");
      Assert.IsTrue(d.Tld == "com");
    }

    [TestMethod]
    public void IdnDomainTest()
    {
      var d = new Domain(string.Empty, string.Empty, string.Empty, string.Empty);
      Assert.IsTrue(string.IsNullOrEmpty(d.Sld)
        && string.IsNullOrEmpty(d.Tld)
        && string.IsNullOrEmpty(d.PunyCodeTld)
        && string.IsNullOrEmpty(d.PunyCodeSld)
        && string.IsNullOrEmpty(d.PunyCodeDomainName)
        && string.IsNullOrEmpty(d.DomainName));
      var d1 = new Domain("тестнарусскомязыке", "ком", "xn--80ajbhobmflsidahct6lyc", "xn--j1aef");
      Assert.IsTrue(d1.Sld == "тестнарусскомязыке"
        && d1.Tld == "ком"
        && d1.PunyCodeSld == "xn--80ajbhobmflsidahct6lyc"
        && d1.PunyCodeTld == "xn--j1aef"
        && d1.DomainName == "тестнарусскомязыке.ком"
        && d1.PunyCodeDomainName == "xn--80ajbhobmflsidahct6lyc.xn--j1aef");
    }

    [TestMethod]
    public void IdnConvertDomainTest()
    {
      var d = new Domain("xn--80ajbhobmflsidahct6lyc", "xn--j1aef");
      Assert.IsTrue(d.Sld == "тестнарусскомязыке"
        && d.Tld == "ком"
        && d.PunyCodeSld == "xn--80ajbhobmflsidahct6lyc"
        && d.PunyCodeTld == "xn--j1aef"
        && d.DomainName == "тестнарусскомязыке.ком"
        && d.PunyCodeDomainName == "xn--80ajbhobmflsidahct6lyc.xn--j1aef");
    }

    [TestMethod]
    public void InPreRegPhraseTest()
    {
      var request = new MockHttpRequest("http://www.spoonymac.com");
      MockHttpContext.SetFromWorkerRequest(request);
      var domainName = "sdf34sdfsdfsdfsdf.menu";
      var requestData = new DomainSearchRequestData(SHOPPER_ID, string.Empty, string.Empty, string.Empty, 0)
      {
        RequestTimeout = TimeSpan.FromSeconds(10),
        ClientIp = "172.16.172.211",
        CountrySite = "WWW",
        IncludeSpins = true,
        Language = "en",
        PrivateLabelId = 1,
        SearchPhrase = domainName,
        ShopperStatus = ShopperStatusType.Public,
        SourceCode = "mblDPPSearch",
        Tlds = new List<string>(0),
        DomainSearchDataBases = _databases
      };
      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, REQUESTID);

      var domain = response.ExactMatchDomains[0];

      Assert.IsTrue(domain.InPreRegPhase);
      Assert.IsTrue(domain.LaunchPhaseItems.Any());

      var sunrise = domain.LaunchPhaseItems.First(p => p.LaunchPhase == LaunchPhases.SunriseA);
      var landursh = domain.LaunchPhaseItems.First(p => p.LaunchPhase == LaunchPhases.Landrush);

      Assert.IsTrue(sunrise.TierId.Value > 0);
      Assert.IsFalse(landursh.TierId.HasValue);
    }
  }
}