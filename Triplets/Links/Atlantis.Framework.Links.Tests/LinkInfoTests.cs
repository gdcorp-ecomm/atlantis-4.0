﻿using afe = Atlantis.Framework.Engine;
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
      foreach (var item in response.Links)
      {
        Assert.IsNotNull(item.Value.CountryParameter);
        Assert.IsNotNull(item.Value.CountrySupportType);
        Assert.IsNotNull(item.Value.LanguageParameter);
        Assert.IsNotNull(item.Value.LanguageSupportType);
      }
    }

    [TestMethod]
    public void LinkInfoMissingForNoContext()
    {
      var request = new LinkInfoRequestData((int)ContextIds.NoContext);
      var response = (LinkInfoResponseData)afe.Engine.ProcessRequest(request, kEngineRequest);
      Assert.AreEqual(0, response.Links.Count);
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

  }
}