using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCGetOrderHistory.Interface
{
	public class QSCGetOrderHistoryRequestData : RequestData
	{
		public QSCGetOrderHistoryRequestData(string shopperId,
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
		}

		public string AccountUid { get; set; }
		public string InvoiceId { get; set; }

		#region Overrides of RequestData

		public override string GetCacheMD5()
		{
			throw new Exception("QSCGetOrderHistory is not a cacheable request.");
		}

		#endregion

	}
}
