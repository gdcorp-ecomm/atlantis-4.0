using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Links.Interface;
using Atlantis.Framework.Links.MockImpl;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using afe = Atlantis.Framework.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.Links.Tests
{
  /// <summary>
  /// This class tests the LinkProvider using a mock'd database in xyzfakeData. 
  /// Add to this class when you have misc tests that exercise the LinkProvider w/o a need to match
  /// backend database values.
  /// </summary>
  [TestClass]
  [DeploymentItem(afeConfig)]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.Links.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Links.MockImpl.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  public class LinkProviderTestsWithoutLocalization
  {
    private static Dictionary<string, ILinkInfo> gdfakeData;

    public const string afeConfig = "atlantis.nolocalization.config";

    private MockProviderContainer FrameworkContainer;

    [ClassInitialize]
    public static void ClassInit(TestContext c)
    {
      gdfakeData = new Dictionary<string, ILinkInfo>
        {
          { 
            "MYAURL", 
            new LinkInfoImpl
              {
                BaseUrl = "mya.godaddy.com",
                CountrySupportType = LinkTypeCountrySupport.NoSupport,
                CountryParameter = String.Empty,
                LanguageSupportType = LinkTypeLanguageSupport.NoSupport,
                LanguageParameter = String.Empty
              }
          },
          { 
            "SITEURL", 
            new LinkInfoImpl
              {
                BaseUrl = "www.godaddy.com",
                CountrySupportType = LinkTypeCountrySupport.ReplaceHostNameSupport,
                CountryParameter = String.Empty,
                LanguageSupportType = LinkTypeLanguageSupport.PrefixPathSupport,
                LanguageParameter = String.Empty
              }
          },
          { 
            "CARTURL", 
            new LinkInfoImpl
              {
                BaseUrl = "cart.godaddy.com",
                CountrySupportType = LinkTypeCountrySupport.NoSupport,
                CountryParameter = String.Empty,
                LanguageSupportType = LinkTypeLanguageSupport.PrefixPathSupport,
                LanguageParameter = String.Empty
              }
          },
          { 
            "DCCURL", 
            new LinkInfoImpl
              {
                BaseUrl = "us.dcc.godaddy.com/mypage.aspx",
                CountrySupportType = LinkTypeCountrySupport.ReplaceHostNameSupport,
                CountryParameter = String.Empty,
                LanguageSupportType = LinkTypeLanguageSupport.QueryStringSupport,
                LanguageParameter = "lang"
              }
          },
          { 
            "SSLURL", 
            new LinkInfoImpl
              {
                BaseUrl = "certificates.godaddy.com",
                CountrySupportType = LinkTypeCountrySupport.PrefixHostNameSupport,
                CountryParameter = String.Empty,
                LanguageSupportType = LinkTypeLanguageSupport.PrefixPathSupport,
                LanguageParameter = "lang"
              }
          },
          { 
            "SUPPORTURL", 
            new LinkInfoImpl
              {
                BaseUrl = "support.godaddy.com",
                CountrySupportType = LinkTypeCountrySupport.QueryStringSupport,
                CountryParameter = "cntry",
                LanguageSupportType = LinkTypeLanguageSupport.QueryStringSupport,
                LanguageParameter = "lang"
              }
          }
        };

      afe.Engine.ReloadConfig(afeConfig);

      ReloadCache();
    }

    [TestCleanup]
    public void TestCleanup()
    {
      FrameworkContainer = null;
    }

    [TestInitialize]
    public void TestInit()
    {
      FrameworkContainer = new MockProviderContainer();

      FrameworkContainer.RegisterProvider<ISiteContext, MockSiteContext>();
      FrameworkContainer.RegisterProvider<IShopperContext, MockShopperContext>();
      FrameworkContainer.RegisterProvider<IManagerContext, MockManagerContext>();
      FrameworkContainer.RegisterProvider<ILinkProvider, LinkProvider>();
    }

    private static void ReloadCache()
    {
      foreach (var config in Engine.Engine.GetConfigElements())
      {
        DataCache.DataCache.ClearCachedData(config.RequestType);
      }
    }

    private ILinkProvider NewLinkProvider(string url, int privateLabelId, string shopperId, bool isInternal = false, bool isManager = false, IPAddress userHostAddress = null)
    {
      var request = new MockHttpRequest(url);
      if (userHostAddress != null)
      {
        request.MockRemoteAddress(userHostAddress);
      }

      MockHttpContext.SetFromWorkerRequest(request);

      FrameworkContainer.SetMockSetting(MockSiteContextSettings.IsRequestInternal, isInternal);
      FrameworkContainer.SetMockSetting(MockSiteContextSettings.PrivateLabelId, privateLabelId);

      if (isManager)
      {
        FrameworkContainer.SetMockSetting(MockManagerContextSettings.IsManager, isManager);
        FrameworkContainer.SetMockSetting(MockManagerContextSettings.PrivateLabelId, privateLabelId);
        FrameworkContainer.SetMockSetting(MockManagerContextSettings.ShopperId, shopperId);
      }
      else
      {
        var shopperContext = FrameworkContainer.Resolve<IShopperContext>();
        shopperContext.SetNewShopper(shopperId);
      }

      HttpContext.Current.Items[MockLinkInfoRequestContextSettings.LinkInfoTable + ".1"] = gdfakeData;

      var linkProvider = FrameworkContainer.Resolve<ILinkProvider>();
      return linkProvider;
    }

    [TestMethod]
    public void LinkWithoutLocalization()
    {
      string homePage = "http://www.godaddy.com/default.aspx";
      var links = NewLinkProvider(homePage, 1, "832652");
      string url = links.GetUrl("SSLURL", "");
      Assert.IsTrue(url.IndexOf("//", 8, StringComparison.OrdinalIgnoreCase) == -1);

      // verify the host entry doesn't have a country
      Assert.IsTrue(url.Contains("http://certificates.godaddy"));
      // verify the host entry didn't get the WWW code
      Assert.IsFalse(url.Contains("www."));
      Assert.IsFalse(url.Contains("us."));
      // verify the language is present
      Assert.IsFalse(url.Contains("/en"));

      // ensure country and language didn't showup as query params
      int iQueryStart = url.IndexOf('?');
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=en-us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=us", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
      Assert.IsFalse(iQueryStart != -1 && url.IndexOf("=www", iQueryStart, StringComparison.OrdinalIgnoreCase) != -1);
    }

  }
}