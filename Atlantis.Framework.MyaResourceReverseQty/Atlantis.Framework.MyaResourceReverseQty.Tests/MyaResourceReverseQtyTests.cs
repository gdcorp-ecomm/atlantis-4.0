using System;
using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.MyaResourceReverseQty.Interface;

namespace Atlantis.Framework.MyaResourceReverseQty.Tests
{
  [TestClass]
  public class MyaResourceReverseQtyTests
  {
    private const int _requestType = 394;
    private const string _shopperID = "840420";
    private const int _resourceID = 414599;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetMyaResourceReverseQuantity()
    {
      var requestData = new MyaResourceReverseQtyRequestData(_shopperID
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 1
         , _resourceID);


      MyaResourceReverseQtyResponseData responseData;

      try
      {
        responseData = (MyaResourceReverseQtyResponseData)Engine.Engine.ProcessRequest(requestData, _requestType);

        Debug.WriteLine(string.Format("Total Records: {0}", responseData.ResourceReverseQtyList.Count));
        foreach (ResourceReverseQty rrq in responseData.ResourceReverseQtyList)
        {
          Debug.WriteLine(string.Format("OrderID: {0}, RowID: {1}, CanBeReversedQty: {2}", rrq.OrderId, rrq.RowId, rrq.CanBeReversedQuantity));
        }

        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }


  }
}
