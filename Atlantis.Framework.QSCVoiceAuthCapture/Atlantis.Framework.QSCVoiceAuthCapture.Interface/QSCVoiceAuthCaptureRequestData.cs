using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.QSCVoiceAuthCapture.Interface
{
	public class QSCVoiceAuthCaptureRequestData : RequestData
	{
		public QSCVoiceAuthCaptureRequestData(string shopperId,
			string sourceURL,
			string orderId,
			string pathway,
			int pageCount,
			string accountUid,
			string invoiceId,
			int paymentId,
			string voiceAuthCode)
			: base(shopperId, sourceURL, orderId, pathway, pageCount)
		{
			RequestTimeout = TimeSpan.FromSeconds(5);

			AccountUid = accountUid;
			InvoiceId = invoiceId;
			PaymentId = paymentId;
			PaymentIdSpecified = true;
			VoiceAuthCode = voiceAuthCode;
			
		}

		public string AccountUid { get; set; }
		public string InvoiceId { get; set; }
		public int PaymentId { get; set; }
		public bool PaymentIdSpecified { get; set; }
		public string VoiceAuthCode { get; set; }

		#region Overrides of RequestData

		public override string GetCacheMD5()
		{
			throw new Exception("QSCVoiceAuthCapture is not a cacheable request.");
		}

		#endregion
	}
}
