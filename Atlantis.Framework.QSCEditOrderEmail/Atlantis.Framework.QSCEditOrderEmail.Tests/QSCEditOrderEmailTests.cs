using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCEditOrderEmail.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCEditOrderEmail.Tests
{
  [TestClass]
  public class QSCEditOrderEmailTests
  {
    [TestMethod]
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

      Debug.WriteLine(response.ToXML());
    }

    [TestMethod]
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

      Debug.WriteLine(response.ToXML());
    }
  }
}
