using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.ProductFreeCreditsByResource.Interface;

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
    [DeploymentItem("bin/Interop.gdDataCacheLib.dll")]
    [DeploymentItem("bin/Interop.ADODB.dll")]
    public void GetHostingProductFreeCredits()
    {
      var requestData = new ProductFreeCreditsByResourceRequestData(_shopperID,
                                                                    string.Empty,
                                                                    string.Empty,
                                                                    string.Empty,
                                                                    0,
                                                                    _hostingResourceID,
                                                                    _hostingProductTypeID);
      requestData.RequestTimeout = TimeSpan.FromSeconds(5d);

      try
      {
        var responseData = (ProductFreeCreditsByResourceResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Debug.WriteLine(string.Format("Available Free Credits: {0}", responseData.ResourceFreeCredits.Keys.Count));
        foreach (List<ResourceFreeCredit> rfcGroup in responseData.ResourceFreeCredits.Values)
        {
          foreach (var rfc in rfcGroup)
          {
            Debug.WriteLine(string.Format("Namespace:{0}, UnifiedProductId:{1}, Quantity:{2}", rfc.ProductNamespace, rfc.UnifiedProductId, rfc.Quantity));
          }
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
