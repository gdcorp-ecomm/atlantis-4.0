using System;
using System.Diagnostics;
using Atlantis.Framework.QSCUnblockIP.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCUnblockIP.Tests
{
	[TestClass]
	public class QSCUnblockIPTests
	{
		[TestMethod]
		public void UnblockValidIPAddress()
		{
			string _shopperId = "837435";
			string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
			string _ipAddress = "192.168.1.1";
			int requestId = 587;

			QSCUnblockIPRequestData request = new QSCUnblockIPRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _ipAddress);

			request.RequestTimeout = TimeSpan.FromSeconds(30);

			QSCUnblockIPResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCUnblockIPResponseData;

			Assert.IsTrue(response.IsSuccess);

			Console.WriteLine(response.ToXML());
		}

		[TestMethod]
		public void UnblockInvalidIPAddress()
		{
			string _shopperId = "837435";
			string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
			string _ipAddress = "192";
			int requestId = 587;

			QSCUnblockIPRequestData request = new QSCUnblockIPRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _ipAddress);

			request.RequestTimeout = TimeSpan.FromSeconds(30);

			QSCUnblockIPResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCUnblockIPResponseData;

			Assert.IsFalse(response.IsSuccess);

			Console.WriteLine(response.ToXML());
		}
	}
}
