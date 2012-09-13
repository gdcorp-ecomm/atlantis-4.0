using System;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCVoiceAuthCapture.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCVoiceAuthCapture.Tests
{
	[TestClass]
	public class QSCVoiceAuthCaptureTests
	{
		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCVoiceAuthCapture.Impl.dll")]
		public void VoiceAuthCaptureFailure()
		{
			string _shopperId = "837435";
			string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
			string _invoiceId = "0000002222";
			int requestId = 586;
			int _paymentId = 1249;
			string _voiceAuthCode = "test123";

			QSCVoiceAuthCaptureRequestData request = new QSCVoiceAuthCaptureRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _paymentId, _voiceAuthCode);

			request.RequestTimeout = TimeSpan.FromSeconds(30);

			QSCVoiceAuthCaptureResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCVoiceAuthCaptureResponseData;

			Assert.IsFalse(response.IsSuccess);
			Console.WriteLine(response.ToXML());
		}
	}
}
