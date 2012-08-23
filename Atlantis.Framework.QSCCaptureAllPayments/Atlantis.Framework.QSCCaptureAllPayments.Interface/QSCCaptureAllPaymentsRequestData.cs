using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCCaptureAllPayments.Interface
{
	public class QSCCaptureAllPaymentsRequestData : RequestData
	{
		public QSCCaptureAllPaymentsRequestData(string shopperId,
			string sourceURL,
			string orderId,
			string pathway,
			int pageCount,
			string accountUid,
			string invoiceId)
			: base(shopperId, sourceURL, orderId, pathway, pageCount)
		{
			RequestTimeout = TimeSpan.FromSeconds(5);

			AccountUid = accountUid;
			InvoiceId = invoiceId;
		}

		public string AccountUid { get; set; }
		public string InvoiceId { get; set; }

		#region Overrides of RequestData

		public override string GetCacheMD5()
		{
			throw new Exception("QSCCaptureAllPayments is not a cacheable request.");
		}

		#endregion
	}
}
