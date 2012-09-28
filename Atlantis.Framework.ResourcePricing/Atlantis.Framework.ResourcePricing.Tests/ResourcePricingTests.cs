using System.Diagnostics;
using Atlantis.Framework.ResourcePricing.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ResourcePricing.Tests
{
  [TestClass]
  public class ResourcePricingTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ResourcePricing.Impl.dll")]
    public void ResourcePricingTest()
    {
      const string shopperid = "842904";
      const string resourceid = "431794";
      const string resourcetype = "dedhost";
      const string idtype = "billing";
      const string currency = "usd";
      const string addlupidlist = "13153";

      var request = new ResourcePricingRequestData(shopperid, string.Empty, string.Empty, string.Empty, 0, resourceid, resourcetype, idtype, currency, string.Empty);
      var response = (ResourcePricingResponseData)Engine.Engine.ProcessRequest(request, 602);
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ResourcePricing.Impl.dll")]
    public void ResourcePricingTest2()
    {
      const string shopperid = "842904";
      const string resourceid = "1025223";
      const string resourcetype = "dedhost";
      const string idtype = "billing";
      const string currency = "usd";
      const string addlupidlist = "937";

      var request = new ResourcePricingRequestData(shopperid, string.Empty, string.Empty, string.Empty, 0, resourceid, resourcetype, idtype, currency, addlupidlist);
      var response = (ResourcePricingResponseData)Engine.Engine.ProcessRequest(request, 602);
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.ResourcePricing.Impl.dll")]
    public void ResourcePricingTest3()
    {
      const string shopperid = "842904";
      const string resourceid = "414825";
      const string resourcetype = "hosting";
      const string idtype = "billing";
      const string currency = "usd";
      

      var request = new ResourcePricingRequestData(shopperid, string.Empty, string.Empty, string.Empty, 0, resourceid, resourcetype, idtype, currency, string.Empty);
      var response = (ResourcePricingResponseData)Engine.Engine.ProcessRequest(request, 602);
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
