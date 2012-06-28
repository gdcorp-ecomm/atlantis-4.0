using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCUpdateOrderStatus.Interface
{
  public class QSCUpdateOrderStatusRequestData : RequestData
  {
    public QSCUpdateOrderStatusRequestData(string shopperId,
      string sourceURL,
      string orderId,
      string pathway,
      int pageCount,
      string accountUid,
      string invoiceId,
      string orderStatus)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      InvoiceId = invoiceId;
      OrderStatus = orderStatus;
    }

    public string AccountUid { get; set; }
    public string InvoiceId { get; set; }
    public string OrderStatus { get; set; }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("QSCUpdateOrderStatus is not a cacheable request.");
    }

    #endregion
  }
}
