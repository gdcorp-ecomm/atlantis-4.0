using System;
using Atlantis.Framework.QSCOrderCntByStatus.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCOrderCntByStatus.Tests
{
  [TestClass]
  public class QSCOrderCntByStatusTests
  {
    [TestMethod]
    public void GetOrderStatusesForShopAccount()
    {
      string _shopperId = "847235";
      string _shopUid = "265ddd62-2f88-11de-baa9-005056956427";
      int requestId = 567;

      QSCOrderCntByStatusRequestData request = new QSCOrderCntByStatusRequestData(_shopperId, "", string.Empty, string.Empty, 1, _shopUid, false);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCOrderCntByStatusResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCOrderCntByStatusResponseData;

      Assert.IsTrue(response.IsSuccess);
      

      Console.WriteLine(response.ToXML());
    }
  }
}
