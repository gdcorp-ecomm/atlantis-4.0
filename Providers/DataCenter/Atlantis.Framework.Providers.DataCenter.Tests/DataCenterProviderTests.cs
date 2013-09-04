using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DataCenter.Interface;
using Atlantis.Framework.Providers.Geo.Interface;
using Atlantis.Framework.Providers.Interface.Preferences;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Testing.MockEngine;
using Atlantis.Framework.Testing.MockPreferencesProvider;

namespace Atlantis.Framework.Providers.DataCenter.Tests
{
  [TestClass]
  public class DataCenterProviderTests
  {
    private MockErrorLogger _testLogger = new MockErrorLogger();
    private IErrorLogger _defaultLogger = Engine.EngineLogging.EngineLogger;

    [TestInitialize]
    public void TestInit()
    {
      Engine.EngineLogging.EngineLogger = _testLogger;
    }

    [TestCleanup]
    public void TestCleanup()
    {
      Engine.EngineLogging.EngineLogger = _defaultLogger;
    }

    private const string _DATACENTERPREFKEY = "dataCenterCode";

    private MockProviderContainer NewDataCenterProvider(string ipCountry = null, bool usePreferences = true, string adcQuery = null)
    {
      var mock = new MockProviderContainer();
      mock.RegisterProvider<IDataCenterProvider, DataCenterProvider>();

      if (ipCountry != null)
      {
        mock.RegisterProvider<IGeoProvider, MockGeoProvider>();
        mock.SetData("MockGeoProvider.RequestCountryCode", ipCountry);
      }

      if (usePreferences)
      {
        mock.RegisterProvider<IShopperPreferencesProvider, MockShopperPreference>();
      }

      if (adcQuery != null)
      {
        var url = "http://testurl.com/";
        if (adcQuery != "none")
        {
          url = url + "?adc=" + adcQuery;
        }
        var request = new MockHttpRequest(url);
        MockHttpContext.SetFromWorkerRequest(request);
      }
      else
      {
        HttpContext.Current = null;
      }

      return mock;
    }

    [TestMethod]
    public void DataCenterProviderAllDependenciesMissing()
    {
      var container = NewDataCenterProvider(null, false);
      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("US", dataCenter.DataCenterCode, true);
    }

    [TestMethod]
    public void DataCenterProviderPreferencesOnlyUs()
    {
      var container = NewDataCenterProvider(null, true);

      var preferences = container.Resolve<IShopperPreferencesProvider>();
      preferences.UpdatePreference(_DATACENTERPREFKEY, "uS");

      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("US", dataCenter.DataCenterCode, true);
    }

    [TestMethod]
    public void DataCenterProviderPreferencesOnlyNoPreference()
    {
      var container = NewDataCenterProvider(null, true);
      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("US", dataCenter.DataCenterCode, true);
    }

    [TestMethod]
    public void DataCenterProviderPreferencesOnlyBadData()
    {
      var container = NewDataCenterProvider(null, true);

      var preferences = container.Resolve<IShopperPreferencesProvider>();
      preferences.UpdatePreference(_DATACENTERPREFKEY, "sG");

      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("US", dataCenter.DataCenterCode, true);
    }

    [TestMethod]
    public void DataCenterProviderPreferencesOnlyEu()
    {
      var container = NewDataCenterProvider(null, true);

      var preferences = container.Resolve<IShopperPreferencesProvider>();
      preferences.UpdatePreference(_DATACENTERPREFKEY, "eU");

      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("eu", dataCenter.DataCenterCode, true);
    }

    [TestMethod]
    public void DataCenterProviderPreferencesOnlyAp()
    {
      var container = NewDataCenterProvider(null, true);

      var preferences = container.Resolve<IShopperPreferencesProvider>();
      preferences.UpdatePreference(_DATACENTERPREFKEY, "Ap");

      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("ap", dataCenter.DataCenterCode, true);
    }

    [TestMethod]
    public void DataCenterProviderGeoOnlyUs()
    {
      var container = NewDataCenterProvider("uS", false);
      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("US", dataCenter.DataCenterCode, true);
    }

    [TestMethod]
    public void DataCenterProviderGeoOnlyEu()
    {
      var container = NewDataCenterProvider("fr", false);
      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("eu", dataCenter.DataCenterCode, true);
    }

    [TestMethod]
    public void DataCenterProviderGeoOnlyAp()
    {
      var container = NewDataCenterProvider("sg", false);
      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("ap", dataCenter.DataCenterCode, true);
    }

    [TestMethod]
    public void DataCenterProviderGeoOnlySg()
    {
      var container = NewDataCenterProvider("co", false);
      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("us", dataCenter.DataCenterCode, true);
    }

    [TestMethod]
    public void DataCenterProviderGeoOnlyEmpty()
    {
      var container = NewDataCenterProvider(string.Empty, false);
      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("us", dataCenter.DataCenterCode, true);
    }

    [TestMethod]
    public void DataCenterProviderGeoOnlyError()
    {
      _testLogger.Exceptions.Clear();
      var container = NewDataCenterProvider("error", false);
      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("us", dataCenter.DataCenterCode, true);
      Assert.AreNotEqual(0, _testLogger.Exceptions.Count);
    }

    [TestMethod]
    public void DataCenterProviderGeoNoPrefEu()
    {
      var container = NewDataCenterProvider("fr");
      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("eu", dataCenter.DataCenterCode, true);
    }

    [TestMethod]
    public void DataCenterProviderGeoApPrefEuCountry()
    {
      var container = NewDataCenterProvider("fr");

      var preferences = container.Resolve<IShopperPreferencesProvider>();
      preferences.UpdatePreference(_DATACENTERPREFKEY, "Ap");

      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("ap", dataCenter.DataCenterCode, true);
    }

    [TestMethod]
    public void DataCenterProviderAdcQuery()
    {
      var container = NewDataCenterProvider(null, true, "eu");

      var preferences = container.Resolve<IShopperPreferencesProvider>();
      preferences.UpdatePreference(_DATACENTERPREFKEY, "Ap");

      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("eu", dataCenter.DataCenterCode, true);

      var newPref = preferences.GetPreference(_DATACENTERPREFKEY, string.Empty);
      Assert.AreEqual("eu", newPref, true);
    }

    [TestMethod]
    public void DataCenterProviderNoAdcQuery()
    {
      var container = NewDataCenterProvider(null, true, "none");

      var preferences = container.Resolve<IShopperPreferencesProvider>();
      preferences.UpdatePreference(_DATACENTERPREFKEY, "Ap");

      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("ap", dataCenter.DataCenterCode, true);
    }

    [TestMethod]
    public void DataCenterProviderAdcQueryEmpty()
    {
      var container = NewDataCenterProvider(null, true, "");

      var preferences = container.Resolve<IShopperPreferencesProvider>();
      preferences.UpdatePreference(_DATACENTERPREFKEY, "Ap");

      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("ap", dataCenter.DataCenterCode, true);
    }

    [TestMethod]
    public void DataCenterProviderAdcQueryBad()
    {
      var container = NewDataCenterProvider(null, true, "blue");

      var preferences = container.Resolve<IShopperPreferencesProvider>();
      preferences.UpdatePreference(_DATACENTERPREFKEY, "Ap");

      var dataCenter = container.Resolve<IDataCenterProvider>();
      Assert.AreEqual("ap", dataCenter.DataCenterCode, true);
    }


  }
}
