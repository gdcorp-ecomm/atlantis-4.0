using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaOrderHistory.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaOrderHistory.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.MyaOrderHistory.Impl.dll")]
  public class GetHistory
  {
    [TestMethod]
    public void GetOrdersByShopperId()
    {

      var request = new MyaOrderHistoryRequestData("840420", string.Empty, string.Empty, string.Empty, 0);

      var response = (MyaOrderHistoryResponseData)Engine.Engine.ProcessRequest(request, 571);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void GetOrdersByOrderIdCheckProductList()
    {

      var request = new MyaOrderHistoryRequestData("840420", string.Empty, "1461362", string.Empty, 0);

      var response = (MyaOrderHistoryResponseData)Engine.Engine.ProcessRequest(request, 571);

      var items = response.GetRecords;

      var item = items[0];

      Assert.IsTrue(item.NonUnifiedReceiptProductIds.Contains(764000));
      Assert.IsTrue(response.ToXML().Equals(string.Empty));
    }

    [TestMethod]
    public void GetOrdersByPaymentProfileId()
    {

      var request = new MyaOrderHistoryRequestData("840420", string.Empty, string.Empty, string.Empty, 0)
        {
          PaymentProfileId = 58071
        };

      var response = (MyaOrderHistoryResponseData)Engine.Engine.ProcessRequest(request, 571);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void GetOrdersByDomain()
    {

      var request = new MyaOrderHistoryRequestData("840420", string.Empty, string.Empty, string.Empty, 0)
        {
          DomainName = "ima.gogetterkickass"
        };

      var response = (MyaOrderHistoryResponseData)Engine.Engine.ProcessRequest(request, 571);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void GetOrdersByDateRange()
    {

      var request = new MyaOrderHistoryRequestData("840420", string.Empty, string.Empty, string.Empty, 0)
        {
          StartDate = DateTime.Now.AddYears(-1),
          EndDate = DateTime.Now
        };

      var response = (MyaOrderHistoryResponseData)Engine.Engine.ProcessRequest(request, 571);

      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void GetOrdersByProductGroup()
    {

      var request = new MyaOrderHistoryRequestData("840420", string.Empty, string.Empty, string.Empty, 0)
        {
          ProductGroupId = 1
        };

      var response = (MyaOrderHistoryResponseData)Engine.Engine.ProcessRequest(request, 571);

      Assert.IsTrue(response.GetRecords.Count > 0);
    }

    [TestMethod]
    public void GetOrdersByProductType()
    {

      var request = new MyaOrderHistoryRequestData("840420", string.Empty, string.Empty, string.Empty, 0)
      {
        ProductTypeId = 14
      };

      var response = (MyaOrderHistoryResponseData)Engine.Engine.ProcessRequest(request, 571);

      Assert.IsTrue(response.GetRecords.Count > 0);
    }

    [TestMethod]
    public void ReqestException()
    {
      try
      {
        var request = new MyaOrderHistoryRequestData("840420", string.Empty, "1461362", string.Empty, 0);

        Engine.Engine.ProcessRequest(request, 999571);

      }
      catch (Exception)
      {
        Assert.IsTrue(true);
      }
    }

    [TestMethod]
    public void ReqestExceptionBadConfig()
    {
      try
      {
        var request = new MyaOrderHistoryRequestData("840420", string.Empty, "1461362", string.Empty, 0);

        Engine.Engine.ProcessRequest(request, 99571);

      }
      catch (Exception)
      {
        Assert.IsTrue(true);
      }
    }

    [TestMethod]
    public void ResponseDataException()
    {
      var request = new MyaOrderHistoryRequestData("840420", string.Empty, "1461362", string.Empty, 0);

      var responseEx = new MyaOrderHistoryResponseData(new AtlantisException(request, "ResponseDataException", "Exception Test1", string.Empty));

      Assert.IsTrue(responseEx.GetException().ErrorDescription.Equals("Exception Test1"));
    }

    [TestMethod]
    public void ResponseDataExceptionRequest()
    {
      var request = new MyaOrderHistoryRequestData("840420", string.Empty, "1461362", string.Empty, 0);

      var responseEx = new MyaOrderHistoryResponseData(request, new AtlantisException(request, "ResponseDataException", "Exception Test2", string.Empty));

      Assert.IsTrue(responseEx.GetException().ErrorDescription.Equals("Exception Test2"));
    }

    [TestMethod]
    public void RequestMD5Test()
    {
      try
      {
        var request = new MyaOrderHistoryRequestData("840420", string.Empty, "1461362", string.Empty, 0);
        request.GetCacheMD5();
      }
      catch (Exception ex)
      {
        Assert.IsTrue(ex.Message.Equals("MyaOrderHistoryRequestData is not a cachable request."));
      }
    }
  }
}
