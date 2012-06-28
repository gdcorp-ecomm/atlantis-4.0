using System;
using System.Collections.Generic;
using Atlantis.Framework.QSC.Interface.Constants;
using Atlantis.Framework.QSCResendOrderEmail.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCResendOrderEmail.Tests
{
  [TestClass]
  public class QSCResendOrderEmailTests
  {
    [TestMethod]
    public void ResendOrderConfirmationForValidInvoice()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "2219";
      string _emailToSend = QSCEmailTypes.ORDER_CONFIRMATION_NOTICE;
      int requestId = 554;
      List<int> _selectedIds = new List<int>(0);

      QSCResendOrderEmailRequestData request = new QSCResendOrderEmailRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _emailToSend, _selectedIds, _selectedIds);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCResendOrderEmailResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCResendOrderEmailResponseData;

      Assert.IsTrue(response.IsSuccess);
      Console.WriteLine(response.ToXML());
    }

    [TestMethod]
    public void ResendOrderConfirmationForInvalidInvoice()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "A";
      string _emailToSend = QSCEmailTypes.ORDER_CONFIRMATION_NOTICE;
      int requestId = 554;
      List<int> _selectedIds = new List<int>(0);

      QSCResendOrderEmailRequestData request = new QSCResendOrderEmailRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _emailToSend, _selectedIds, _selectedIds);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCResendOrderEmailResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCResendOrderEmailResponseData;

      Assert.IsFalse(response.IsSuccess);
      Console.WriteLine(response.ToXML());
    }

    [TestMethod]
    public void ResendShippingNoticeForValidInvoice()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "1404";
      string _emailToSend = QSCEmailTypes.ORDER_SHIPPING_STATUS_NOTICE;
      int requestId = 554;
      List<int> _selectedIds = new List<int>() {2875};
      List<int> _selectedUnPackedIds = new List<int>(0);

      QSCResendOrderEmailRequestData request = new QSCResendOrderEmailRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _emailToSend, _selectedIds, _selectedUnPackedIds);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCResendOrderEmailResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCResendOrderEmailResponseData;

      Assert.IsTrue(response.IsSuccess);
      Console.WriteLine(response.ToXML());
    }

    [TestMethod]
    public void ResendShippingNoticeForValidInvoiceFailsWithoutShipmentId()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "1404";
      string _emailToSend = QSCEmailTypes.ORDER_SHIPPING_STATUS_NOTICE;
      int requestId = 554;
      List<int> _selectedIds = new List<int>(0);
      List<int> _selectedUnPackedIds = new List<int>(0);

      QSCResendOrderEmailRequestData request = new QSCResendOrderEmailRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _emailToSend, _selectedIds, _selectedUnPackedIds);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCResendOrderEmailResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCResendOrderEmailResponseData;

      Assert.IsFalse(response.IsSuccess);
      Console.WriteLine(response.ToXML());
    }

    [TestMethod]
    public void ResendOrderEditNoticeForValidInvoice()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "2180";
      string _emailToSend = QSCEmailTypes.ORDER_EDIT_NOTICE;
      int requestId = 554;
      List<int> _selectedIds = new List<int>(0);

      QSCResendOrderEmailRequestData request = new QSCResendOrderEmailRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _emailToSend, _selectedIds, _selectedIds);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCResendOrderEmailResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCResendOrderEmailResponseData;

      Assert.IsTrue(response.IsSuccess);
      Console.WriteLine(response.ToXML());
    }

    [TestMethod]
    public void ResendRefundNoticeForValidInvoice()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "2163";
      string _emailToSend = QSCEmailTypes.ORDER_REFUND_NOTICE;
      int requestId = 554;
      List<int> _selectedIds = new List<int>(0);

      QSCResendOrderEmailRequestData request = new QSCResendOrderEmailRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _emailToSend, _selectedIds, _selectedIds);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCResendOrderEmailResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCResendOrderEmailResponseData;

      Assert.IsTrue(response.IsSuccess);
      Console.WriteLine(response.ToXML());
    }
  }
}
