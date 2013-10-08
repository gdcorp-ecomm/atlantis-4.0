using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.CDSContent.Interface;
using Atlantis.Framework.Providers.PlaceHolder;
using Atlantis.Framework.Providers.PlaceHolder.Interface;
using Atlantis.Framework.Render.Pipeline;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlantis.Framework.Providers.CDSContent.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.CDS.Impl.dll")]
  public class VersionSpoofingTests
  {
    const string APP_NAME = "sales/unittest";

    public static IProviderContainer ProviderContainer
    {
      get
      {
        IProviderContainer providerContainer = new MockProviderContainer();

        providerContainer.SetData<bool>("MockSiteContextSettings.IsRequestInternal", true);

        providerContainer.RegisterProvider<ISiteContext, MockSiteContext>();
        providerContainer.RegisterProvider<IShopperContext, MockShopperContext>();
        providerContainer.RegisterProvider<IManagerContext, MockManagerContext>();
        providerContainer.RegisterProvider<ICDSContentProvider, CDSContentProvider>();
        providerContainer.RegisterProvider<IPlaceHolderProvider, PlaceHolderProvider>();

        return providerContainer;
      }
    }

    private void SetUrl(string url)
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest(url);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
    }

    [TestMethod]
    public void Can_A_WhiteListVersion_Be_Spoofed()
    {
      string url = "http://www.debug.godaddy-com.ide/home?version=sales/unittest/whitelist|52532ac4f778fc15d8df6d32";
      SetUrl(url);

      CDSContentProvider provider = new CDSContentProvider(ProviderContainer);
      var result = provider.CheckWhiteList(APP_NAME, "/blahblah/blahblah-hosting");

      Assert.IsTrue(result.Exists);
    }

    [TestMethod]
    public void Can_A_RuleFileVersion_Be_Spoofed()
    {
      string url = "http://www.debug.godaddy-com.ide/home?version=sales/unittest/onlytrue_redirectruletests.rule|52535253f778fc15d8df6d40";
      SetUrl(url);

      CDSContentProvider provider = new CDSContentProvider(ProviderContainer);
      var result = provider.CheckRedirectRules(APP_NAME, "onlytrue_redirectruletests");

      Assert.IsTrue(result.RedirectRequired);
      Assert.AreEqual<string>(result.RedirectData.Location, "blah blah blah blah");
    }

    [TestMethod]
    public void Can_A_ContentVersion_Be_Spoofed()
    {
      string url = "http://www.debug.godaddy-com.ide/home?version=sales/unittest/producthassavingsmorethan|52535572f778fc15d8df6d42";
      SetUrl(url);

      CDSContentProvider provider = new CDSContentProvider(ProviderContainer);
      var result = provider.GetContent(APP_NAME, "producthassavingsmorethan");

      Assert.IsTrue(result.Content.Contains("42002"));
    }


    [TestMethod]
    public void Can_both_ContentVersion_and_CDSPlaceHolderVersion_Be_Spoofed()
    {
      string url = "http://www.debug.godaddy-com.ide/home?version=atlantis/_global/nesteddocument|5254834bf778fc3be4d63525,atlantis/_global/bannerimage|525487f3f778fc3be4d63526";
      SetUrl(url);

      CDSContentProvider provider = new CDSContentProvider(ProviderContainer);
      var result = provider.GetContent("atlantis", "_global/nesteddocument");

      IPlaceHolderProvider placeHolderProvider = ProviderContainer.Resolve<IPlaceHolderProvider>();

      string finalContent = placeHolderProvider.ReplacePlaceHolders(result.Content, null);

      Assert.IsTrue(finalContent.Contains("blah blah blah blah"));
      Assert.IsTrue(finalContent.Contains("zoom zoom zoom zoom"));
    }

  }
}
