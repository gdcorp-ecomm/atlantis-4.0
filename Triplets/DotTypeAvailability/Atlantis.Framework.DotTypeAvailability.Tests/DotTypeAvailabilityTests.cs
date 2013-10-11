using Atlantis.Framework.DotTypeAvailability.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DotTypeAvailability.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeAvailability.Impl.dll")]
  public class DotTypeAvailabilityTests
  {
    [TestMethod]
    public void DotTypeAvailabilityGoodRequest()
    {
      var request = new DotTypeAvailabilityRequestData();
      var response = (DotTypeAvailabilityResponseData)Engine.Engine.ProcessRequest(request, 753);
      Assert.AreEqual(true, response.IsSuccess);
      Assert.AreEqual(true, response.TldAvailabilityList != null);
    }

    [TestMethod]
    public void DotTypeAvailabilityBadRequest()
    {
    }
  }
}
