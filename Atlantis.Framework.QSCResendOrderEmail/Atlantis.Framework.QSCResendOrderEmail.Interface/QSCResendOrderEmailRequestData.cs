using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.Constants;

namespace Atlantis.Framework.QSCResendOrderEmail.Interface
{
  public class QSCResendOrderEmailRequestData : RequestData
  {
    public QSCResendOrderEmailRequestData(string shopperId, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount,
      string accountId,
      string invoiceId,
      string emailToResend,
      List<int> packageIds,
      List<int> unpackedContentIds)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);

      AccountUid = accountId;
      InvoiceId = invoiceId;
      EmailToResend = emailToResend;
      PackageIds = packageIds;
      UnpackedContentIds = unpackedContentIds;

      if (emailToResend == QSCEmailTypes.ORDER_SHIPPING_STATUS_NOTICE && (packageIds == null && unpackedContentIds == null ))
      {
        throw new AtlantisException(this, "QSCResendOrderEmailRequestData", "Order Shipping Status Notice Requires Package Ids and/or Unpacked Content Ids", string.Empty);
      }
    }

    public string AccountUid { get; set; }
    public string InvoiceId { get; set; }
    public string EmailToResend { get; set; }
    public List<int> PackageIds { get; set; }
    public List<int> UnpackedContentIds { get; set; }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("QSCResendOrderEmail is not a cacheable request.");
    }

    #endregion
  }
}
