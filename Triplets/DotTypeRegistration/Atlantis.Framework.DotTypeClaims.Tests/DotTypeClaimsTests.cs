﻿using System;
using Atlantis.Framework.DotTypeClaims.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DotTypeClaims.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeClaims.Impl.dll")]
  public class DotTypeClaimsTests
  {
    [TestMethod]
    public void DotTypeClaimsGoodRequest()
    {
      var request = new DotTypeClaimsRequestData(1764, "fos", "lr", "en-US", "validateandt9st.lrclaim");
      var response = (DotTypeClaimsResponseData)Engine.Engine.ProcessRequest(request, 710);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DotTypeClaimsTrademarkGoodRequest()
    {
      var request = new DotTypeClaimsRequestData(1794, "fos", "ga", "en-US", "testandvalidate.foreverclaims");
      var response = (DotTypeClaimsResponseData)Engine.Engine.ProcessRequest(request, 710);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    [TestMethod]
    public void DotTypeClaimsBadRequest()
    {
      var request = new DotTypeClaimsRequestData(1734, "FOS", "GA", "en-US", "123.lrclaim");
      try
      {
        var response = (DotTypeClaimsResponseData)Engine.Engine.ProcessRequest(request, 710);
        Assert.AreEqual(false, response.IsSuccess);
      }
      catch (Exception)
      {
        Assert.IsTrue(true);
      }
    }

    [TestMethod]
    public void DotTypeClaimsRequestData_ToXml()
    {
      var request = new DotTypeClaimsRequestData(1734, "FOS", "GA", "en-US", "123.lrclaim");
      var xml = request.ToXML();
      Assert.IsNotNull(xml);
    }
  }
}
