using System.Collections.Specialized;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace Atlantis.Framework.Providers.ProxyContext.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  [DeploymentItem("Atlantis.Framework.AppSettings.Impl.dll")]
  public class ProxyContextTests
  {
    [TestInitialize]
    public void InitProxies()
    {
      ActiveProxyTypes.Add(ProxyTypes.LocalARR);
      ActiveProxyTypes.Add(ProxyTypes.AkamaiDSA);
      ActiveProxyTypes.Add(ProxyTypes.CustomResellerARR);
      ActiveProxyTypes.Add(ProxyTypes.TransPerfectTranslation);
      ActiveProxyTypes.Add(ProxyTypes.SmartlingTranslation);
      ActiveProxyTypes.Add(ProxyTypes.CountrySiteARR);
    }

    [TestMethod]
    public void RequestNoProxies()
    {
      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      MockHttpContext.SetFromWorkerRequest(request);
      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();

      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();
      Assert.AreEqual(ProxyStatusType.None, context.Status);
      Assert.AreEqual(0, context.ActiveProxyChain.Count());
    }

    const string _NONPROXIEDURL = "http://mysite.com/default.aspx";
    const string _NONPROXIEDHOST = "mysite.com";
    const string _ARRORIGINHOST = "www.originalhost.com";
    const string _CUSTOMORIGINHOST = "customhost.com";
    const string _TXORIGINHOST = "klingon.originalhost.com";
    const string _TXLANGUAGE = "kl";
    const string _COUNTRYORIGINHOST = "jp.originalhost.com";
    const string _AKAMAISECRET = "DSADEBUG";
    const string _AKAMAIHOST = "yy.originalhost.com";
    const string _SMARTLINGORIGINHOST = "zz.originalhost.com";
    const string _SMARTLINGSECRET = "SLDEBUG";
    const string _SMARTLINGLANGUAGE = "fr-FR";

    private void AppendARRHeaders(Dictionary<string, string> headers, string ipAddress)
    {
      headers["X-ARR-OriginalIP"] = ipAddress;
      headers["X-ARR-OriginalHost"] = _ARRORIGINHOST;
    }

    private void AppendCustomResellerHeaders(Dictionary<string, string> headers, string ipAddress)
    {
      headers["X-ARR-PL-OriginalIP"] = ipAddress;
      headers["X-ARR-PL-OriginalHost"] = _CUSTOMORIGINHOST;
    }

    private void AppendTranslationHeaders(Dictionary<string, string> headers, string ipAddress)
    {
      headers["X-OriginalIP"] = ipAddress;
      headers["X-OriginalHost"] = _TXORIGINHOST;
      headers["X-OriginalLang"] = _TXLANGUAGE;
    }

    private void AppendCountrySiteHeaders(Dictionary<string, string> headers, string ipAddress)
    {
      headers["X-CTRY-OriginalIP"] = ipAddress;
      headers["X-CTRY-OriginalHost"] = _COUNTRYORIGINHOST;
    }

    private void AppendAkamaiSiteHeaders(Dictionary<string, string> headers, string ipAddress)
    {
      headers["X-DSA-OriginalIP"] = ipAddress;
      headers["X-DSA-Secret"] = _AKAMAISECRET;
      headers["X-DSA-Host"] = _AKAMAIHOST;
    }

    private void AppendSmartlingHeaders(Dictionary<string, string> headers, string ipAddress)
    {
      headers["X-Smartling-OriginalIP"] = ipAddress;
      headers["X-Smartling-OriginalHost"] = _SMARTLINGORIGINHOST;
      headers["X-Smartling-Secret"] = _SMARTLINGSECRET;
      headers["X-Language-Locale"] = _SMARTLINGLANGUAGE;
    }

    [TestMethod]
    public void RequestARR()
    {
      const string ARRORIGINIP = "68.68.68.68";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendARRHeaders(headers, ARRORIGINIP);

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.IsTrue(context.IsProxyActive(ProxyTypes.LocalARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CustomResellerARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.TransPerfectTranslation));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CountrySiteARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.AkamaiDSA));

      IProxyData arrData;
      Assert.IsTrue(context.TryGetActiveProxy(ProxyTypes.LocalARR, out arrData));
      Assert.AreEqual(ARRORIGINIP, context.OriginIP);
      Assert.AreEqual(ARRORIGINIP, arrData.OriginalIP);
      Assert.AreEqual(_ARRORIGINHOST, arrData.OriginalHost);
      Assert.IsTrue(arrData.IsContextualHost);

      Assert.AreEqual(_ARRORIGINHOST, context.ContextHost);
      Assert.AreEqual(_ARRORIGINHOST, context.OriginHost);
    }

    [TestMethod]
    public void RequestARRFromRemote()
    {
      const string ARRORIGINIP = "68.68.68.68";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendARRHeaders(headers, ARRORIGINIP);

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      request.MockRemoteAddress(IPAddress.Parse("80.4.56.34"));
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.IsFalse(context.IsProxyActive(ProxyTypes.LocalARR));
      Assert.AreEqual(ProxyStatusType.Invalid, context.Status);
    }

    [TestMethod]
    public void RequestResellerDomain()
    {
      const string CUSTOMORIGINIP = "68.68.68.68";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendCustomResellerHeaders(headers, CUSTOMORIGINIP);

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.IsFalse(context.IsProxyActive(ProxyTypes.LocalARR));
      Assert.IsTrue(context.IsProxyActive(ProxyTypes.CustomResellerARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.TransPerfectTranslation));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CountrySiteARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.AkamaiDSA));

      IProxyData resellerData;
      Assert.IsTrue(context.TryGetActiveProxy(ProxyTypes.CustomResellerARR, out resellerData));
      Assert.AreEqual(CUSTOMORIGINIP, context.OriginIP);
      Assert.AreEqual(CUSTOMORIGINIP, resellerData.OriginalIP);
      Assert.AreEqual(_CUSTOMORIGINHOST, resellerData.OriginalHost);
      Assert.IsFalse(resellerData.IsContextualHost);

      Assert.AreEqual(_CUSTOMORIGINHOST, context.OriginHost);
      Assert.AreNotEqual(_CUSTOMORIGINHOST, context.ContextHost);
    }

    [TestMethod]
    public void ResellerInvalidHeaders()
    {
      const string CUSTOMORIGINIP = "68.68.68.68";
      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendCustomResellerHeaders(headers, CUSTOMORIGINIP);
      headers.Remove("X-ARR-PL-OriginalHost");

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CustomResellerARR));
      Assert.AreEqual(ProxyStatusType.Invalid, context.Status);
    }

    [TestMethod]
    public void ResellerNotWhiteListed()
    {
      const string CUSTOMORIGINIP = "68.68.68.68";
      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendCustomResellerHeaders(headers, CUSTOMORIGINIP);

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockRemoteAddress(IPAddress.Parse("80.8.132.123"));
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CustomResellerARR));
      Assert.AreEqual(ProxyStatusType.Invalid, context.Status);
    }

    [TestMethod]
    public void TranslationDomain()
    {
      const string TXORIGINIP = "68.68.68.68";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendTranslationHeaders(headers, TXORIGINIP);

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.IsFalse(context.IsProxyActive(ProxyTypes.LocalARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CustomResellerARR));
      Assert.IsTrue(context.IsProxyActive(ProxyTypes.TransPerfectTranslation));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CountrySiteARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.AkamaiDSA));

      Assert.AreEqual(TXORIGINIP, context.OriginIP);

      IProxyData transData;
      Assert.IsTrue(context.TryGetActiveProxy(ProxyTypes.TransPerfectTranslation, out transData));
      Assert.AreEqual(TXORIGINIP, transData.OriginalIP);
      Assert.AreEqual(_TXORIGINHOST, transData.OriginalHost);

      string language;
      Assert.IsTrue(transData.TryGetExtendedData("language", out language));
      Assert.AreEqual(_TXLANGUAGE, language);

      Assert.AreEqual(_TXORIGINHOST, context.OriginHost);
      Assert.AreEqual(_NONPROXIEDHOST, context.ContextHost);
    }

    [TestMethod]
    public void TranslationInvalidHeaders()
    {
      const string TXORIGINIP = "68.68.68.68";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendTranslationHeaders(headers, TXORIGINIP);
      headers.Remove("X-OriginalHost");

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.IsFalse(context.IsProxyActive(ProxyTypes.TransPerfectTranslation));
      Assert.AreEqual(ProxyStatusType.Invalid, context.Status);
    }

    [TestMethod]
    public void TranslationNotWhiteListed()
    {
      const string TXORIGINIP = "68.68.68.68";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendTranslationHeaders(headers, TXORIGINIP);

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockRemoteAddress(IPAddress.Parse("80.8.132.123"));
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.IsFalse(context.IsProxyActive(ProxyTypes.TransPerfectTranslation));
      Assert.AreEqual(ProxyStatusType.Invalid, context.Status);
    }

    [TestMethod]
    public void TranslatedARR()
    {
      const string ARRORIGINIP = "127.0.0.1";
      const string TXORIGINIP = "68.68.68.68";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendARRHeaders(headers, ARRORIGINIP);
      AppendTranslationHeaders(headers, TXORIGINIP);

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.IsTrue(context.IsProxyActive(ProxyTypes.LocalARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CustomResellerARR));
      Assert.IsTrue(context.IsProxyActive(ProxyTypes.TransPerfectTranslation));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CountrySiteARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.AkamaiDSA));

      IProxyData arrData;
      Assert.IsTrue(context.TryGetActiveProxy(ProxyTypes.LocalARR, out arrData));

      IProxyData transData;
      Assert.IsTrue(context.TryGetActiveProxy(ProxyTypes.TransPerfectTranslation, out transData));

      Assert.AreEqual(TXORIGINIP, context.OriginIP);
      Assert.AreEqual(TXORIGINIP, transData.OriginalIP);
      Assert.AreNotEqual(ARRORIGINIP, context.OriginIP);
      Assert.AreEqual(ARRORIGINIP, arrData.OriginalIP);

      Assert.AreEqual(_TXORIGINHOST, context.OriginHost);
      Assert.AreEqual(_ARRORIGINHOST, context.ContextHost);
    }

    [TestMethod]
    public void CountrySite()
    {
      const string COUNTRYORIGINIP = "68.68.68.68";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendCountrySiteHeaders(headers, COUNTRYORIGINIP);

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.IsFalse(context.IsProxyActive(ProxyTypes.LocalARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CustomResellerARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.TransPerfectTranslation));
      Assert.IsTrue(context.IsProxyActive(ProxyTypes.CountrySiteARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.AkamaiDSA));

      Assert.AreEqual(COUNTRYORIGINIP, context.OriginIP);

      IProxyData countrySiteData;
      context.TryGetActiveProxy(ProxyTypes.CountrySiteARR, out countrySiteData);

      Assert.AreEqual(COUNTRYORIGINIP, countrySiteData.OriginalIP);
      Assert.AreEqual(_COUNTRYORIGINHOST, countrySiteData.OriginalHost);

      Assert.AreEqual(_COUNTRYORIGINHOST, context.OriginHost);
      Assert.AreEqual(_COUNTRYORIGINHOST, context.ContextHost);
    }

    [TestMethod]
    public void CountrySiteARRSameBox()
    {
      const string ARRORIGINIP = "127.0.0.1";
      const string COUNTRYORIGINIP = "68.68.68.68";
      
      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendARRHeaders(headers, ARRORIGINIP);
      AppendCountrySiteHeaders(headers, COUNTRYORIGINIP);

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.IsTrue(context.IsProxyActive(ProxyTypes.LocalARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CustomResellerARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.TransPerfectTranslation));
      Assert.IsTrue(context.IsProxyActive(ProxyTypes.CountrySiteARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.AkamaiDSA));

      IProxyData countrySiteData;
      context.TryGetActiveProxy(ProxyTypes.CountrySiteARR, out countrySiteData);

      IProxyData arrData;
      context.TryGetActiveProxy(ProxyTypes.LocalARR, out arrData);

      Assert.AreEqual(COUNTRYORIGINIP, context.OriginIP);
      Assert.AreEqual(COUNTRYORIGINIP, countrySiteData.OriginalIP);
      Assert.AreEqual(ARRORIGINIP, arrData.OriginalIP);

      Assert.AreEqual(_COUNTRYORIGINHOST, context.OriginHost);
      Assert.AreEqual(_COUNTRYORIGINHOST, context.ContextHost);
    }

    [TestMethod]
    public void CountrySiteARRSameBoxWithOtherSpoofedHeaders()
    {
      const string ARRORIGINIP = "127.0.0.1";
      const string COUNTRYORIGINIP = "68.68.68.68";
      const string RESELLERORIGINIP = "100.100.100.100";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendARRHeaders(headers, ARRORIGINIP);
      AppendCountrySiteHeaders(headers, COUNTRYORIGINIP);
      AppendCustomResellerHeaders(headers, RESELLERORIGINIP);

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.AreEqual(ProxyStatusType.Invalid, context.Status);

      Assert.IsTrue(context.IsProxyActive(ProxyTypes.LocalARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CustomResellerARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.TransPerfectTranslation));
      Assert.IsTrue(context.IsProxyActive(ProxyTypes.CountrySiteARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.AkamaiDSA));

      IProxyData countrySiteData;
      context.TryGetActiveProxy(ProxyTypes.CountrySiteARR, out countrySiteData);

      Assert.AreEqual(COUNTRYORIGINIP, context.OriginIP);
      Assert.AreEqual(COUNTRYORIGINIP, countrySiteData.OriginalIP);
    }

    [TestMethod]
    public void AkamaiDSA()
    {
      const string AKAMAIORIGINIP = "44.44.44.44";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendAkamaiSiteHeaders(headers, AKAMAIORIGINIP);

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.IsFalse(context.IsProxyActive(ProxyTypes.LocalARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CustomResellerARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.TransPerfectTranslation));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CountrySiteARR));
      Assert.IsTrue(context.IsProxyActive(ProxyTypes.AkamaiDSA));

      IProxyData akamaiData;
      context.TryGetActiveProxy(ProxyTypes.AkamaiDSA, out akamaiData);

      Assert.AreEqual(AKAMAIORIGINIP, context.OriginIP);
      Assert.AreEqual(AKAMAIORIGINIP, akamaiData.OriginalIP);

      Assert.AreEqual(_AKAMAIHOST, context.OriginHost);
      Assert.AreEqual(_AKAMAIHOST, context.ContextHost);
    }

    [TestMethod]
    public void AkamaiDSADuplicatedHeaders()
    {
      const string AKAMAIORIGINIP = "44.44.44.44";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendAkamaiSiteHeaders(headers, AKAMAIORIGINIP);

      List<KeyValuePair<string, string>> headersList = new List<KeyValuePair<string, string>>(headers);
      headersList.Add(new KeyValuePair<string, string>("X-DSA-Secret", "DSADEBUG"));

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headersList);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.AreEqual(ProxyStatusType.Valid, context.Status);
      Assert.IsTrue(context.IsProxyActive(ProxyTypes.AkamaiDSA));

      IProxyData akamaiData;
      context.TryGetActiveProxy(ProxyTypes.AkamaiDSA, out akamaiData);

      Assert.AreEqual(AKAMAIORIGINIP, context.OriginIP);
      Assert.AreEqual(AKAMAIORIGINIP, akamaiData.OriginalIP);

      Assert.AreEqual(_AKAMAIHOST, context.OriginHost);
      Assert.AreEqual(_AKAMAIHOST, context.ContextHost);
    }

    [TestMethod]
    public void AkamaiDSADelimitedHeaders()
    {
      const string AKAMAIORIGINIP = "44.44.44.44";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendAkamaiSiteHeaders(headers, AKAMAIORIGINIP);
      headers["X-DSA-Secret"] = "DSADEBUG, DSADEBUG";

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.AreEqual(ProxyStatusType.Invalid, context.Status);
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.AkamaiDSA));
    }

    [TestMethod]
    public void AkamaiARR()
    {
      const string ARRORIGINIP = "127.0.0.1";
      const string AKAMAIORIGINIP = "44.44.44.44";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendARRHeaders(headers, ARRORIGINIP);
      AppendAkamaiSiteHeaders(headers, AKAMAIORIGINIP);

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.IsTrue(context.IsProxyActive(ProxyTypes.LocalARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CustomResellerARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.TransPerfectTranslation));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CountrySiteARR));
      Assert.IsTrue(context.IsProxyActive(ProxyTypes.AkamaiDSA));

      IProxyData akamaiData;
      context.TryGetActiveProxy(ProxyTypes.AkamaiDSA, out akamaiData);

      IProxyData arrData;
      context.TryGetActiveProxy(ProxyTypes.LocalARR, out arrData);

      Assert.AreEqual(AKAMAIORIGINIP, context.OriginIP);
      Assert.AreEqual(AKAMAIORIGINIP, akamaiData.OriginalIP);
      Assert.AreNotEqual(ARRORIGINIP, context.OriginIP);
      Assert.AreEqual(ARRORIGINIP, arrData.OriginalIP);

      Assert.AreEqual(_AKAMAIHOST, context.OriginHost);
      Assert.AreEqual(_AKAMAIHOST, context.ContextHost);
    }

    [TestMethod]
    public void AkamaiDSAMissingSecret()
    {
      const string AKAMAIORIGINIP = "44.44.44.44";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendAkamaiSiteHeaders(headers, AKAMAIORIGINIP);
      headers.Remove("X-DSA-Secret");

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.IsFalse(context.IsProxyActive(ProxyTypes.AkamaiDSA));

      Assert.AreNotEqual(AKAMAIORIGINIP, context.OriginIP);
    }

    [TestMethod]
    public void AkamaiDSAWrongSecret()
    {
      const string AKAMAIORIGINIP = "44.44.44.44";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendAkamaiSiteHeaders(headers, AKAMAIORIGINIP);
      headers["X-DSA-Secret"] = "blah";

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.IsFalse(context.IsProxyActive(ProxyTypes.AkamaiDSA));
      Assert.AreNotEqual(AKAMAIORIGINIP, context.OriginIP);
    }

    [TestMethod]
    public void NoHttpContext()
    {
      HttpContext.Current = null;

      IProxyContext context = new WebProxyContext(null);
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.LocalARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.CustomResellerARR));
      Assert.IsFalse(context.IsProxyActive(ProxyTypes.TransPerfectTranslation));

      Assert.AreEqual(0, context.ActiveProxyChain.Count());
    }

    [TestMethod]
    public void SmartlingTranslation()
    {
      const string SMARTLINGORIGINIP = "33.33.44.44";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendSmartlingHeaders(headers, SMARTLINGORIGINIP);

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();
      IProxyContext context = HttpProviderContainer.Instance.Resolve<IProxyContext>();

      Assert.AreEqual(ProxyStatusType.Valid, context.Status);
      Assert.IsTrue(context.IsProxyActive(ProxyTypes.SmartlingTranslation));
      Assert.AreEqual(1, context.ActiveProxyChain.Count());

      Assert.AreEqual(SMARTLINGORIGINIP, context.OriginIP);
      Assert.AreEqual(_SMARTLINGORIGINHOST, context.OriginHost);
      Assert.AreEqual(_SMARTLINGORIGINHOST, context.ContextHost);

      IProxyData smartlingProxy;
      context.TryGetActiveProxy(ProxyTypes.SmartlingTranslation, out smartlingProxy);

      Assert.AreEqual(SMARTLINGORIGINIP, smartlingProxy.OriginalIP);
      Assert.AreEqual(_SMARTLINGORIGINHOST, smartlingProxy.OriginalHost);
      Assert.IsTrue(smartlingProxy.IsContextualHost);

      string language;
      smartlingProxy.TryGetExtendedData("language", out language);
      Assert.AreEqual(_SMARTLINGLANGUAGE, language);
    }

    [TestMethod]
    public void ChainedAtlantisExceptionUrl()
    {
      const string ARRORIGINIP = "127.0.0.1";
      const string SMARTLINGORIGINIP = "44.44.44.44";

      Dictionary<string, string> headers = new Dictionary<string, string>();
      AppendARRHeaders(headers, ARRORIGINIP);
      AppendSmartlingHeaders(headers, SMARTLINGORIGINIP);

      MockHttpRequest request = new MockHttpRequest(_NONPROXIEDURL);
      request.MockHeaderValues(headers);
      MockHttpContext.SetFromWorkerRequest(request);
      AtlantisException.SetWebRequestProviderContainer(HttpProviderContainer.Instance);

      HttpProviderContainer.Instance.RegisterProvider<IProxyContext, WebProxyContext>();

      AtlantisException exception = new AtlantisException("ChainedAtlantisExceptionUrl", "0", "ChainedAtlantisExceptionUrl", string.Empty, null, null);
      string sourceUrl = exception.SourceURL;

      Assert.IsTrue(sourceUrl.Contains(_NONPROXIEDHOST));
      Assert.IsTrue(sourceUrl.Contains(_SMARTLINGORIGINHOST));

    }
  }
}
