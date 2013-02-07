using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.ResourceCountByPaymentProfile.Interface;

namespace Atlantis.Framework.ResourceCountByPaymentProfile.Tests
{
  [TestClass]
  public class GetCount
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ResourceCountByPaymentProfile.Impl.dll")]
    public void GetCountTest()
    {
      ResourceCountByPaymentProfileRequestData request = new ResourceCountByPaymentProfileRequestData("850774", string.Empty, string.Empty, string.Empty, 0, 61795);
      ResourceCountByPaymentProfileResponseData response = (ResourceCountByPaymentProfileResponseData)Engine.Engine.ProcessRequest(request, 650);

      int count = response.TotalRecords;
      Assert.IsTrue(response.IsSuccess);


    }
  }
}
