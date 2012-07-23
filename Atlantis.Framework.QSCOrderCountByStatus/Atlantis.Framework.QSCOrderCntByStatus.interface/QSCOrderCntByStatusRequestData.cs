using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCOrderCntByStatus.Interface
{
  public class QSCOrderCntByStatusRequestData : RequestData
  {
    public QSCOrderCntByStatusRequestData(string shopperId, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount,
      string accountUid,
      bool onlyDashboardStatusTypes) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      OnlyDashboardStatusTypes = onlyDashboardStatusTypes;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public string AccountUid { get; set; }
    public bool OnlyDashboardStatusTypes { get; set; }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
      throw new Exception("Atlantis.Framework.QSCOrderCntByStatus is not a cacheable request.");
    }

    #endregion
  }
}
