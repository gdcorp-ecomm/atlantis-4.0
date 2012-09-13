using System;
using Atlantis.Framework.QSC.Interface.QSCMobileAPI;
using Atlantis.Framework.QSCGetOrders.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCGetOrders.Tests
{
  [TestClass]
  public class QSCGetOrdersTests
  {
    [TestMethod]
		[DeploymentItem("atlantis.config")]
		[DeploymentItem("Atlantis.Framework.QSCGetOrders.Impl.dll")]
		public void GetOpenOrdersFromStore()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      int requestId = 549;

      // create an order search field object to get a group of orders (use the getOrderSearchParameters method to get a list of valid search options)
      // Examples are:
      // property = status & value = {Any: empty} | open | closed | new | potentialFraud | pending | cancel | refunded
      // property = invoiceId & value = {invoice number}
      
      // You can submit this object multiple times, if you want to filter on multiple criteria.
      // each property (status, startDateDisplay, etc) should only be submitted once. If multiple 'status' criteria are sent, only the last one will be used.
      orderSearchField orderSearchField = new orderSearchField();
      orderSearchField.property = "status";
      orderSearchField.value = string.Empty;
      orderSearchField.label = string.Empty; // Not required, and not used

      QSCGetOrdersRequestData request = new QSCGetOrdersRequestData(_shopperId, "", string.Empty, string.Empty, 1,_accountUid, 0, 10);
      
      // add the search criteria to the request
      request.OrderSearchFields.Add(orderSearchField);


      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCGetOrdersResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCGetOrdersResponseData;

      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.OrderList.Count >= 1);
      Console.WriteLine("Total Records Found: " + response.ResultSize );
      Console.WriteLine("Records This Page: " + response.OrderList.Count);

      Console.WriteLine(response.ToXML());
    }
  }
}
