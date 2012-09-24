using System;
using Atlantis.Framework.QSCUpdatePaymentStatus.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCUpdatePaymentStatus.Tests
{
	[TestClass]
	public class QSCUpdatePaymentStatusTests
	{
		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCUpdatePaymentStatus.Impl.dll")]
		public void ChangePaymentStatusToAllowedStatus()
		{
			string _shopperId = "837435";
			string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
			string _invoiceId = "2219";
			string _paymentStatus = "cancel";
			int _paymentId = 1246;
			int requestId = 601;

			QSCUpdatePaymentStatusRequestData request = new QSCUpdatePaymentStatusRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _paymentId, _paymentStatus);

			request.RequestTimeout = TimeSpan.FromSeconds(30);

			QSCUpdatePaymentStatusResponseData response = (QSCUpdatePaymentStatusResponseData)Engine.Engine.ProcessRequest(request, requestId);

			Assert.IsTrue(response.IsSuccess);
			Console.WriteLine(response.ToXML());
		}

		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCUpdatePaymentStatus.Impl.dll")]
		public void ChangePaymentStatusFailsIfNewStatusIsNotAllowed()
		{
			string _shopperId = "837435";
			string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
			string _invoiceId = "2219";
			string _paymentStatus = "new";
			int _paymentId = 1246;
			int requestId = 601;

			QSCUpdatePaymentStatusRequestData request = new QSCUpdatePaymentStatusRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _paymentId, _paymentStatus);

			request.RequestTimeout = TimeSpan.FromSeconds(30);

			QSCUpdatePaymentStatusResponseData response = (QSCUpdatePaymentStatusResponseData)Engine.Engine.ProcessRequest(request, requestId);

			Assert.IsFalse(response.IsSuccess);
			Console.WriteLine(response.ToXML());
		}
	}
}
