using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCRemovePackage.Interface
{
  public class QSCRemovePackageRequestData : RequestData
  {
    public QSCRemovePackageRequestData(string shopperId, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount,
      string accountUid,
      string invoiceId,
      int packageId) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      InvoiceId = invoiceId;
      PackageId = packageId;
    }

    public string AccountUid { get; set; }
    public string InvoiceId { get; set; }
    public int PackageId { get; set; }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("QSCRemovePackage is not a cacheable request.");
    }

    #endregion
  }
}
