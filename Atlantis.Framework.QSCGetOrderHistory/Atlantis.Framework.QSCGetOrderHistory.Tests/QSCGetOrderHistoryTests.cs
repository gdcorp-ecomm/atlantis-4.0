using System;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCGetOrderHistory.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCGetOrderHistory.Tests
{
	[TestClass]
	public class QSCGetOrderHistoryTests
	{
		[TestMethod]
		[DeploymentItem("atlantis.config")]
		public void GetOrderHistory()
		{
			string _shopperId = "837435";
			string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
			int requestId = 584;
			string _invoiceId = "2178";

			QSCGetOrderHistoryRequestData request = new QSCGetOrderHistoryRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId);

			request.RequestTimeout = TimeSpan.FromSeconds(10);

			QSCGetOrderHistoryResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCGetOrderHistoryResponseData;

			Assert.IsTrue(response.IsSuccess);
			Console.WriteLine("Total Records Found: " + response.Response.resultSize);
			Console.WriteLine(response.ToXML());
		}
	}
}
