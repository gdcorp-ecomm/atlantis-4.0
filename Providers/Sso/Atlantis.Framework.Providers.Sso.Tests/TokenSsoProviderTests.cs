﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.Links;
using Atlantis.Framework.Providers.Links;
using Atlantis.Framework.Providers.Sso.Interface;
using Atlantis.Framework.Providers.Sso.Tests.Mocks;
using Atlantis.Framework.Providers.Sso.Tests.Mocks.Http;
using Atlantis.Framework.Providers.Tests;
using Atlantis.Framework.Sso.Interface.JsonHelperClasses;
using Atlantis.Framework.Testing.MockProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockHttpRequest = Atlantis.Framework.Testing.MockHttpContext.MockHttpRequest;

namespace Atlantis.Framework.Providers.Sso.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Sso.Impl.dll")]
  public class TokenSsoProviderTests
  {
    //NOTE THIS IS NOT VALID AFTER FOUR HOURS OF ISSUING, SO TESTS WILL FAIL
    private string _tokenData = "eyJhbGciOiJSUzI1NiIsImtpZCI6IklvbnFwUlhlSHcifQ.eyJzaG9wcGVySWQiOiI4Njc5MDAiLCJmaXJzdG5hbWUiOiJzZXRoIiwicGxpZCI6IjEiLCJmYWN0b3JzIjp7ImtfcHciOjEzODM2ODM1MzJ9LCJleHAiOjEzODM3MjY3MzIsImlhdCI6MTM4MzY4MzUzMiwianRpIjoiT0dOaVptRmtNbVV4T1ROa056WmlObVF4WWpVME1qbGtPVEptTm1NME9EWSIsInR5cCI6ImlkcCJ9.TfaBb0L9NKRibhzHMXLzW2e7HVmMHuPp-KJsMqSbuOcW-kfv4z6sISl3acron-O2-LdMrueGqR-qQfSuXiiF4226IYFkHvVWv-hG1it9rde3xsmRYDU4cBeaaMLbgBCu7EAatIPy61d-tcx1Ol3-JYpFb10VISg73j7GpHLtGxA";

    private IProviderContainer GetProviderContainer(string url, int privateLabelId = 1, ServerLocationType serverLocationType = ServerLocationType.Prod, string httpMethod = "POST", string virtualDirectoryName = "")
    {
      MockHttpRequest request = new MockCustomHttpRequest(url, httpMethod, virtualDirectoryName);

      var cc = new NameValueCollection();
      cc["auth_idp"] = _tokenData;

      request.MockCookies(cc);

      Testing.MockHttpContext.MockHttpContext.SetFromWorkerRequest(request);

      string filename;
      string queryString;
      ParseUrl(url, out filename, out queryString);
      var mockRequest = new Mocks.Http.MockHttpRequest(new HttpRequest(filename, url, queryString), httpMethod, virtualDirectoryName);

      var context = new Mocks.Http.MockHttpContext(mockRequest, new Mocks.Http.MockHttpResponse());
      HttpContextFactory.SetHttpContext(context);


      IProviderContainer result = new MockProviderContainer();
      result.RegisterProvider<IShopperContext, MockShopperContext>();
      result.RegisterProvider<ISiteContext, MockSiteContext>();
      result.RegisterProvider<IManagerContext, MockManagerContext>();
      result.RegisterProvider<ILinkProvider, LinkProvider>();
      result.RegisterProvider<ITokenSsoProvider, TokenSsoProvider>();

      result.SetData(MockSiteContextSettings.PrivateLabelId, privateLabelId);
      result.SetData(MockSiteContextSettings.ServerLocation, serverLocationType);
      result.SetData(MockManagerContextSettings.ShopperId, "867900");

      return result;
    }

    private void ParseUrl(string url, out string filename, out string queryString)
    {
      string[] parts = url.Split(new char[] { '?' }, 2, StringSplitOptions.RemoveEmptyEntries);
      if (parts.Length == 2)
        queryString = parts[1];
      else
      {
        queryString = string.Empty;
      }

      filename = parts[0].Substring(parts[0].LastIndexOf('/') + 1);
      if (!filename.Contains("."))
        filename = string.Empty;
    }

    [TestMethod]
    public void VerifyPayload()
    {
      var prov = GetProviderContainer("http://www.godaddy.com");
      var tokProv = prov.Resolve<ITokenSsoProvider>();

      Assert.IsTrue(PayloadHasAllData(tokProv.Token.Payload));
    }

    [TestMethod]
    public void TokenHasData()
    {
      var prov = GetProviderContainer("http://www.godaddy.com");
      var tokProv = prov.Resolve<ITokenSsoProvider>();

      Assert.IsTrue(tokProv.TokenHasData);
    }


    [TestMethod]
    [Ignore]
    public void TokenIsValid()
    {
      var prov = GetProviderContainer("http://www.godaddy.com");
      var tokProv = prov.Resolve<ITokenSsoProvider>();

      Assert.IsTrue(tokProv.IsTokenValid());
    }

    [TestMethod]
    public void TokenHasTimeout()
    {
      var prov = GetProviderContainer("http://www.godaddy.com");
      var tokProv = prov.Resolve<ITokenSsoProvider>();

      Assert.IsNotNull(tokProv.TokenTimeoutMins);
    }

    [TestMethod]
    public void CurrentShopperIsTokenShopper()
    {
      var prov = GetProviderContainer("http://www.godaddy.com");
      var tokProv = prov.Resolve<ITokenSsoProvider>();

      Assert.IsTrue(tokProv.CurrentShopperIsTokenShopper);
    }

    [TestMethod]
    public void CurrentLoggedInShopper()
    {
      var prov = GetProviderContainer("http://www.godaddy.com");
      var tokProv = prov.Resolve<ITokenSsoProvider>();

      Assert.IsTrue(tokProv.CurrentLoggedInShopperId == "867900");
    }

    [TestMethod]
    public void SetLoggedInShopper()
    {
      var prov = GetProviderContainer("http://www.godaddy.com");
      var tokProv = prov.Resolve<ITokenSsoProvider>();

      tokProv.SetLoggedInShopper();
    }

    [TestMethod]
    public void GetDevCookie()
    {
      var prov = GetProviderContainer("http://www.godaddy.com", 1, ServerLocationType.Dev);
      var tokProv = prov.Resolve<ITokenSsoProvider>();

      Assert.IsFalse(tokProv.TokenHasData);
    }

    [TestMethod]
    public void GetTestCookie()
    {
      var prov = GetProviderContainer("http://www.godaddy.com", 1, ServerLocationType.Test);
      var tokProv = prov.Resolve<ITokenSsoProvider>();

      Assert.IsFalse(tokProv.TokenHasData);
    }

    [TestMethod]
    public void GetOteCookie()
    {
      var prov = GetProviderContainer("http://www.godaddy.com", 1, ServerLocationType.Ote);
      var tokProv = prov.Resolve<ITokenSsoProvider>();

      Assert.IsFalse(tokProv.TokenHasData);
    }
    private bool PayloadHasAllData(Payload payload, bool checkTwoFactor = false)
    {
      var list = new List<string>();
      list.Add(payload.exp);
      list.Add(payload.firstname);
      list.Add(payload.iat);
      list.Add(payload.jti);
      list.Add(payload.plid);
      list.Add(payload.shopperId);
      list.Add(payload.typ);
      list.Add(payload.factors.k_pw);

      if (checkTwoFactor)
      {
        list.Add(payload.factors.p_sms);
      }

      return AllFieldsHaveData(list);
    }

    private bool AllFieldsHaveData(List<string> values)
    {
      foreach (string value in values)
      {
        if (string.IsNullOrEmpty(value))
        {
          return false;
        }
      }

      return true;
    }
  }
}
