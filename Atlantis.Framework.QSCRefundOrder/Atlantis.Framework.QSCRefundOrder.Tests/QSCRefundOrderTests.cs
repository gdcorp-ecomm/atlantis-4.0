using System;
using Atlantis.Framework.QSCRefundOrder.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCRefundOrder.Tests
{
  [TestClass]
  public class QSCRefundOrderTests
  {
    [TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCRefundOrder.Impl.dll")]
    public void RefundInvalidInvoiceIdFails()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "";
      int requestId = 552;

      QSCRefundOrderRequestData request = new QSCRefundOrderRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCRefundOrderResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCRefundOrderResponseData;

      Assert.IsFalse(response.IsSuccess);
      Console.WriteLine(response.ToXML());
    }

    [TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCRefundOrder.Impl.dll")]
		public void RefundValidInvoiceId()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      
      // need to provide invoice number to refund...
      string _invoiceId = "";
      int requestId = 552;

      QSCRefundOrderRequestData request = new QSCRefundOrderRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCRefundOrderResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCRefundOrderResponseData;

      Assert.IsTrue(response.IsSuccess);
      Console.WriteLine(response.ToXML());
    }
  }
}
