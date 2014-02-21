using System;
using System.Globalization;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.LogDomainSearchResults.Interface;
using Atlantis.Framework.Providers.LogDomainSearchResults.Interface;
using Atlantis.Framework.Testing.MockEngine;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.LogDomainSearchResults.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Providers.LogDomainSearchResults.Impl.dll")]
  public class LogDomainSearchResultsTests
  {
    private const string _KEY = "Providers.LogDomainSearchResultsProvider";

    [TestInitialize]
    public void Initialize()
    {
      var request = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(request);
    }

    private IProviderContainer _providerContainer;
    private IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          _providerContainer = new MockProviderContainer();
          _providerContainer.SetData(MockSiteContextSettings.IsRequestInternal, true);

          _providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
          _providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
          _providerContainer.RegisterProvider<ILogDomainSearchResultsProvider, LogDomainSearchResultsProvider>();
        }

        return _providerContainer;
      }
    }

    [TestMethod]
    public void LogSearchResultWiException()
    {
      var logDomainSearchResults = ProviderContainer.Resolve<ILogDomainSearchResultsProvider>();

      Func<RequestData, ConfigElement, IResponseData> mockImpl = (data, element) =>
      {
        Assert.IsInstanceOfType(data, typeof(LogDomainSearchResultsRequestData));
        var typedData = (LogDomainSearchResultsRequestData)data;
        Assert.AreEqual(string.Empty, typedData.Domain);
        Assert.AreEqual(0, typedData.Availability);
        return new LogDomainSearchResultsResponseData(typedData, new AtlantisException("LogSearchResultWiException", 0, "Testing Exception", string.Format("Domain:{0}, Availability:{1}", typedData.Domain, typedData.Availability)));
      };

      EngineRequestMocking.RegisterOverride(130, mockImpl);

      try
      {
        logDomainSearchResults.SubmitLog(HttpContext.Current.Request.Url.ToString());
      }
      finally
      {
        EngineRequestMocking.ClearOverrides();
      }
    }

    [TestMethod]
    public void LogSearchResultWoSearchedDomain()
    {
      var logDomainSearchResults = ProviderContainer.Resolve<ILogDomainSearchResultsProvider>();

      Func<RequestData, ConfigElement, IResponseData> mockImpl = (data, element) =>
      {
        Assert.IsInstanceOfType(data, typeof(LogDomainSearchResultsRequestData));
        var typedData = (LogDomainSearchResultsRequestData)data;
        Assert.AreEqual(string.Empty, typedData.Domain);
        Assert.AreEqual(0, typedData.Availability);
        return new LogDomainSearchResultsResponseData();
      };

      EngineRequestMocking.RegisterOverride(130, mockImpl);

      try
      {
        logDomainSearchResults.SubmitLog(HttpContext.Current.Request.Url.ToString());
      }
      finally
      {
        EngineRequestMocking.ClearOverrides();
      }
    }

    [TestMethod]
    public void LogSearchResultWoSearchedDomainWiSuggested()
    {
      var logDomainSearchResults = ProviderContainer.Resolve<ILogDomainSearchResultsProvider>();
      logDomainSearchResults.AddSuggestedDomain("xyz.net", 1, 3, 1, "1099", 0, 0, 1);
      logDomainSearchResults.AddSuggestedDomain("xyz.info", 1, 1, 2, "1099", 0, 0, 1);

      Func<RequestData, ConfigElement, IResponseData> mockImpl = (data, element) =>
      {
        Assert.IsInstanceOfType(data, typeof(LogDomainSearchResultsRequestData));
        var typedData = (LogDomainSearchResultsRequestData)data;
        Assert.AreEqual(string.Empty, typedData.Domain);
        Assert.AreEqual(0, typedData.Availability);
        return new LogDomainSearchResultsResponseData();
      };

      EngineRequestMocking.RegisterOverride(130, mockImpl);

      try
      {
        logDomainSearchResults.SubmitLog(HttpContext.Current.Request.Url.ToString());
      }
      finally
      {
        EngineRequestMocking.ClearOverrides();
      }
    }

    [TestMethod]
    public void LogSearchResult()
    {
      var logDomainSearchResults = ProviderContainer.Resolve<ILogDomainSearchResultsProvider>();
      logDomainSearchResults.SetSearchedDomain("xyz.com", 1);

      Func<RequestData, ConfigElement, IResponseData> mockImpl = (data, element) =>
      {
        Assert.IsInstanceOfType(data, typeof(LogDomainSearchResultsRequestData));
        var typedData = (LogDomainSearchResultsRequestData)data;
        Assert.AreEqual("xyz.com", typedData.Domain);
        Assert.AreEqual(1, typedData.Availability);
        return new LogDomainSearchResultsResponseData();
      };

      EngineRequestMocking.RegisterOverride(130, mockImpl);

      try
      {
        logDomainSearchResults.SubmitLog(HttpContext.Current.Request.Url.ToString());
      }
      finally
      {
        EngineRequestMocking.ClearOverrides();
      }
    }

    [TestMethod]
    public void LogSearchResultWiSuggested()
    {
      var logDomainSearchResults = ProviderContainer.Resolve<ILogDomainSearchResultsProvider>();
      logDomainSearchResults.SetSearchedDomain("xyz.com", 1);
      logDomainSearchResults.AddSuggestedDomain("xyz.net", 1, 3, 1, "1099", 0, 0, 1);
      logDomainSearchResults.AddSuggestedDomain("xyz.info", 1, 1, 2, "1099", 0, 0, 1);

      Func<RequestData, ConfigElement, IResponseData> mockImpl = (data, element) =>
      {
        Assert.IsInstanceOfType(data, typeof(LogDomainSearchResultsRequestData));
        var typedData = (LogDomainSearchResultsRequestData)data;
        Assert.AreEqual("xyz.com", typedData.Domain);
        Assert.AreEqual(1, typedData.Availability);
        return new LogDomainSearchResultsResponseData();
      };

      EngineRequestMocking.RegisterOverride(130, mockImpl);

      try
      {
        logDomainSearchResults.SubmitLog(HttpContext.Current.Request.Url.ToString());
      }
      finally
      {
        EngineRequestMocking.ClearOverrides();
      }
    }
  }
}
