using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;

namespace Atlantis.Framework.QSCGetOrders.Interface
{
  public class QSCGetOrdersRequestData : RequestData
  {
    public QSCGetOrdersRequestData(string shopperId, 
      string sourceURL, 
      string orderId, 
      string pathway, 
      int pageCount,
      string accountUid,
      int pageNumber,
      int pageSize) : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      AccountUid = accountUid;
      PageNumber = pageNumber;
      PageSize = pageSize;

      OrderSearchFields = new List<orderSearchField>();
    }

    public string AccountUid { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public List<orderSearchField> OrderSearchFields { get; set; }

    #region Overrides of RequestData

    public override string GetCacheMD5()
    {
			throw new Exception("QSCGetOrders is not a cacheable request.");
		}

    #endregion
  }
}
