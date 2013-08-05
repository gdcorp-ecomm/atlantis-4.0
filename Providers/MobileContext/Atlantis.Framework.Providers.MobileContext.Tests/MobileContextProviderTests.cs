using Atlantis.Framework.Providers.MobileContext.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Providers.MobileContext.Tests
{
  [TestClass]
  public class MobileContextProviderTests
  {
    private const string IPHONE_USER_AGENT = "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_2 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8H7 Safari/6533.18.5";
    private const string CHROME_USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1500.95 Safari/537.36";
    private const string FIREFOX_USER_AGENT = "Mozilla/5.0 (Windows; U; Windows NT 6.1; ru; rv:1.9.0.18) Gecko/2010020220 Firefox/3.0.18";
    private const string IE_USER_AGENT = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
    private const string OPERA_USER_AGENT = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; en) Opera 9.23";
    private const string ANDROID_USER_AGENT = "Mozilla/5.0 (Linux; U; Android 2.1-update1; en-us; Nexus One Build/ERE27) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17";
    private const string IPAD_USER_AGENT = "Mozilla/5.0 (iPad; U; CPU OS 3_2 like Mac OS X; en-us) AppleWebKit/531.21.10 (KHTML, like Gecko) Version/4.0.4 Mobile/7B334b Safari/531.21.10";
    private const string WINDOWS_OPERA_USER_AGENT = "Opera/9.80 (Windows Mobile; WCE; Opera Mobi/WMD-50433; U; en) Presto/2.4.13 Version/10.00";


    readonly MockProviderContainer _container = new MockProviderContainer();

    private IMobileContextProvider NewMobileContextProvider()
    {
      _container.RegisterProvider<IMobileContextProvider, MobileContextProvider>();

      return _container.Resolve<IMobileContextProvider>();
    }

    [TestMethod]
    public void ApplicationTypeNone()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      IMobileContextProvider provider = NewMobileContextProvider();
      MobileApplicationType mobileApplicationType = provider.MobileApplicationType;
      Assert.AreEqual(true, mobileApplicationType == MobileApplicationType.None);
    }

    [TestMethod]
    public void ApplicationTypeiPhone()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/?mappid=1");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      IMobileContextProvider provider = NewMobileContextProvider();
      MobileApplicationType mobileApplicationType = provider.MobileApplicationType;
      Assert.AreEqual(true, mobileApplicationType == MobileApplicationType.iPhone );
    }

    [TestMethod]
    public void ApplicationTypeiPad()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/?mappid=4");
      mockHttpRequest.MockUserAgent(IPAD_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      IMobileContextProvider provider = NewMobileContextProvider();
      MobileApplicationType mobileApplicationType = provider.MobileApplicationType;
      Assert.AreEqual(true, mobileApplicationType == MobileApplicationType.iPad);
    }

    [TestMethod]
    public void ApplicationTypeAndroid()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/?mappid=3");
      mockHttpRequest.MockUserAgent(ANDROID_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      IMobileContextProvider provider = NewMobileContextProvider();
      MobileApplicationType mobileApplicationType = provider.MobileApplicationType;
      Assert.AreEqual(true, mobileApplicationType == MobileApplicationType.Android);
    }

    [TestMethod]
    public void DeviceTypeiPhone()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      IMobileContextProvider provider = NewMobileContextProvider();
      MobileDeviceType mobileDeviceType = provider.MobileDeviceType;
      Assert.AreEqual(true, mobileDeviceType == MobileDeviceType.iPhone);
    }

    [TestMethod]
    public void DeviceTypeiPad()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(IPAD_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      IMobileContextProvider provider = NewMobileContextProvider();
      MobileDeviceType mobileDeviceType = provider.MobileDeviceType;
      Assert.AreEqual(true, mobileDeviceType == MobileDeviceType.iPad);
    }

    [TestMethod]
    public void DeviceTypeAndroid()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(ANDROID_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      IMobileContextProvider provider = NewMobileContextProvider();
      MobileDeviceType mobileDeviceType = provider.MobileDeviceType;
      Assert.AreEqual(true, mobileDeviceType == MobileDeviceType.Android);
    }

    [TestMethod]
    public void DeviceTypeOldDeviceUnknown()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(WINDOWS_OPERA_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      IMobileContextProvider provider = NewMobileContextProvider();
      MobileDeviceType mobileDeviceType = provider.MobileDeviceType;
      Assert.AreEqual(true, mobileDeviceType == MobileDeviceType.Unknown);
    }

    [TestMethod]
    public void DeviceTypeNoUserAgentUnknown()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent("");
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      IMobileContextProvider provider = NewMobileContextProvider();
      MobileDeviceType mobileDeviceType = provider.MobileDeviceType;
      Assert.AreEqual(true, mobileDeviceType == MobileDeviceType.Unknown);
    }

    [TestMethod]
    public void ViewTypeWebkit()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(CHROME_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      IMobileContextProvider provider = NewMobileContextProvider();
      MobileViewType mobileViewType = provider.MobileViewType;
      Assert.AreEqual(true, mobileViewType == MobileViewType.Webkit);
    }

    [TestMethod]
    public void ViewTypeDefault()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(IE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      IMobileContextProvider provider = NewMobileContextProvider();
      MobileViewType mobileViewType = provider.MobileViewType;
      Assert.AreEqual(true, mobileViewType == MobileViewType.Default);
    }

    [TestMethod]
    public void ViewTypeFirefox()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(FIREFOX_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      IMobileContextProvider provider = NewMobileContextProvider();
      MobileViewType mobileViewType = provider.MobileViewType;
      Assert.AreEqual(true, mobileViewType == MobileViewType.FireFox);
    }

    [TestMethod]
    public void ViewTypeOpera()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/");
      mockHttpRequest.MockUserAgent(OPERA_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      IMobileContextProvider provider = NewMobileContextProvider();
      MobileViewType mobileViewType = provider.MobileViewType;
      Assert.AreEqual(true, mobileViewType == MobileViewType.Opera);
    }

    [TestMethod]
    public void ViewTypeAppleiPhone()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/?mappid=1");
      mockHttpRequest.MockUserAgent(IPHONE_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      IMobileContextProvider provider = NewMobileContextProvider();
      MobileViewType mobileViewType = provider.MobileViewType;
      Assert.AreEqual(true, mobileViewType == MobileViewType.Apple);
    }

    [TestMethod]
    public void ViewTypeAppleiPad()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/?mappid=4");
      mockHttpRequest.MockUserAgent(IPAD_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      IMobileContextProvider provider = NewMobileContextProvider();
      MobileViewType mobileViewType = provider.MobileViewType;
      Assert.AreEqual(true, mobileViewType == MobileViewType.Webkit);
    }

    [TestMethod]
    public void ViewTypeAndroid()
    {
      MockHttpRequest mockHttpRequest = new MockHttpRequest("http://www.debug.godaddy-com.ide/?mappid=3");
      mockHttpRequest.MockUserAgent(ANDROID_USER_AGENT);
      MockHttpContext.SetFromWorkerRequest(mockHttpRequest);
      IMobileContextProvider provider = NewMobileContextProvider();
      MobileViewType mobileViewType = provider.MobileViewType;
      Assert.AreEqual(true, mobileViewType == MobileViewType.Android);
    }
  }
}