using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCCapturePayment.Interface
{
	public class QSCCapturePaymentRequestData : RequestData
	{
		public QSCCapturePaymentRequestData(string shopperId,
			string sourceURL,
			string orderId,
			string pathway,
			int pageCount,
			string accountUid,
			string invoiceId,
			int paymentId)
			: base(shopperId, sourceURL, orderId, pathway, pageCount)
		{
			RequestTimeout = TimeSpan.FromSeconds(5);

			AccountUid = accountUid;
			InvoiceId = invoiceId;
			PaymentId = paymentId;
			PaymentIdSpecified = true;
		}

		public string AccountUid { get; set; }
		public string InvoiceId { get; set; }
		public int PaymentId { get; set; }
		public bool PaymentIdSpecified { get; set; }

		#region Overrides of RequestData

		public override string GetCacheMD5()
		{
			throw new Exception("QSCCapturePayment is not a cacheable request.");
		}

		#endregion
	}
}
