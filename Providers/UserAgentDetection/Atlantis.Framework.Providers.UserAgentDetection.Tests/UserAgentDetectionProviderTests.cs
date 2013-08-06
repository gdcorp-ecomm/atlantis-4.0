using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.UserAgentDetection.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.UserAgentDetection.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.UserAgentEx.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  public class UserAgentDetectionProviderTests
  {
    private IProviderContainer _providerContainer;
    private IProviderContainer ProviderContainer
    {
      get
      {
        if (_providerContainer == null)
        {
          _providerContainer = new MockProviderContainer();
          _providerContainer.RegisterProvider<IUserAgentDetectionProvider, UserAgentDetectionProvider>();
        }
        return _providerContainer;
      }
    }

    private XDocument _userAgentXml;
    private XDocument UserAgentXml
    {
      get
      {
        string resourcePath = "Atlantis.Framework.Providers.UserAgentDetection.Tests.useragents.xml";
        Assembly assembly = Assembly.GetExecutingAssembly();
        using (StreamReader textReader = new StreamReader(assembly.GetManifestResourceStream(resourcePath)))
        {
          _userAgentXml = XDocument.Load(textReader);
        }

        return _userAgentXml;
      }
    }

    private bool IsResultValid(string type, bool isMobileDevice, bool isNoRedirectBrowser, bool isOutDatedBrowser, bool isSearchEngineBot)
    {
      bool isResultValid = false;

      switch (type.ToLowerInvariant())
      {
        case "bot":
          isResultValid = isSearchEngineBot;
          break;
        case "internal":
          isResultValid = !isMobileDevice && !isNoRedirectBrowser && !isOutDatedBrowser && !isSearchEngineBot;
          break;
        case "mobile":
          isResultValid = isMobileDevice;
          break;
        case "browser":
          isResultValid = !isMobileDevice && !isNoRedirectBrowser && !isOutDatedBrowser && !isSearchEngineBot;
          break;
        case "legacybrowser":
          isResultValid = isOutDatedBrowser;
          break;
        case "noredirect":
          isResultValid = isNoRedirectBrowser;
          break;
      }

      return isResultValid;
    }

    [TestMethod]
    public void UserAgentNullTest()
    {
      IUserAgentDetectionProvider userAgentDetectionProvider = ProviderContainer.Resolve<IUserAgentDetectionProvider>();

      bool isMobileDevice = userAgentDetectionProvider.IsMobileDevice(null);
      bool isNoRedirectBrowser = userAgentDetectionProvider.IsNoRedirectBrowser(null);
      bool isOutDatedBrowser = userAgentDetectionProvider.IsOutDatedBrowser(null);
      bool isSearchEngineBot = userAgentDetectionProvider.IsSearchEngineBot(null);

      Assert.IsFalse(isMobileDevice);
      Assert.IsTrue(isNoRedirectBrowser);
      Assert.IsFalse(isOutDatedBrowser);
      Assert.IsFalse(isSearchEngineBot);
    }

    [TestMethod]
    public void UserAgentEmptyTest()
    {
      IUserAgentDetectionProvider userAgentDetectionProvider = ProviderContainer.Resolve<IUserAgentDetectionProvider>();

      bool isMobileDevice = userAgentDetectionProvider.IsMobileDevice(string.Empty);
      bool isNoRedirectBrowser = userAgentDetectionProvider.IsNoRedirectBrowser(string.Empty);
      bool isOutDatedBrowser = userAgentDetectionProvider.IsOutDatedBrowser(string.Empty);
      bool isSearchEngineBot = userAgentDetectionProvider.IsSearchEngineBot(string.Empty);

      Assert.IsFalse(isMobileDevice);
      Assert.IsTrue(isNoRedirectBrowser);
      Assert.IsFalse(isOutDatedBrowser);
      Assert.IsFalse(isSearchEngineBot);
    }

    [TestMethod]
    public void UserAgentXmlTest()
    {
      IEnumerable<XElement> userAgentElements = UserAgentXml.Element("UserAgents").Elements("UserAgent");
      IUserAgentDetectionProvider userAgentDetectionProvider = ProviderContainer.Resolve<IUserAgentDetectionProvider>();

      foreach (XElement userAgentElement in userAgentElements)
      {
        string name = userAgentElement.Attribute("name").Value;
        string type = userAgentElement.Attribute("type").Value;
        string userAgent = userAgentElement.Value;


        bool isMobileDevice = userAgentDetectionProvider.IsMobileDevice(userAgent);
        bool isNoRedirectBrowser = userAgentDetectionProvider.IsNoRedirectBrowser(userAgent);
        bool isOutDatedBrowser = userAgentDetectionProvider.IsOutDatedBrowser(userAgent);
        bool isSearchEngineBot = userAgentDetectionProvider.IsSearchEngineBot(userAgent);

        if (!IsResultValid(type, isMobileDevice, isNoRedirectBrowser, isOutDatedBrowser, isSearchEngineBot))
        {
          Assert.Fail("Detection Invalid. User Agent: {0} | Name: {1} | Type: {2} | IsMobileDevice: {3} | IsNoRedirectBrowser: {4} | IsOutDatedBrowser: {5} | IsSearchEngineBot: {6}", userAgent, name, type, isMobileDevice, isNoRedirectBrowser, isOutDatedBrowser, isSearchEngineBot);
        }
      }
    }
  }
}
