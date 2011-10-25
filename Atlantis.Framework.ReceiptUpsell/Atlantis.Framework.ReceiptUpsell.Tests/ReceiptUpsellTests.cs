using System;
using Atlantis.Framework.ReceiptUpsell.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ReceiptUpsell.Tests
{
  [TestClass]
  public class ReceiptUpsellTests
  {
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    [TestMethod]
    public void LogUpsell()
    {
      ReceiptUpsellRequestData requestData = new ReceiptUpsellRequestData("847235",
                                                                          "localhost",
                                                                          string.Empty,
                                                                          Guid.NewGuid().ToString(),
                                                                          1,
                                                                          1,
                                                                          7001,
                                                                          1,
                                                                          599,
                                                                          766001,
                                                                          1,
                                                                          1199,
                                                                          string.Empty,
                                                                          1);

      ReceiptUpsellResponseData responseData = (ReceiptUpsellResponseData)Engine.Engine.ProcessRequest(requestData, 69);

      Assert.IsTrue(responseData.IsSuccess);
    }
  }
}
