using System;
using Atlantis.Framework.QSCUpdateOrderStatus.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCUpdateOrderStatus.Tests
{
  [TestClass]
  public class QSCUpdateOrderStatusTests
  {
    [TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCUpdateOrderStatus.Impl.dll")]
		public void ChangeOrderStatusToAllowedStatus()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "2172";
      string _orderStatus = "invoiced";
      int requestId = 557;

      QSCUpdateOrderStatusRequestData request = new QSCUpdateOrderStatusRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _orderStatus);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCUpdateOrderStatusResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCUpdateOrderStatusResponseData;

      Assert.IsTrue(response.IsSuccess);
      Console.WriteLine(response.ToXML());
    }

    [TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCUpdateOrderStatus.Impl.dll")]
		public void ChangeOrderStatusFailsIfNewStatusIsNotAllowed()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _invoiceId = "2138";
      string _orderStatus = "invoiced";
      int requestId = 557;

      QSCUpdateOrderStatusRequestData request = new QSCUpdateOrderStatusRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _invoiceId, _orderStatus);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCUpdateOrderStatusResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCUpdateOrderStatusResponseData;

      Assert.IsFalse(response.IsSuccess);
      Console.WriteLine(response.ToXML());
    }
  }
}
