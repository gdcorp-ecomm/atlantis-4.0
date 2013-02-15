using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.ProductBillingResourceInfo.Interface;

namespace Atlantis.Framework.ProductBillingResourceInfo.Tests
{
  [TestClass]
  public class GetBillingResourceInfo
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ProductBillingResourceInfo.Impl.dll")]
    public void GetResourcesTest()
    {
      ProductBillingResourceInfoRequestData request = new ProductBillingResourceInfoRequestData("850774", string.Empty, string.Empty,string.Empty,0);

      ProductBillingResourceInfoResponseData response =
        (ProductBillingResourceInfoResponseData) Engine.Engine.ProcessRequest(request, 653);

      List<BillingResourceInfo> billingResources = response.GetBillingResourceList;

    }
  }
}
