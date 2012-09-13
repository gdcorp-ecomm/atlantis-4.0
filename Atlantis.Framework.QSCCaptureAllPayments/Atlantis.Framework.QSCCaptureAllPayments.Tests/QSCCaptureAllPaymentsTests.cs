using System;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCCaptureAllPayments.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCCaptureAllPayments.Tests
{
	[TestClass]
	public class QSCCaptureAllPaymentsTests
	{
		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCCaptureAllPayments.Impl.dll")]
		public void CaptureAllPaymentsFailure()
		{
			string _shopperId = "837435";
			string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
			string _invoiceId = "0000002222";
			int requestId = 585;

			QSCCaptureAllPaymentsRequestData request = new QSCCaptureAllPaymentsRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId);

			request.RequestTimeout = TimeSpan.FromSeconds(30);

			QSCCaptureAllPaymentsResponseData response = Engine.Engine.ProcessRequest(request, 585) as QSCCaptureAllPaymentsResponseData;

			Assert.IsFalse(response.IsSuccess);
			Console.WriteLine(response.ToXML());
		}
	}
}
