﻿using System;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCEditOrderEmail.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCEditOrderEmail.Tests
{
  [TestClass]
  public class QSCEditOrderEmailTests
  {
    [TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCEditOrderEmail.Impl.dll")]
		public void ChangeValidInvoiceEmailAddress()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "2219";
      string _emailAddress = "apitest@somedomain.com";
      int requestId = 550;

      QSCEditOrderEmailRequestData request = new QSCEditOrderEmailRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _emailAddress);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCEditOrderEmailResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCEditOrderEmailResponseData;

      Assert.IsTrue(response.IsSuccess);

      Console.WriteLine(response.ToXML());
    }

    [TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCEditOrderEmail.Impl.dll")]
		public void ChangeInvalidInvoiceEmailAddress()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "1";
      string _emailAddress = "apitest@somedomain.com";
      int requestId = 550;

      QSCEditOrderEmailRequestData request = new QSCEditOrderEmailRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _emailAddress);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCEditOrderEmailResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCEditOrderEmailResponseData;

      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.Response.responseStatus.statusCode == statusCode.FAILURE);

      Console.WriteLine(response.ToXML());
    }
  }
}
