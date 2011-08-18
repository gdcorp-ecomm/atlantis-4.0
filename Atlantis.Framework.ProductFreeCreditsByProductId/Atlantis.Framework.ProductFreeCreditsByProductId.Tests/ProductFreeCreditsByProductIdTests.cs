using System;
using System.Collections.Generic;
using System.Diagnostics;
using Atlantis.Framework.ProductFreeCreditsByProductId.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
      ProductFreeCreditsByProductIdRequestData requestData = new ProductFreeCreditsByProductIdRequestData(_shopperID
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 5
         , _hostingProductID
         , _hostingPrivateLabelID);


      ProductFreeCreditsByProductIdResponseData responseData;

      try
      {
        responseData = (ProductFreeCreditsByProductIdResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

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
