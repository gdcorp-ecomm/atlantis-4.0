using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.DomainSearch.Interface;
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
    const int _REQUESTID = 714;
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
                            DomainSearchDataBases = _databases
                          };

      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, _REQUESTID);
      Assert.IsTrue(response != null);
      Assert.IsTrue(response.Domains.Count > 0);
      Assert.IsTrue(response.ExactMatchDomains.Count > 0);
      Assert.IsTrue(response.ExactMatchDomains[0].Domain.DomainName.ToLowerInvariant() == domainName);
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

      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, _REQUESTID);
      Assert.IsTrue(response != null);
      Assert.IsTrue(response.Domains.Count > 0);
      Assert.IsTrue(response.ExactMatchDomains.Count > 0);

      var domain = response.ExactMatchDomains.FirstOrDefault(d => d.Domain.DomainName.ToLowerInvariant() == domainName);
      Assert.IsTrue(!domain.IsAvailable);
    }

    [TestMethod]
    public void TestSimilarDomains()
    {
      var request = new MockHttpRequest("http://www.spoonymac.com");
      MockHttpContext.SetFromWorkerRequest(request);

      var domainName = string.Format("my-random-godaddy-{0}.com", Guid.NewGuid().ToString().Substring(0, 15));

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

      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, _REQUESTID);
      Assert.IsTrue(response != null);
      Assert.IsTrue(response.Domains.Count > 0);

      var hasSimiliarDomains = response.Domains.Any(d => d.Domain.DomainName.ToLowerInvariant().Contains("daddy"));
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

      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, _REQUESTID);
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

      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, _REQUESTID);
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

      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, _REQUESTID);
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
      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, _REQUESTID);
      Assert.IsTrue(response != null);
      Assert.IsTrue(response.ExactMatchDomains.Count > 0);
      Assert.IsTrue(response.ExactMatchDomains[0].Domain.DomainName.ToLowerInvariant() == domainName);
      //"ExactDomains":[{"Extension":"com","DomainName":"тестнарусскомязыке.com","PunnyCodeName":"xn--80ajbhobmflsidahct6lyc.com","PunnyCodeExtension":"","NameWithoutExtension":"тестнарусскомязыке","Keys":[],"Data":[{"Name":"isavailable","Data":"true"},{"Name":"availcheckstatus","Data":"full"},{"Name":"isvaliddomain","Data":"true"},{"Name":"idnscript","Data":"[{\"Name\":\"eng\",\"Data\":\"35\"}]"},{"Name":"database","Data":"similar"},{"Name":"cartattributes","Data":"[{\"Name\":\"isoingo\",\"Data\":\"10\"}]"}]}]
    }

    [TestMethod]
    public void TestPunnyCodeDomainSearch()
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
      var response = (DomainSearchResponseData)Engine.Engine.ProcessRequest(requestData, _REQUESTID);
      Assert.IsTrue(response != null);
      Assert.IsTrue(response.ExactMatchDomains.Count > 0);
      Assert.IsTrue(response.ExactMatchDomains[0].Domain.PunnyCodeDomainName == domainName);
    }
  }
}