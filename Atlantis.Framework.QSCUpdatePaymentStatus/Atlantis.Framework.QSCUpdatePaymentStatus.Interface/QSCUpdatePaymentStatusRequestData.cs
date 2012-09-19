using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCUpdatePaymentStatus.Interface
{
	public class QSCUpdatePaymentStatusRequestData : RequestData
	{
		public QSCUpdatePaymentStatusRequestData(string shopperId,
			string sourceURL,
			string orderId,
			string pathway,
			int pageCount,
			string accountUid,
			string invoiceId,
			int paymentId,
			string paymentStatus)
			: base(shopperId, sourceURL, orderId, pathway, pageCount)
		{
			AccountUid = accountUid;
			InvoiceId = invoiceId;
			PaymentId = paymentId;
			PaymentStatus = paymentStatus;
		}

		public string AccountUid { get; set; }
		public string InvoiceId { get; set; }
		public int PaymentId { get; set; }
		public string PaymentStatus { get; set; }

		#region Overrides of RequestData

		public override string GetCacheMD5()
		{
			throw new Exception("QSCUpdatePaymentStatus is not a cacheable request.");
		}

		#endregion
	}
}
