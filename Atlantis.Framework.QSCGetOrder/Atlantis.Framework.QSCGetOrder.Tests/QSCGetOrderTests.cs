using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.QSCGetOrder.Interface;
using Atlantis.Framework.QSC.Interface.Enums;

namespace Atlantis.Framework.QSCGetOrder.Tests
{
	[TestClass]
	public class QSCGetOrderTests
	{
		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCGetOrder.Impl.dll")]
		public void GetOrderForValidInvoiceId()
		{
			string _shopperId = "837435";
			string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
			int requestId = 575;
			string _invoiceId = "0000002222";

			QSCGetOrderRequestData request = new QSCGetOrderRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId);
			request.RequestTimeout = TimeSpan.FromSeconds(10);
			QSCGetOrderResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCGetOrderResponseData;

			Assert.IsTrue(response.Response.responseStatus.statusCode.ToString() == QSCStatusCodes.SUCCESS.ToString());
			Console.WriteLine(response.ToXML());
		}

		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCGetOrder.Impl.dll")]
		public void GetOrderForInvalidInvoiceId()
		{
			string _shopperId = "837435";
			string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
			int requestId = 575;
			string _invoiceId = "000009999999";		//more than 10 chars

			QSCGetOrderRequestData request = new QSCGetOrderRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId);
			request.RequestTimeout = TimeSpan.FromSeconds(10);
			QSCGetOrderResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCGetOrderResponseData;

			Assert.IsTrue(response.Response.responseStatus.statusCode.ToString() == QSCStatusCodes.VALIDATION_ERROR.ToString());
			Console.WriteLine(response.ToXML());
		}

		[TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCGetOrder.Impl.dll")]
		public void GetNonExistingOrderForValidInvoiceId()
		{
			string _shopperId = "837435";
			string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
			int requestId = 575;
			string _invoiceId = "0009099999";		//10 chars

			QSCGetOrderRequestData request = new QSCGetOrderRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId);
			request.RequestTimeout = TimeSpan.FromSeconds(10);
			QSCGetOrderResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCGetOrderResponseData;

			Assert.IsTrue(response.Response.responseStatus.statusCode.ToString() == QSCStatusCodes.SUCCESS_WITH_WARNINGS.ToString());
			Console.WriteLine(response.ToXML());
		}

	}
}
