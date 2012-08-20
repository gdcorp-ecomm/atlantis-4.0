using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;

namespace Atlantis.Framework.QSCUpdateContact.Interface
{
  public class QSCUpdateContactRequestData : RequestData
  {
    public QSCUpdateContactRequestData(string shopperId, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount,
      string accountUid,
      string invoiceId,
			orderContact contact
			) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      InvoiceId = invoiceId;
			Contact = contact;
    }

    public string AccountUid { get; set; }
    public string InvoiceId { get; set; }
    public orderContact Contact { get; set; }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("QSCUpdateContact is not a cacheable request.");
    }

    #endregion
  }
}
