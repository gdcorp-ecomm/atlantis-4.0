using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Currency.Tests.Mocks;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Providers.Localization.Interface;
using Atlantis.Framework.Providers.Shopper.Interface;
using Atlantis.Framework.Testing.MockPreferencesProvider;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Atlantis.Framework.Providers.Currency.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.EcommPricing.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.Currency.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PLSignupInfo.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  public class CurrencyPreferenceTests
  {
    private IProviderContainer SetProviders(string currencyPreference = null, string currencyProfile = null, string shopperId = "", ICountrySite countrySite = null, int privateLabelId = 1)
    {
      MockProviderContainer result = new MockProviderContainer();

      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IShopperContext, MockShopperContext>();

      if (privateLabelId != 1)
      {
        result.SetData(MockSiteContextSettings.PrivateLabelId, privateLabelId);
      }

      if (!string.IsNullOrEmpty(shopperId))
      {
        var shopperContext = result.Resolve<IShopperContext>();
        shopperContext.SetNewShopper(shopperId);
      }

      if (currencyPreference != null)
      {
        result.RegisterProvider<IShopperPreferencesProvider, MockShopperPreference>();
        var preferences = result.Resolve<IShopperPreferencesProvider>();
        if (currencyPreference.Length > 0)
        {
          preferences.UpdatePreference("currency", currencyPreference);
        }
      }

      if (currencyProfile != null)
      {
        result.RegisterProvider<IShopperDataProvider, MockShopperDataProvider>();
        Dictionary<string, string> profileData = new Dictionary<string, string>();
        if (currencyProfile.Length > 0)
        {
          profileData[ShopperDataFields.CurrencyType] = currencyProfile;
        }
        result.SetData(MockShopperDataProvider.MockProperties.ShopperData, profileData);
      }

      if (countrySite != null)
      {
        result.RegisterProvider<ILocalizationProvider, Atlantis.Framework.Testing.MockLocalization.MockLocalizationProvider>();
        result.SetData(Atlantis.Framework.Testing.MockLocalization.MockLocalizationProviderSettings.CountrySiteInfo, countrySite);
      }

      return result;
    }

    [TestMethod]
    public void NoProvidersAvailable()
    {
      var container = SetProviders(null, null, "832652");
      var currencyPreference = new CurrencyPreference(container);
      currencyPreference.SetCurrencyPreference("USD");
      Assert.AreEqual("USD", currencyPreference.GetCurrencyPreference());
    }

    [TestMethod]
    public void OnlyShopperDataAvailableNoShopperId()
    {
      var container = SetProviders(null, "JPY");
      var currencyPreference = new CurrencyPreference(container);
      currencyPreference.SetCurrencyPreference("USD");
      Assert.AreEqual("USD", currencyPreference.GetCurrencyPreference());
    }

    [TestMethod]
    public void OnlyShopperDataAvailable()
    {
      var container = SetProviders(null, "JPY", "832652");
      var currencyPreference = new CurrencyPreference(container);
      currencyPreference.SetCurrencyPreference("USD");
      Assert.AreEqual("JPY", currencyPreference.GetCurrencyPreference());
    }

    [TestMethod]
    public void OnlyShopperDataAvailableFieldEmpty()
    {
      var container = SetProviders(null, "", "832652");
      var currencyPreference = new CurrencyPreference(container);
      currencyPreference.SetCurrencyPreference("USD");
      Assert.AreEqual("USD", currencyPreference.GetCurrencyPreference());
    }

    [TestMethod]
    public void OnlyPreferencesAvailable()
    {
      var container = SetProviders("JPY");
      var currencyPreference = new CurrencyPreference(container);
      Assert.AreEqual("JPY", currencyPreference.GetCurrencyPreference());
      currencyPreference.SetCurrencyPreference("CAD");     
    }

    [TestMethod]
    public void OnlyPreferencesAvailableButPreferenceEmpty()
    {
      var container = SetProviders("");
      var currencyPreference = new CurrencyPreference(container);
      Assert.AreEqual("USD", currencyPreference.GetCurrencyPreference());
    }

    [TestMethod]
    public void OnlyLocalizationAvailable()
    {
      var mockCountrySite = new Atlantis.Framework.Testing.MockLocalization.MockCountrySiteInfo("au", "Australia", 0, false, "AUD", "en-AU");
      var container = SetProviders(countrySite: mockCountrySite);
      var currencyPreference = new CurrencyPreference(container);
      Assert.AreEqual("AUD", currencyPreference.GetCurrencyPreference());
    }

    [TestMethod]
    public void PlSignupInfoDefaultMCPOn()
    {
      /// Fragile test... if it ever breaks because 281896 causes issue, then create a mock plsignupinforequest handler
      /// to give back exact test data that has MCP turned on and uses a different currency than USD (change test assert)
      var container = SetProviders(privateLabelId: 281896);
      var currencyPreference = new CurrencyPreference(container);
      Assert.AreEqual("USD", currencyPreference.GetCurrencyPreference());
    }

    [TestMethod]
    public void PlSignupInfoDefaultMCPOff()
    {
      /// Fragile test... if it ever breaks because 1724 causes issue, then create a mock plsignupinforequest handler
      /// to give back exact test data that has MCP turned off
      var container = SetProviders(privateLabelId: 1724);
      var currencyPreference = new CurrencyPreference(container);
      Assert.AreEqual("USD", currencyPreference.GetCurrencyPreference());
    }

    [TestMethod]
    public void PreferencesBeforeProfile()
    {
      var container = SetProviders("CAD", "JPY", "832652");
      var currencyPreference = new CurrencyPreference(container);
      Assert.AreEqual("CAD", currencyPreference.GetCurrencyPreference());
    }
  }
}
