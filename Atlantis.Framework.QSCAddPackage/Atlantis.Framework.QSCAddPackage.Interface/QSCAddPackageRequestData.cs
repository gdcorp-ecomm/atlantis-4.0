using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;

namespace Atlantis.Framework.QSCAddPackage.Interface
{
  public class QSCAddPackageRequestData : RequestData
  {
    public QSCAddPackageRequestData(string shopperId, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount,
      string accountUid,
      string invoiceId)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      InvoiceId = invoiceId;
      Items = new List<itemReference>();
    }

    public string AccountUid { get; set; }
    public string InvoiceId { get; set; }
    public List<itemReference> Items { get; set; }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("QSCAddPackage is not a cacheable request.");
    }

    #endregion
  }
}
