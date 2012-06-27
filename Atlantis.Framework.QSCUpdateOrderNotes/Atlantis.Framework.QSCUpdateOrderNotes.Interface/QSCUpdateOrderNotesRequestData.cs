using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCUpdateOrderNotes.Interface
{
  public class QSCUpdateOrderNotesRequestData : RequestData
  {
    public QSCUpdateOrderNotesRequestData(string shopperId, 
                                          string sourceURL, 
                                          string orderId, 
                                          string pathway, 
                                          int pageCount,
                                          string accountUid,
                                          string invoiceId,
                                          string orderNotes
      ) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
      
      AccountUid = accountUid;
      InvoiceId = invoiceId;
      OrderNotes = orderNotes;
    }

    public string AccountUid { get; set; }
    public string InvoiceId { get; set; }
    public string OrderNotes { get; set; }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("QSCUpdateOrderNotes is not a cacheable request.");
    }

    #endregion
  }
}
