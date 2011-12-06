using System.Diagnostics;
using Atlantis.Framework.MyaProductBundleChildren.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaProductBundleChildren.Tests
{
  [TestClass]
  public class GetMyaProductBundleChildrenTests
  {

    //private const string _shopperId = "858346";
    //private const int _billingResourceId = 15680;   // Bundle with setup SEV = 15663 | Bundle with not setup SEV = 15680
    private const string _shopperId = "856907";
    private const int _billingResourceId = 15475;   // Bundle with setup SEV = 15663 | Bundle with not setup SEV = 15680
    private const int _requestType = 373;

    public GetMyaProductBundleChildrenTests()
    { }

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MyaProductBundleChildrenTest()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      var request = new MyaProductBundleChildrenRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , _billingResourceId);

      var response = SessionCache.SessionCache.GetProcessRequest<MyaProductBundleChildrenResponseData>(request, _requestType);
      var response2 = SessionCache.SessionCache.GetProcessRequest<MyaProductBundleChildrenResponseData>(request, _requestType);

      Debug.WriteLine(string.Format("Child Product Count: {0}", response.BundleChildProducts.Count));
      foreach (ChildProduct cp in response.BundleChildProducts)
      {
        Debug.WriteLine("ProductTypeId: {0} | BillingResourceId: {1} | OrionId: {2} | CommonName: {3} | UserWebsiteId: {4} | CustomerId: {5}"
          , cp.ProductTypeId, cp.BillingResourceId, cp.OrionResourceId, cp.CommonName, cp.UserWebsiteId, cp.CustomerId);
      }

      Assert.IsTrue(response.IsSuccess);
      for (int i = 0; i < response.BundleChildProducts.Count; i++)
      {
        Assert.AreEqual(response.BundleChildProducts[i].BillingResourceId, response2.BundleChildProducts[i].BillingResourceId);
      }
    }
  }
}
