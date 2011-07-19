using System;

namespace Atlantis.Framework.GetCoupons.Interface
{
  public class AdWordCoupon
  {
    #region Properties

    private string _couponKey = null;
    private string _orderId = null;
    private string _vendor = null;
    private int _vendorId = 0;
    private int _couponValue = 0;
    private bool _outOfStock = false;
    private string _couponCode = string.Empty;
    
    public string CouponKey { get { return _couponKey; } }

    public string OrderId { get { return _orderId; } }

    [Obsolete("Use VendorId and convert to friendly name yourself.  Logic is missing to convert Bing/Yahoo credit and anything else after Fotolia")]
    public string Vendor { get { return _vendor; } }

    public int VendorId { get { return _vendorId; } }

    public int CouponValue { get { return _couponValue; } }

    public bool OutOfStock { get { return _outOfStock; } }

    public string CouponCode { get { return _couponCode; } }

    #endregion 

    public AdWordCoupon(string couponKey
      , string orderId
      , string vendor
      , int vendorId
      , int couponValue
      , bool outOfStock )
    {
      _couponKey = couponKey;
      _orderId = orderId;
      _vendor = vendor;
      _vendorId = vendorId;
      _couponValue = couponValue;
      _outOfStock = outOfStock;
      _couponCode = string.Empty;
    }
    
    public AdWordCoupon(string couponKey
      , string orderId
      , string vendor
      , int vendorId
      , int couponValue
      , bool outOfStock
      , string couponCode)
    {
      _couponKey = couponKey;
      _orderId = orderId;
      _vendor = vendor;
      _vendorId = vendorId;
      _couponValue = couponValue;
      _outOfStock = outOfStock;
      _couponCode = couponCode;
    }
  }
}
