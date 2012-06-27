using System;
using System.Diagnostics;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCUpdateOrderNotes.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCUpdateOrderNotes.Tests
{
  [TestClass]
  public class QSCUpdateOrderNotesTests
  {
    [TestMethod]
    public void UpdateValidInvoiceId()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "2219";
      string _orderNote = string.Format("This note added by QSCUpdateOrderNotes triplet via the QSC API at {0}.", DateTime.Now);
      int requestId = 548;

      QSCUpdateOrderNotesRequestData request = new QSCUpdateOrderNotesRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _orderNote);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCUpdateOrderNotesResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCUpdateOrderNotesResponseData;

      Assert.IsTrue(response.IsSuccess);

      Console.WriteLine(response.ToXML());
    }

    [TestMethod]
    public void UpdateInvalidInvoiceId()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "1";
      string _orderNote = string.Format("This note added by QSCUpdateOrderNotes triplet via the QSC API at {0}.", DateTime.Now);
      int requestId = 548;

      QSCUpdateOrderNotesRequestData request = new QSCUpdateOrderNotesRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _orderNote);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCUpdateOrderNotesResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCUpdateOrderNotesResponseData;

      Assert.IsFalse(response.IsSuccess);
      Assert.IsTrue(response.Response.responseStatus.statusCode == statusCode.FAILURE);

      Console.WriteLine(response.ToXML());
    }
  }
}
