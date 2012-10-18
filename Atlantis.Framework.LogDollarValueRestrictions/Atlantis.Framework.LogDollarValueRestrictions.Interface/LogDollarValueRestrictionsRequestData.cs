using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.LogDollarValueRestrictions.Interface
{
  public class LogDollarValueRestrictionsRequestData : RequestData
  {
    private TimeSpan _wsRequestTimeout = TimeSpan.FromSeconds(4);

    public LogDollarValueRestrictionsRequestData(
      string shopperID, string sourceURL, string orderID, string pathway,
      int pageCount, int totalPrice, bool isManager)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      TotalPrice = totalPrice;
      IsManager = isManager;
      RequestTimeout = _wsRequestTimeout;
    }

    #region Properties
    public bool IsManager  { get; set; }
    public int TotalPrice { get; set; }

    #endregion

    public override string GetCacheMD5()
    {
      throw new Exception("LogDollarValueRestrictions is not a cacheable request.");
    }
  }
}
