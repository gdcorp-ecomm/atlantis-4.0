using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.MarketingPublication.Interface;

namespace Atlantis.Framework.MarketingPublication.Tests
{
  [TestClass]
  public class MarketingPublicationTests
  {
    [TestMethod]
    public void SetInterestPreference()
    {
      MktgSetShopperInterestPrefRequestData request = new MktgSetShopperInterestPrefRequestData("850774", 1, 2, true);
      MktgSetShopperInterestPrefResponseData response = (MktgSetShopperInterestPrefResponseData)Engine.Engine.ProcessRequest(request, 757);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void SetCommPreference()
    {
      MktgSetShopperCommPrefRequestData request = new MktgSetShopperCommPrefRequestData("850774", string.Empty, string.Empty, string.Empty,
                                       0, 3, true);
      MktgSetShopperCommPrefResponseData response = (MktgSetShopperCommPrefResponseData)Engine.Engine.ProcessRequest(request, 337);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void SetDoubleOpt()
    {
      MktgSetShopperCommDoubleOptRequestData request = new MktgSetShopperCommDoubleOptRequestData("850774", string.Empty, string.Empty, string.Empty, 0, 3);
      MktgSetShopperCommDoubleOptResponseData response = (MktgSetShopperCommDoubleOptResponseData)Engine.Engine.ProcessRequest(request, 341);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void GetPreferences()
    {
      MktgGetShopperPreferencesRequestData request = new MktgGetShopperPreferencesRequestData("850774", string.Empty, string.Empty, string.Empty,
                                 0);
      MktgGetShopperPreferencesResponseData response = (MktgGetShopperPreferencesResponseData)Engine.Engine.ProcessRequest(request, 340);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
