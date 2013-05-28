using System;
using Atlantis.Framework.Currency.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Currency.Tests
{
  [TestClass]
  public class CurrencyAllTests
  {
    const int REQUESTTYPE = 693000;

    [TestMethod]
    public void CurrencyTypesAllRequest()
    {
      var request = new CurrencyTypesRequestData();
      request.RequestTimeout = new TimeSpan(0, 0, 10);
      var response = (CurrencyTypesResponseData)Engine.Engine.ProcessRequest(request, REQUESTTYPE);
      Assert.IsNotNull(response);
      Assert.IsTrue(response.Count > 0);
    }

    [TestMethod]
    public void CurrencyTypesContainsUSD()
    {
      var request = new CurrencyTypesRequestData();
      request.RequestTimeout = new TimeSpan(0, 0, 10);
      var response = (CurrencyTypesResponseData)Engine.Engine.ProcessRequest(request, REQUESTTYPE);
      Assert.IsTrue(response.Contains("USD"));
    }

    [TestMethod]
    public void CurrencyTypesAllCheckIsActiveRequest()
    {
      var request = new CurrencyTypesRequestData();
      request.RequestTimeout = new TimeSpan(0, 0, 10);
      var response = (CurrencyTypesResponseData)Engine.Engine.ProcessRequest(request, REQUESTTYPE);
      Assert.IsNotNull(response);
      var isActive = true;
      foreach (var currInfo in response)
      {
        isActive = isActive && currInfo.IsActive;
      }
      Assert.IsTrue(isActive);
    }


  }
}
