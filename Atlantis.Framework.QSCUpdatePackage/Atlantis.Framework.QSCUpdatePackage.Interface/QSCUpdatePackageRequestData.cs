using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;

namespace Atlantis.Framework.QSCUpdatePackage.Interface
{
  public class QSCUpdatePackageRequestData : RequestData
  {
    public QSCUpdatePackageRequestData(string shopperId,
      string sourceURL,
      string orderId,
      string pathway,
      int pageCount,
      string accountUid,
      string invoiceId,
      package package)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      InvoiceId = invoiceId;
      Package = package;
    }

    public string AccountUid { get; set; }
    public string InvoiceId { get; set; }
    public package Package { get; set; }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("QSCUpdatePackage is not a cacheable request.");
    }

    #endregion
  }
}
