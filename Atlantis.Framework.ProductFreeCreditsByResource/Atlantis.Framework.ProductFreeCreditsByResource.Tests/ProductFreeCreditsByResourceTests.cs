using System;
using System.Collections.Generic;
using System.Diagnostics;
using Atlantis.Framework.ProductFreeCreditsByResource.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ProductFreeCreditsByResource.Tests
{
  [TestClass]
  public class ProductFreeCreditsByResourceTests
  {
    private const int _requestType = 407;
    private const string _shopperID = "840420";
    private const int _hostingResourceID = 421122;
    private const int _hostingProductTypeID = 14;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetHostingProductFreeCredits()
    {
      ProductFreeCreditsByResourceRequestData requestData = new ProductFreeCreditsByResourceRequestData(_shopperID
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 5
         , _hostingResourceID
         , _hostingProductTypeID);


      ProductFreeCreditsByResourceResponseData responseData;

      try
      {
        responseData = (ProductFreeCreditsByResourceResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Debug.WriteLine(string.Format("Available Free Credits: {0}", responseData.ResourceFreeCredits.Count));
        foreach (ResourceFreeCredit rfc in responseData.ResourceFreeCredits)
        {
          Debug.WriteLine(string.Format("PackageId:{0}, UnifiedProductId:{1}, Quantity:{2}", rfc.FreeProductPackageId, rfc.UnifiedProductId, rfc.Quantity));
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
