using System;
using Atlantis.Framework.DotTypeAvailability.Interface;
using Atlantis.Framework.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DotTypeAvailability.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DotTypeAvailability.Impl.dll")]
  public class DotTypeAvailabilityTests
  {
    private const int _TLD_DOTTYPEAVAILABILITY_REQUEST = 753;

    [TestMethod]
    public void DotTypeAvailabilityGoodRequest()
    {
      var request = new DotTypeAvailabilityRequestData();
      var response = (DotTypeAvailabilityResponseData)Engine.Engine.ProcessRequest(request, 753);
      Assert.AreEqual(true, response.TldAvailabilityList != null && response.TldAvailabilityList.Count > 0);
    }

    [TestMethod]
    public void DotTypeAvailabilityGoodRequestTryGetByTldName()
    {
      var request = new DotTypeAvailabilityRequestData();
      var response = (DotTypeAvailabilityResponseData)Engine.Engine.ProcessRequest(request, 753);
      Assert.AreEqual(true, response.TldAvailabilityList != null && response.TldAvailabilityList.Count > 0);

      ITldAvailability tldAvailability;
      Assert.AreEqual(true, response.TldAvailabilityList != null && response.TldAvailabilityList.TryGetValue("L4.Borg", out tldAvailability) && tldAvailability != null);
    }

    [TestMethod]
    public void DotTypeAvailabilityExceptionResponse()
    {
      var requestData = new DotTypeAvailabilityRequestData();
      var ex = new AtlantisException("DotTypeAvailabilityExceptionResponse.Test", "0", "TestError", "TestData", null, null);
      DotTypeAvailabilityResponseData response = DotTypeAvailabilityResponseData.FromException(requestData, ex);
      Assert.AreEqual("TestError", response.GetException().Message);
    }

    [TestMethod]
    public void DotTypeAvailabilityWithBadRequestData()
    {
      var request = new XData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      try
      {
        Engine.Engine.ProcessRequest(request, _TLD_DOTTYPEAVAILABILITY_REQUEST);
      }
      catch (Exception ex)
      {
        Assert.AreEqual(true, !string.IsNullOrEmpty(ex.Message));
      }
    }

    [TestMethod]
    public void DotTypeAvailabilityGoodResponseToXml()
    {
      var request = new DotTypeAvailabilityRequestData();
      var response = (DotTypeAvailabilityResponseData)Engine.Engine.ProcessRequest(request, 753);
      Assert.AreEqual(true, !string.IsNullOrEmpty(response.ToXML()));
    }

    internal class XData : RequestData
    {
      internal XData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
        : base(shopperId, sourceURL, orderId, pathway, pageCount)
      {

      }

      public override string GetCacheMD5()
      {
        throw new NotImplementedException();
      }
    }
  }
}
