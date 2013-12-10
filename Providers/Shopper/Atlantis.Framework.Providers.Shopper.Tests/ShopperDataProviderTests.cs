using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Testing.MockProviders;
using Atlantis.Framework.Providers.Shopper.Interface;
using System.Reflection;
using System.Collections.Generic;
using Atlantis.Framework.Testing.MockEngine;
using Atlantis.Framework.Testing.MockHttpContext;
using System.Net;

namespace Atlantis.Framework.Providers.Shopper.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Shopper.Impl.dll")]
  public class ShopperDataProviderTests
  {

    private IProviderContainer SetContainer(string shopperId = null, int privateLabelId = 1)
    {
      MockProviderContainer container = new MockProviderContainer();
      container.RegisterProvider<ISiteContext, MockSiteContext>();
      container.RegisterProvider<IShopperContext, MockShopperContext>();
      container.RegisterProvider<IShopperDataProvider, ShopperDataProvider>();
      container.SetData(MockSiteContextSettings.PrivateLabelId, privateLabelId);

      if (!string.IsNullOrEmpty(shopperId))
      {
        var shopper = container.Resolve<IShopperContext>();
        shopper.SetNewShopper(shopperId);
      }

      return container;
    }

    [TestMethod]
    public void TryGetFieldEmptyShopper()
    {
      IProviderContainer container = SetContainer();
      var dataProvider = container.Resolve<IShopperDataProvider>();
      dataProvider.RegisterNeededFields("first_name", "last_name");

      string firstName;
      bool success = dataProvider.TryGetField("first_name", out firstName);
      Assert.IsTrue(success);
      Assert.AreEqual(string.Empty, firstName);
    }

    [TestMethod]
    public void TryGetFieldRealShopperOneRequestOnly()
    {
      ConfigElement getShopperConfig;
      Engine.Engine.TryGetConfigElement(ShopperProviderEngineRequests.GetShopper, out getShopperConfig);
      getShopperConfig.ResetStats();

      IProviderContainer container = SetContainer("832652");
      var dataProvider = container.Resolve<IShopperDataProvider>();
      dataProvider.RegisterNeededFields("first_name", "last_name");

      string firstName;
      bool success = dataProvider.TryGetField("first_name", out firstName);
      Assert.IsTrue(success);
      Assert.AreNotEqual(string.Empty, firstName);

      string lastName;
      success = dataProvider.TryGetField("last_name", out lastName);
      Assert.IsTrue(success);
      Assert.AreNotEqual(string.Empty, lastName);

      Assert.AreEqual(1, getShopperConfig.Stats.Succeeded);
      Assert.AreEqual(0, getShopperConfig.Stats.Failed);
    }

    [TestMethod]
    public void TryGetFieldRealShopperAdditionalRequestRequired()
    {
      ConfigElement getShopperConfig;
      Engine.Engine.TryGetConfigElement(ShopperProviderEngineRequests.GetShopper, out getShopperConfig);
      getShopperConfig.ResetStats();

      IProviderContainer container = SetContainer("832652");
      var dataProvider = container.Resolve<IShopperDataProvider>();
      dataProvider.RegisterNeededFields("first_name", "last_name");

      string firstName;
      bool success = dataProvider.TryGetField("first_name", out firstName);
      Assert.IsTrue(success);
      Assert.AreNotEqual(string.Empty, firstName);

      string city;
      success = dataProvider.TryGetField("city", out city);
      Assert.IsTrue(success);
      Assert.AreNotEqual(string.Empty, city);

      Assert.AreEqual(2, getShopperConfig.Stats.Succeeded);
      Assert.AreEqual(0, getShopperConfig.Stats.Failed);
    }

    [TestMethod]
    public void TryGetFieldRealShopperAdditionalRequestRequiredSandwich()
    {
      ConfigElement getShopperConfig;
      Engine.Engine.TryGetConfigElement(ShopperProviderEngineRequests.GetShopper, out getShopperConfig);
      getShopperConfig.ResetStats();

      IProviderContainer container = SetContainer("832652");
      var dataProvider = container.Resolve<IShopperDataProvider>();

      HashSet<string> fields = new HashSet<string>(new[] { "first_name", "last_name" });
      dataProvider.RegisterNeededFields(fields);

      string firstName;
      bool success = dataProvider.TryGetField("first_name", out firstName);
      Assert.IsTrue(success);
      Assert.AreNotEqual(string.Empty, firstName);

      string city;
      success = dataProvider.TryGetField("city", out city);
      Assert.IsTrue(success);
      Assert.AreNotEqual(string.Empty, city);

      string lastName;
      success = dataProvider.TryGetField("last_name", out lastName);
      Assert.IsTrue(success);
      Assert.AreNotEqual(string.Empty, lastName);

      Assert.AreEqual(2, getShopperConfig.Stats.Succeeded);
      Assert.AreEqual(0, getShopperConfig.Stats.Failed);
    }

    [TestMethod]
    public void TryGetFieldEmptyFieldName()
    {
      IProviderContainer container = SetContainer();
      var dataProvider = container.Resolve<IShopperDataProvider>();

      string firstName;
      bool success = dataProvider.TryGetField(string.Empty, out firstName);
      Assert.IsFalse(success);
    }

    [TestMethod]
    public void TryGetFieldNullFieldName()
    {
      IProviderContainer container = SetContainer();
      var dataProvider = container.Resolve<IShopperDataProvider>();

      string firstName;
      bool success = dataProvider.TryGetField(null, out firstName);
      Assert.IsFalse(success);
    }

    [TestMethod]
    public void TryGetFieldInvalidTypeConversion()
    {
      IProviderContainer container = SetContainer("832652");
      var dataProvider = container.Resolve<IShopperDataProvider>();
      dataProvider.RegisterNeededFields("first_name", "last_name");

      int firstName;
      bool success = dataProvider.TryGetField("first_name", out firstName);
      Assert.IsFalse(success);
    }

    [TestMethod]
    public void TryGetFieldException()
    {
      IErrorLogger oldLogger = Engine.EngineLogging.EngineLogger;
      int getShopperRequestType = ShopperProviderEngineRequests.GetShopper;

      try
      {
        MockErrorLogger mockLogger = new MockErrorLogger();
        Engine.EngineLogging.EngineLogger = mockLogger;
        ShopperProviderEngineRequests.GetShopper = ExceptionRequest.RequestType;

        IProviderContainer container = SetContainer("832652");
        var dataProvider = container.Resolve<IShopperDataProvider>();
        dataProvider.RegisterNeededFields("first_name", "last_name");

        int firstName;
        bool success = dataProvider.TryGetField("first_name", out firstName);
        Assert.IsFalse(success);

        Assert.AreNotEqual(0, mockLogger.Exceptions.Count);
      }
      finally
      {
        Engine.EngineLogging.EngineLogger = oldLogger;
        ShopperProviderEngineRequests.GetShopper = getShopperRequestType;
      }
    }

    [TestMethod]
    public void TryGetFieldShopperDoesNotExist()
    {
      IProviderContainer container = SetContainer("832652000");
      var dataProvider = container.Resolve<IShopperDataProvider>();
      dataProvider.RegisterNeededFields("first_name", "last_name");

      string firstName;
      bool success = dataProvider.TryGetField("first_name", out firstName);
      Assert.IsFalse(success);
    }

    [TestMethod]
    public void IsValidShopperEmpty()
    {
      IProviderContainer container = SetContainer();
      var dataProvider = container.Resolve<IShopperDataProvider>();
      Assert.IsFalse(dataProvider.IsShopperValid());
    }

    [TestMethod]
    public void IsValidShopperBasic()
    {
      IProviderContainer container = SetContainer(shopperId: "832652");
      var dataProvider = container.Resolve<IShopperDataProvider>();
      Assert.IsTrue(dataProvider.IsShopperValid());
    }

    [TestMethod]
    public void IsValidShopperBasicTwiceButOneCall()
    {
      ConfigElement verifyShopperConfig;
      Engine.Engine.TryGetConfigElement(ShopperProviderEngineRequests.VerifyShopper, out verifyShopperConfig);
      verifyShopperConfig.ResetStats();

      IProviderContainer container = SetContainer(shopperId: "832652");
      var dataProvider = container.Resolve<IShopperDataProvider>();
      Assert.IsTrue(dataProvider.IsShopperValid());
      Assert.IsTrue(dataProvider.IsShopperValid());

      Assert.AreEqual(1, verifyShopperConfig.Stats.Succeeded);
      Assert.AreEqual(0, verifyShopperConfig.Stats.Failed);
    }


    [TestMethod]
    public void IsValidShopperWrongPlId()
    {
      IProviderContainer container = SetContainer(shopperId: "832652", privateLabelId: 2);
      var dataProvider = container.Resolve<IShopperDataProvider>();
      Assert.IsFalse(dataProvider.IsShopperValid());
    }

    [TestMethod]
    public void IsValidShopperNotFound()
    {
      IProviderContainer container = SetContainer(shopperId: "832652000");
      var dataProvider = container.Resolve<IShopperDataProvider>();
      Assert.IsFalse(dataProvider.IsShopperValid());
    }

    [TestMethod]
    public void IsShopperValidExceptionDefaultsToTrue()
    {
      IErrorLogger oldLogger = Engine.EngineLogging.EngineLogger;
      int verifyShopperRequestType = ShopperProviderEngineRequests.VerifyShopper;

      try
      {
        MockErrorLogger mockLogger = new MockErrorLogger();
        Engine.EngineLogging.EngineLogger = mockLogger;
        ShopperProviderEngineRequests.VerifyShopper = ExceptionRequest.RequestType;

        IProviderContainer container = SetContainer(shopperId: "832652");
        var dataProvider = container.Resolve<IShopperDataProvider>();
        Assert.IsTrue(dataProvider.IsShopperValid());
        Assert.AreNotEqual(0, mockLogger.Exceptions.Count);
      }
      finally
      {
        Engine.EngineLogging.EngineLogger = oldLogger;
        ShopperProviderEngineRequests.VerifyShopper = verifyShopperRequestType;
      }
    }

    [TestMethod]
    public void CreateShopperBasic()
    {
      IProviderContainer container = SetContainer(shopperId: "832652");
      var dataProvider = container.Resolve<IShopperDataProvider>();
      var shopperContext = container.Resolve<IShopperContext>();

      Assert.AreEqual("832652", shopperContext.ShopperId);
      bool success = dataProvider.TryCreateNewShopper();
      Assert.IsTrue(success);
      Assert.AreNotEqual("832652", shopperContext.ShopperId);
    }

    [TestMethod]
    public void CreateShopperError()
    {
      IErrorLogger oldLogger = Engine.EngineLogging.EngineLogger;
      int createShopperRequestType = ShopperProviderEngineRequests.CreateShopper;

      try
      {
        MockErrorLogger mockLogger = new MockErrorLogger();
        Engine.EngineLogging.EngineLogger = mockLogger;
        ShopperProviderEngineRequests.CreateShopper = ExceptionRequest.RequestType;

        IProviderContainer container = SetContainer(shopperId: "832652");
        var dataProvider = container.Resolve<IShopperDataProvider>();
        var shopperContext = container.Resolve<IShopperContext>();

        Assert.AreEqual("832652", shopperContext.ShopperId);
        bool success = dataProvider.TryCreateNewShopper();
        Assert.IsFalse(success);
        Assert.AreEqual("832652", shopperContext.ShopperId);
        Assert.AreNotEqual(0, mockLogger.Exceptions.Count);
      }
      finally
      {
        Engine.EngineLogging.EngineLogger = oldLogger;
        ShopperProviderEngineRequests.CreateShopper = createShopperRequestType;
      }
    }

    [TestMethod]
    public void UpdateShopperBasic()
    {
      IProviderContainer container = SetContainer(shopperId: "832652");
      var dataProvider = container.Resolve<IShopperDataProvider>();
      var shopperContext = container.Resolve<IShopperContext>();

      Dictionary<string, string> updateFields = new Dictionary<string, string>();
      updateFields["city"] = "Springfield";

      ShopperUpdateResultType result = dataProvider.UpdateShopperInfo(updateFields);
      Assert.IsTrue(result == ShopperUpdateResultType.Success);
    }

    [TestMethod]
    public void UpdateShopperNoShopper()
    {
      ConfigElement updateShopperConfig;
      Engine.Engine.TryGetConfigElement(ShopperProviderEngineRequests.UpdateShopper, out updateShopperConfig);
      updateShopperConfig.ResetStats();

      IProviderContainer container = SetContainer(shopperId: string.Empty);
      var dataProvider = container.Resolve<IShopperDataProvider>();
      var shopperContext = container.Resolve<IShopperContext>();

      Dictionary<string, string> updateFields = new Dictionary<string, string>();
      updateFields["city"] = "Springfield";

      bool success = dataProvider.TryUpdateShopper(updateFields);
      Assert.IsFalse(success);
      Assert.AreEqual(0, updateShopperConfig.Stats.Succeeded + updateShopperConfig.Stats.Failed);
    }

    [TestMethod]
    public void UpdateShopperError()
    {
      IErrorLogger oldLogger = Engine.EngineLogging.EngineLogger;
      int updateShopperRequestType = ShopperProviderEngineRequests.UpdateShopper;

      try
      {
        MockErrorLogger mockLogger = new MockErrorLogger();
        Engine.EngineLogging.EngineLogger = mockLogger;
        ShopperProviderEngineRequests.UpdateShopper = ExceptionRequest.RequestType;

        IProviderContainer container = SetContainer(shopperId: "832652");
        var dataProvider = container.Resolve<IShopperDataProvider>();
        var shopperContext = container.Resolve<IShopperContext>();

        Dictionary<string, string> updateFields = new Dictionary<string, string>();
        updateFields["city"] = "Springfield";

        bool success = dataProvider.TryUpdateShopper(updateFields);
        Assert.IsFalse(success);
        Assert.AreNotEqual(0, mockLogger.Exceptions.Count);
      }
      finally
      {
        Engine.EngineLogging.EngineLogger = oldLogger;
        ShopperProviderEngineRequests.UpdateShopper = updateShopperRequestType;
      }

    }

    [TestMethod]
    public void UpdateShopperResetsGetShopperCache()
    {
      ConfigElement getShopperConfigItem;
      Engine.Engine.TryGetConfigElement(ShopperProviderEngineRequests.GetShopper, out getShopperConfigItem);
      getShopperConfigItem.ResetStats();

      IProviderContainer container = SetContainer(shopperId: "832652");
      var dataProvider = container.Resolve<IShopperDataProvider>();
      dataProvider.RegisterNeededFields("city");

      string originalCity;
      Assert.IsTrue(dataProvider.TryGetField("city", out originalCity));
      Assert.AreEqual(1, getShopperConfigItem.Stats.Succeeded);

      Dictionary<string, string> updateFields = new Dictionary<string, string>();
      string newCityUpdate;
      if (originalCity == "Springfield")
      {
        newCityUpdate = "Fallfield";
      }
      else
      {
        newCityUpdate = "Springfield";
      }
      updateFields["city"] = newCityUpdate;

      Assert.IsTrue(dataProvider.TryUpdateShopper(updateFields));
      string newCityGotten;
      Assert.IsTrue(dataProvider.TryGetField("city", out newCityGotten));
      Assert.AreEqual(2, getShopperConfigItem.Stats.Succeeded);
    }

    [TestMethod]
    public void GetShopperWithProxyActive()
    {
      IProviderContainer container = SetContainer("832652");
      container.RegisterProvider<IProxyContext, MockProxy>();
      var dataProvider = container.Resolve<IShopperDataProvider>();
      dataProvider.RegisterNeededFields("city");

      string city;
      Assert.IsTrue(dataProvider.TryGetField("city", out city));
    }

    [TestMethod]
    public void GetShopperWithHttpContext()
    {
      var request = new MockHttpRequest("http://www.godaddy.com/");
      request.MockRemoteAddress(new IPAddress(67306499)); //4.3.4.3
      MockHttpContext.SetFromWorkerRequest(request);

      IProviderContainer container = SetContainer("832652");
      var dataProvider = container.Resolve<IShopperDataProvider>();
      dataProvider.RegisterNeededFields("city");

      string city;
      Assert.IsTrue(dataProvider.TryGetField("city", out city));
    }

  }
}
