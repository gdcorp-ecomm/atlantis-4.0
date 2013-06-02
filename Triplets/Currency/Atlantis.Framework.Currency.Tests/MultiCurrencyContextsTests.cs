using Atlantis.Framework.Currency.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Atlantis.Framework.Currency.Tests
{
  [TestClass]
  public class MultiCurrencyContextsTests
  {
    [TestMethod]
    public void MultiCurrencyContextsCacheKeySame()
    {
      var request = new MultiCurrencyContextsRequestData();
      var request2 = new MultiCurrencyContextsRequestData();
      Assert.AreEqual(request.GetCacheMD5(), request2.GetCacheMD5());
    }

    [TestMethod]
    public void MultiCurrencyContextsResponseEmpty()
    {
      var response = MultiCurrencyContextsResponseData.FromDelimitedSetting(string.Empty);
      response.ValidateNotActive(1, 2, 5, 6);
    }

    [TestMethod]
    public void MultiCurrencyContextsResponseNull()
    {
      var response = MultiCurrencyContextsResponseData.FromDelimitedSetting(null);
      response.ValidateNotActive(1, 2, 5, 6);
    }

    [TestMethod]
    public void MultiCurrencyContextsResponseGarbage()
    {
      var response = MultiCurrencyContextsResponseData.FromDelimitedSetting("hello,blue");
      response.ValidateNotActive(1, 2, 5, 6);
    }

    [TestMethod]
    public void MultiCurrencyContextsResponseSomeGarbage()
    {
      var response = MultiCurrencyContextsResponseData.FromDelimitedSetting(",,2,hello,blue");
      response.ValidateNotActive(1, 5, 6);
      response.ValidateActive(2);
    }

    [TestMethod]
    public void MultiCurrencyContextsResponseGood()
    {
      var response = MultiCurrencyContextsResponseData.FromDelimitedSetting("333,444,555");
      response.ValidateNotActive(1, 2, 5, 6);
      response.ValidateActive(333, 444, 555);
    }

    const int _REQUESTTYPE = 707;

    [TestMethod]
    public void MultiCurrencyRequestSuccess()
    {
      var request = new MultiCurrencyContextsRequestData();
      var response = (MultiCurrencyContextsResponseData)Engine.Engine.ProcessRequest(request, _REQUESTTYPE);
      response.ValidateActive(1);
      XElement.Parse(response.ToXML());
    }
  }

  internal static class MultiCurrencyContextsResponseDataExtensions
  {
    public static void ValidateActive(this MultiCurrencyContextsResponseData response, params int[] contextIds)
    {
      foreach (int contextId in contextIds)
      {
        Assert.IsTrue(response.IsContextIdActive(contextId));
      }
    }

    public static void ValidateNotActive(this MultiCurrencyContextsResponseData response, params int[] contextIds)
    {
      foreach (int contextId in contextIds)
      {
        Assert.IsFalse(response.IsContextIdActive(contextId));
      }
    }
  }

}
