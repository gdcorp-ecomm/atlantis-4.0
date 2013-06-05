using System.Diagnostics;
using Atlantis.Framework.GetCoupons.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.GetCoupons.Tests
{
  [TestClass]
  public class GetCouponsTests
  {
    private const string _shopperId = "856907";

    private TestContext testContextInstance;

    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.GetCoupons.Impl.dll")]
    public void GetCoupons()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      GetCouponsRequestData request = new GetCouponsRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0);

      GetCouponsResponseData response = SessionCache.SessionCache.GetProcessRequest<GetCouponsResponseData>(request, 86);
      GetCouponsResponseData response2 = SessionCache.SessionCache.GetProcessRequest<GetCouponsResponseData>(request, 86);

      for (int i=0; i< response.GetCouponList().Count; i++)
      {
        AdWordCoupon coupon = response.GetCouponList()[i];
        AdWordCoupon coupon2 = response2.GetCouponList()[i];

        Assert.IsFalse(string.IsNullOrEmpty(coupon.CouponKey));
        Debug.WriteLine(string.Format("CouponKey: {0}", coupon.CouponKey)); 
        
        Assert.IsTrue(coupon.CouponValue > 0);
        Debug.WriteLine(string.Format("CouponValue: {0}", coupon.CouponValue)); 

        Assert.IsFalse(string.IsNullOrEmpty(coupon.OrderId));
        Debug.WriteLine(string.Format("OrderId: {0}", coupon.OrderId)); 

        Assert.IsFalse(coupon.VendorId.Equals(0));
        Debug.WriteLine(string.Format("VendorId: {0}", coupon.VendorId));
        Debug.WriteLine(string.Format("OutOfStock: {0}", coupon.OutOfStock));

        Debug.WriteLine(string.Format("ProvisionDate: {0}", coupon.ProvisionDate));
        Debug.WriteLine(string.Format("ExpirationDate: {0}", coupon.ExpirationDate));
        Debug.WriteLine("");

        Assert.AreEqual(coupon.CouponCode, coupon2.CouponCode);
      }

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
