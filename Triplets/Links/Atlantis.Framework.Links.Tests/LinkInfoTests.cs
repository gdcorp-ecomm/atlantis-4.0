using afe = Atlantis.Framework.Engine;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Links.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Atlantis.Framework.Links.Tests
{
  [TestClass]
  [DeploymentItem(afeConfig)]
  [DeploymentItem("Atlantis.Framework.Links.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Links.MockImpl.dll")]
  public class LinkInfoTests
  {
    public const string afeConfig = "atlantis.config";

    private const int kEngineRequest = 731;

    private enum ContextIds
    {
      Invalid = -1,
      NoContext = 0,
      GoDaddy = 1
    }

    [ClassInitialize]
    public static void InitTests(TestContext c)
    {
      Engine.Engine.ReloadConfig(afeConfig);
    }

    [TestInitialize]
    public void TestInit()
    {
    }

    [TestCleanup]
    public void TestCleanup()
    {
    }

    [TestMethod]
    public void LinkInfoBasic()
    {
      var request = new LinkInfoRequestData((int)ContextIds.GoDaddy);
      var response = (LinkInfoResponseData)afe.Engine.ProcessRequest(request, kEngineRequest);
      Assert.AreNotEqual(0, response.Links.Count);
      bool atleastOneRealCountryParamValue = false;
      bool atleastOneRealCountrySupportTypeValue = false;
      bool atleastOneRealLanguageParamValue = false;
      bool atleastOneRealLanguageSupportTypeValue = false;
      foreach (var item in response.Links)
      {
        if (!String.IsNullOrWhiteSpace(item.Value.CountryParameter))
        {
          atleastOneRealCountryParamValue = true;
        }

        switch( item.Value.CountrySupportType)
        {
          case LinkTypeCountrySupport.PrefixHostNameSupport:
          case LinkTypeCountrySupport.ReplaceHostNameSupport:
          case LinkTypeCountrySupport.QueryStringSupport:
            atleastOneRealCountrySupportTypeValue = true;
            break;
        }

        if (!String.IsNullOrWhiteSpace(item.Value.LanguageParameter))
        {
          atleastOneRealLanguageParamValue = true;
        }

        switch( item.Value.LanguageSupportType)
        {
          case LinkTypeLanguageSupport.PrefixPathSupport:
          case LinkTypeLanguageSupport.QueryStringSupport:
            atleastOneRealLanguageSupportTypeValue = true;
            break;
        }

        Assert.IsNotNull(item.Value.CountryParameter);
        Assert.IsTrue( Enum.IsDefined(typeof(LinkTypeCountrySupport), item.Value.CountrySupportType ) );
        Assert.IsNotNull(item.Value.LanguageParameter);
        Assert.IsTrue( Enum.IsDefined(typeof(LinkTypeLanguageSupport), item.Value.LanguageSupportType ) );
      }
      Assert.IsTrue(atleastOneRealCountryParamValue && atleastOneRealCountrySupportTypeValue && atleastOneRealLanguageParamValue && atleastOneRealLanguageSupportTypeValue);
    }

    [TestMethod]
    public void LinkInfoMissingForNoContext()
    {
      var request = new LinkInfoRequestData((int)ContextIds.NoContext);
      var response = (LinkInfoResponseData)afe.Engine.ProcessRequest(request, kEngineRequest);
      Assert.AreEqual(0, response.Links.Count);
      Assert.AreEqual(0, response.LinkTypesByBaseUrl.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(AtlantisException))]
    public void LinkInfoExceptionForInvalidContext()
    {
      var request = new LinkInfoRequestData((int)ContextIds.Invalid);
      var response = (LinkInfoResponseData)afe.Engine.ProcessRequest(request, kEngineRequest);
    }

    [TestMethod]
    public void LinkInfoCannotSoilTheCache()
    {
      var request = new LinkInfoRequestData((int)ContextIds.GoDaddy);
      var response = (LinkInfoResponseData)afe.Engine.ProcessRequest(request, kEngineRequest);

      try
      {
        response.Links.Clear();
      }
      catch (Exception ex)
      {
        Assert.IsTrue(ex is InvalidOperationException);
      }

      try
      {
        response.Links.Add("SITEURL", new LinkInfoImpl
          {
            BaseUrl = "www.readonly.com",
            CountrySupportType = LinkTypeCountrySupport.ReplaceHostNameSupport,
            CountryParameter = String.Empty,
            LanguageSupportType = LinkTypeLanguageSupport.QueryStringSupport,
            LanguageParameter = String.Empty
          });
      }
      catch (Exception ex)
      {
        Assert.IsTrue(ex is InvalidOperationException);
      }

      try
      {
        response.Links["SITEURL"] = new LinkInfoImpl();
      }
      catch (Exception ex)
      {
        Assert.IsTrue(ex is InvalidOperationException);
      }

      try
      {
        response.Links.Remove("SITEURL");
      }
      catch (Exception ex)
      {
        Assert.IsTrue(ex is InvalidOperationException);
      }

    }

    [TestMethod]
    public void LinkInfoGetLinkInfoByBaseUrl()
    {
      string hostToFind = "idp.dev.GoDaDdy-CoM.IDE";
      
      var request = new LinkInfoRequestData((int)ContextIds.GoDaddy);
      var response = (LinkInfoResponseData)afe.Engine.ProcessRequest(request, kEngineRequest);

      string linkTypeReturnedByDictKey = response.LinkTypesByBaseUrl[hostToFind];
      Assert.AreEqual("SSOURL", linkTypeReturnedByDictKey);
    }
  }
}
