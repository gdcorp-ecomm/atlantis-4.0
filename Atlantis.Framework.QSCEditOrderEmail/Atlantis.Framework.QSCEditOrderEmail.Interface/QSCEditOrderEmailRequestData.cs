using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCEditOrderEmail.Interface
{
  public class QSCEditOrderEmailRequestData : RequestData
  {
    public QSCEditOrderEmailRequestData(string shopperId, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount,
      string accountUid,
      string invoiceId,
      string emailAddress
      ) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);

      AccountUid = accountUid;
      InvoiceId = invoiceId;
      EmailAddress = emailAddress;
    }

    public string AccountUid { get; set; }
    public string InvoiceId { get; set; }
    public string EmailAddress { get; set; }
    
    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("QSCEditOrderEmail is not a cacheable request.");
    }

    #endregion
  }
}
