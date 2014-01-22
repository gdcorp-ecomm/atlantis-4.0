using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Personalization.Interface;
using Atlantis.Framework.Providers.Personalization.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Engine;
using Atlantis.Framework.Testing.MockEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atlantis.Framework.Providers.AppSettings.Interface;

namespace Atlantis.Framework.Providers.Personalization.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  public class PersonalizationMessageTests
  {
    readonly MockProviderContainer _container = new MockProviderContainer();

    private void InitializeProvidersContexts()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.godaddy.com/");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);

      _container.RegisterProvider<ISiteContext, MockSiteContext>();
      _container.RegisterProvider<IManagerContext, MockNoManagerContext>();
      _container.RegisterProvider<IShopperContext, MockShopperContext>();
      _container.RegisterProvider<IPersonalizationProvider, PersonalizationProvider>();
      _container.RegisterProvider<IDebugContext, MockDebugContext>();
      _container.RegisterProvider<IAppSettingsProvider, MockAppSettingsProvider>();
      PersonalizationConfig.TMSAppId = "2";
      _container.SetData<bool>("MockSiteContextSettings.IsRequestInternal", true);

      IAppSettingsProvider settings = _container.Resolve<IAppSettingsProvider>();
      ((MockAppSettingsProvider)settings).ReturnValue = "true";
    }

    [TestMethod]
    public void GetsTargetMessages()
    {
      InitializeProvidersContexts();
      _container.Resolve<IShopperContext>().SetNewShopper("12345");
      IPersonalizationProvider prov = _container.Resolve<IPersonalizationProvider>();
      var targetMessage = prov.GetTargetedMessages("Homepage");
      //targetMessage = prov.GetTargetedMessages("Homepage");  verified by debugging that the second call is returned the stored data in session
      XmlSerializer serializer = new XmlSerializer(typeof(TargetedMessages));

      using (var stringWriter = new StringWriter())
      {
        serializer.Serialize(stringWriter, targetMessage);
        Debug.Write(stringWriter.ToString());
      }

      Assert.IsTrue(targetMessage.ResultCode == 0);
      Assert.IsTrue(targetMessage.Messages.Count == 5);
    }

    [TestMethod]
    public void LogsAnExceptionWhenAppIdIsEmpty()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        InitializeProvidersContexts();
        PersonalizationConfig.TMSAppId = string.Empty;
        _container.Resolve<IShopperContext>().SetNewShopper("12345");
        IPersonalizationProvider prov = _container.Resolve<IPersonalizationProvider>();
        var targetMessage = prov.GetTargetedMessages("Homepage");

        Assert.IsTrue(mockLogger.Exceptions.Count == 1);
        Assert.IsTrue(mockLogger.Exceptions[0].ErrorDescription.Contains("PersonalizationConfig.TMSAppId"));
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
        PersonalizationConfig.TMSAppId = "2";
      }

    }

    [TestMethod]
    public void GetsDataFromSessionWhenParametersAreSame()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        InitializeProvidersContexts();
        _container.Resolve<IShopperContext>().SetNewShopper("12345");
        IPersonalizationProvider prov = _container.Resolve<IPersonalizationProvider>();
        IDebugContext debugContext = _container.Resolve<IDebugContext>();

        string interactionPoint = "First";
        var targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        var list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[0].Key == "TMS Service URL");
        Assert.IsTrue(list[0].Value == interactionPoint);

        targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[1].Key == "TMS Service URL (from Session)");
        Assert.IsTrue(list[1].Value == interactionPoint);

        targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[2].Key == "TMS Service URL (from Session)");
        Assert.IsTrue(list[2].Value == interactionPoint);
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void DoesNotGetDataFromSessionWhenShopperIsDifferent()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        InitializeProvidersContexts();
        _container.Resolve<IShopperContext>().SetNewShopper("12345");
        IPersonalizationProvider prov = _container.Resolve<IPersonalizationProvider>();
        IDebugContext debugContext = _container.Resolve<IDebugContext>();

        string interactionPoint = "First";
        var targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        var list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[0].Key == "TMS Service URL");
        Assert.IsTrue(list[0].Value == interactionPoint);

        _container.Resolve<IShopperContext>().SetNewShopper("123456");
        targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[1].Key == "TMS Service URL");
        Assert.IsTrue(list[1].Value == interactionPoint);
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void DoesNotGetDataFromSessionWhenPrivateLabelIdIsDifferent()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        InitializeProvidersContexts();
        _container.Resolve<IShopperContext>().SetNewShopper("12345");
        _container.SetData<int>("MockSiteContextSettings.PrivateLabelId", 99);
        IPersonalizationProvider prov = _container.Resolve<IPersonalizationProvider>();
        IDebugContext debugContext = _container.Resolve<IDebugContext>();

        string interactionPoint = "First";
        var targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        var list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[0].Key == "TMS Service URL");
        Assert.IsTrue(list[0].Value == interactionPoint);

        _container.SetData<int>("MockSiteContextSettings.PrivateLabelId", 100);
        targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[1].Key == "TMS Service URL");
        Assert.IsTrue(list[1].Value == interactionPoint);
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void DoesNotGetDataFromSessionWhenInteractionPointIsDifferent()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        InitializeProvidersContexts();
        _container.Resolve<IShopperContext>().SetNewShopper("12345");
        IPersonalizationProvider prov = _container.Resolve<IPersonalizationProvider>();
        IDebugContext debugContext = _container.Resolve<IDebugContext>();

        string interactionPoint = "First";
        var targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        var list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[0].Key == "TMS Service URL");
        Assert.IsTrue(list[0].Value == interactionPoint);

        interactionPoint = "Second";
        targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[1].Key == "TMS Service URL");
        Assert.IsTrue(list[1].Value == interactionPoint);
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void StoresConsumedMessageInContainer()
    {
      const string CONTAINER_KEY = "A.F.PersonalizationProvider.ConsumedMessages";
      const string DATA_TOKEN_MESSAGE_Id = "TMSMessageId";
      const string DATA_TOKEN_MESSAGE_NAME = "TMSMessageName";
      const string DATA_TOKEN_MESSAGE_TAG = "TMSMessageTag";
      const string DATA_TOKEN_MESSAGE_TRACKING_ID = "TMSMessageTrackingId";

      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        InitializeProvidersContexts();
        IPersonalizationProvider prov = _container.Resolve<IPersonalizationProvider>();
        var msg = new MockConsumedMessage("1", "msg", "tag", "1");
        prov.AddToConsumedMessages(msg);
        var list = _container.GetData<IEnumerable<IConsumedMessage>>(CONTAINER_KEY, null).ToList<IConsumedMessage>();
        Assert.IsTrue(list.Count() == 1);
        Assert.IsTrue(list[0].TrackingData == "1msgtag1");

        var id = _container.GetData<string>(DATA_TOKEN_MESSAGE_Id, null);
        Assert.IsTrue(id == "1");

        var msgName = _container.GetData<string>(DATA_TOKEN_MESSAGE_NAME, null);
        Assert.IsTrue(msgName == "msg");

        var tag = _container.GetData<string>(DATA_TOKEN_MESSAGE_TAG, null);
        Assert.IsTrue(tag == "tag");

        var trackingId = _container.GetData<string>(DATA_TOKEN_MESSAGE_TRACKING_ID, null);
        Assert.IsTrue(trackingId == "1");


        msg = new MockConsumedMessage("2", "msg2", "tag2", "2");
        prov.AddToConsumedMessages(msg);
        list = _container.GetData<IEnumerable<IConsumedMessage>>(CONTAINER_KEY, null).ToList<IConsumedMessage>();
        Assert.IsTrue(list.Count() == 2);
        Assert.IsTrue(list[1].TrackingData == "2msg2tag22");

        id = _container.GetData<string>(DATA_TOKEN_MESSAGE_Id, null);
        Assert.IsTrue(id == "2");

        msgName = _container.GetData<string>(DATA_TOKEN_MESSAGE_NAME, null);
        Assert.IsTrue(msgName == "msg2");

        tag = _container.GetData<string>(DATA_TOKEN_MESSAGE_TAG, null);
        Assert.IsTrue(tag == "tag2");

        trackingId = _container.GetData<string>(DATA_TOKEN_MESSAGE_TRACKING_ID, null);
        Assert.IsTrue(trackingId == "2");
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void GetsTrackingData()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        InitializeProvidersContexts();
        IPersonalizationProvider prov = _container.Resolve<IPersonalizationProvider>();
        var msg = new MockConsumedMessage("1", "msg", "tag", "1");
        prov.AddToConsumedMessages(msg);

        msg = new MockConsumedMessage("2", "msg2", "tag2", "2");
        prov.AddToConsumedMessages(msg);

        Assert.IsTrue(prov.TrackingData == "1msgtag1^2msg2tag22");
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void GetsVisitorGuidFromCookie()
    {
      InitializeProvidersContexts();
      ISiteContext siteContext = _container.Resolve<ISiteContext>();
      HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("visitor");
      string guid = Guid.NewGuid().ToString();
      preferencesCookie["vid"] = guid;
      HttpContext.Current.Request.Cookies.Add(preferencesCookie);

      PersonalizationProvider prov = _container.Resolve<IPersonalizationProvider>() as PersonalizationProvider;
      Assert.AreEqual(guid, prov.VisitorGuid);

      //returns a string.empty when the cookie is not there
      HttpContext.Current.Request.Cookies.Remove("visitor");
      Assert.AreEqual(string.Empty, prov.VisitorGuid);
    }

    [TestMethod]
    public void HandlesInvalidVisitorCookie()
    {
      InitializeProvidersContexts();
      ISiteContext siteContext = _container.Resolve<ISiteContext>();
      HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("visitor");
      string guid = Guid.NewGuid().ToString();
      preferencesCookie.Value = "blah blah blah";
      HttpContext.Current.Request.Cookies.Add(preferencesCookie);

      PersonalizationProvider prov = _container.Resolve<IPersonalizationProvider>() as PersonalizationProvider;
      Assert.AreEqual(string.Empty, prov.VisitorGuid);
    }

    [TestMethod]
    public void AnonymousShopper_GetsDataFromSessionWhenParametersAreSame()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        InitializeProvidersContexts();
        ISiteContext siteContext = _container.Resolve<ISiteContext>();
        HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("visitor");
        string guid = Guid.NewGuid().ToString();
        preferencesCookie["vid"] = guid;
        HttpContext.Current.Request.Cookies.Add(preferencesCookie);

        _container.Resolve<IShopperContext>().SetNewShopper(string.Empty);
        IPersonalizationProvider prov = _container.Resolve<IPersonalizationProvider>();
        IDebugContext debugContext = _container.Resolve<IDebugContext>();

        string interactionPoint = "First";
        var targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        var list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[0].Key == "TMS Service URL");
        Assert.IsTrue(list[0].Value == interactionPoint);

        targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[1].Key == "TMS Service URL (from Session)");
        Assert.IsTrue(list[1].Value == interactionPoint);

        targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[2].Key == "TMS Service URL (from Session)");
        Assert.IsTrue(list[2].Value == interactionPoint);
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void AnonymousShopper_DoesNotGetDataFromSessionWhenVisitorIsDifferent()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        InitializeProvidersContexts();
        ISiteContext siteContext = _container.Resolve<ISiteContext>();
        HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("visitor");
        string guid = Guid.NewGuid().ToString();
        preferencesCookie["vid"] = guid;
        HttpContext.Current.Request.Cookies.Add(preferencesCookie);

        _container.Resolve<IShopperContext>().SetNewShopper(string.Empty);
        IPersonalizationProvider prov = _container.Resolve<IPersonalizationProvider>();
        IDebugContext debugContext = _container.Resolve<IDebugContext>();

        string interactionPoint = "First";
        var targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        var list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[0].Key == "TMS Service URL");
        Assert.IsTrue(list[0].Value == interactionPoint);

        HttpContext.Current.Request.Cookies["visitor"]["vid"] = Guid.NewGuid().ToString();
        targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[1].Key == "TMS Service URL");
        Assert.IsTrue(list[1].Value == interactionPoint);
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void AnonymousShopper_DoesNotGetDataFromSessionWhenPrivateLabelIdIsDifferent()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        InitializeProvidersContexts();
        ISiteContext siteContext = _container.Resolve<ISiteContext>();
        HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("visitor");
        string guid = Guid.NewGuid().ToString();
        preferencesCookie["vid"] = guid;
        HttpContext.Current.Request.Cookies.Add(preferencesCookie);

        _container.Resolve<IShopperContext>().SetNewShopper(string.Empty);
        _container.SetData<int>("MockSiteContextSettings.PrivateLabelId", 99);
        IPersonalizationProvider prov = _container.Resolve<IPersonalizationProvider>();
        IDebugContext debugContext = _container.Resolve<IDebugContext>();

        string interactionPoint = "First";
        var targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        var list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[0].Key == "TMS Service URL");
        Assert.IsTrue(list[0].Value == interactionPoint);

        _container.SetData<int>("MockSiteContextSettings.PrivateLabelId", 100);
        targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[1].Key == "TMS Service URL");
        Assert.IsTrue(list[1].Value == interactionPoint);
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void AnonymousShopper_DoesNotGetDataFromSessionWhenInteractionPointIsDifferent()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        InitializeProvidersContexts();

        ISiteContext siteContext = _container.Resolve<ISiteContext>();
        HttpCookie preferencesCookie = siteContext.NewCrossDomainMemCookie("visitor");
        string guid = Guid.NewGuid().ToString();
        preferencesCookie["vid"] = guid;
        HttpContext.Current.Request.Cookies.Add(preferencesCookie);

        _container.Resolve<IShopperContext>().SetNewShopper(string.Empty);
        IPersonalizationProvider prov = _container.Resolve<IPersonalizationProvider>();
        IDebugContext debugContext = _container.Resolve<IDebugContext>();

        string interactionPoint = "First";
        var targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        var list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[0].Key == "TMS Service URL");
        Assert.IsTrue(list[0].Value == interactionPoint);

        interactionPoint = "Second";
        targetMessage1 = prov.GetTargetedMessages(interactionPoint);
        list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list[1].Key == "TMS Service URL");
        Assert.IsTrue(list[1].Value == interactionPoint);
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
      }
    }

    [TestMethod]
    public void DoesNotCallTripletWhenTurnedOff()
    {
      IErrorLogger oldLogger = EngineLogging.EngineLogger;
      var mockLogger = new MockErrorLogger();
      EngineLogging.EngineLogger = mockLogger;

      try
      {
        InitializeProvidersContexts();

        IAppSettingsProvider settings = _container.Resolve<IAppSettingsProvider>();
        ((MockAppSettingsProvider)settings).ReturnValue = "false";

        IPersonalizationProvider prov = _container.Resolve<IPersonalizationProvider>();
        var targetMessage = prov.GetTargetedMessages("Homepage");
        Assert.IsTrue(mockLogger.Exceptions.Count == 0);

        
        IDebugContext debugContext = _container.Resolve<IDebugContext>();
        var list = debugContext.GetDebugTrackingData();
        Assert.IsTrue(list.Count == 0);
      }
      finally
      {
        EngineLogging.EngineLogger = oldLogger;
        IAppSettingsProvider settings = _container.Resolve<IAppSettingsProvider>();
        ((MockAppSettingsProvider)settings).ReturnValue = "true";
      }
    }
  }
}
