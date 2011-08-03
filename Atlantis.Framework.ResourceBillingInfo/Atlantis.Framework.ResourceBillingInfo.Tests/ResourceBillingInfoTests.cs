using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.ResourceBillingInfo.Interface;
using Atlantis.Framework.Testing.MockHttpContext;

namespace Atlantis.Framework.ResourceBillingInfo.Tests
{
  [TestClass]
  public class GetResourceBillingInfoTests
  {
    private const string _shopperId = "856907";
    private const int _requestType = 216;

    public GetResourceBillingInfoTests()
    { }

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("bin/netconnect.dll")]
    public void ResourceBillingInfoResourceIdTest()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      const int billingResourceId = 400508;
      var request = new ResourceBillingInfoRequestData(_shopperId
                                                       , string.Empty
                                                       , string.Empty
                                                       , string.Empty
                                                       , 0
                                                       , billingResourceId);

      var response1 = SessionCache.SessionCache.GetProcessRequest<ResourceBillingInfoResponseData>(request, _requestType);
      var response2 = SessionCache.SessionCache.GetProcessRequest<ResourceBillingInfoResponseData>(request, _requestType);

      Debug.WriteLine(response2.ToXML());
      Assert.IsTrue(response2.IsSuccess);
      Assert.IsTrue(response1.ToXML().Equals(response2.ToXML()));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("bin/netconnect.dll")]
    public void ResourceBillingInfoShopperIdTest()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      var request = new ResourceBillingInfoRequestData(_shopperId
                                                       , string.Empty
                                                       , string.Empty
                                                       , string.Empty
                                                       , 0
                                                       , null);

      var response1 = SessionCache.SessionCache.GetProcessRequest<ResourceBillingInfoResponseData>(request, _requestType);
      var response2 = SessionCache.SessionCache.GetProcessRequest<ResourceBillingInfoResponseData>(request, _requestType);

      Debug.WriteLine(response2.ToXML());
      Assert.IsTrue(response2.IsSuccess);
      Assert.IsTrue(response1.ToXML().Equals(response2.ToXML()));
    }
  }
}
