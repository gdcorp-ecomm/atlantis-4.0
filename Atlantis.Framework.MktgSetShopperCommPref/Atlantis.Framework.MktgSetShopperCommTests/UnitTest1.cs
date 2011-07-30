using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.MktgSetShopperCommPref.Interface;

namespace Atlantis.Framework.MktgSetShopperCommTests
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void SetPreference()
    {
      MktgSetShopperCommPrefRequestData request = new MktgSetShopperCommPrefRequestData("850774", string.Empty, string.Empty, string.Empty,
                                       0,3,true);
      MktgSetShopperCommPrefResponseData response = (MktgSetShopperCommPrefResponseData)Engine.Engine.ProcessRequest(request, 337);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
