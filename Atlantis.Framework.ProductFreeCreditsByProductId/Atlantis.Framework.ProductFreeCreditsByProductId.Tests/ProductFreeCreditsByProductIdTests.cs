﻿using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.ProductFreeCreditsByProductId.Interface;

namespace Atlantis.Framework.ProductFreeCreditsByProductId.Tests
{
  [TestClass]
  public class ProductFreeCreditsByProductIdTests
  {
    private const int _requestType = 406;
    private const string _shopperID = "840420";
    private const int _hostingProductID = 7234;
    private const int _hostingPrivateLabelID = 1;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetHostingProductFreeCredits()
    {
      var requestData = new ProductFreeCreditsByProductIdRequestData(_shopperID
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , _hostingProductID
         , _hostingPrivateLabelID) {RequestTimeout = TimeSpan.FromSeconds(5d)};

      try
      {
        var responseData = (ProductFreeCreditsByProductIdResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Debug.WriteLine(string.Format("Available Free Credits: {0}", responseData.ProductFreeCredits.Count));
        foreach (ProductFreeCredit pfc in responseData.ProductFreeCredits)
        {
          Debug.WriteLine(string.Format("UnifiedProductId:{0}, BillingNamespace:{1}", pfc.UnifiedProductId, pfc.BillingNamespace));
        }

        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }

  }
}
