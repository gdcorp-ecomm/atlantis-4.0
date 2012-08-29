using System;
using Atlantis.Framework.QSCCapturePayment.Interface;
using Atlantis.Framework.QSCCapturePayment.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCCapturePayment.Tests
{
	[TestClass]
	public class QSCCapturePaymentTests
	{
		[TestMethod]
		[DeploymentItem("atlantis.config")]
		public void CapturePaymentSuccess()
		{
			string _shopperId = "837435";
			string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
			string _invoiceId = "0000002215";
			int requestId = 591;
			int _paymentId = 1242;

			QSCCapturePaymentRequest a = new QSCCapturePaymentRequest();

			QSCCapturePaymentRequestData request = new QSCCapturePaymentRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _paymentId);

			request.RequestTimeout = TimeSpan.FromSeconds(30);

			QSCCapturePaymentResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCCapturePaymentResponseData;

			Assert.IsFalse(response.IsSuccess);
			Console.WriteLine(response.ToXML());
		}
	}
}
