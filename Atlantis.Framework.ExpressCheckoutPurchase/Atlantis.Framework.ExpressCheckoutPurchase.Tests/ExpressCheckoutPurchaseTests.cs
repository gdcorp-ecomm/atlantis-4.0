using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.AddItem.Interface;
using Atlantis.Framework.ExpressCheckoutPurchase.Interface;


namespace Atlantis.Framework.ExpressCheckoutPurchase.Tests
{
  [TestClass]
  public class GetExpressCheckoutPurchaseTests
  {

    private const string _shopperId = "856907";  // Valid: 856907  WithFraudulentEmail: 857623
    private const int _xcRequestType = 178;


    public GetExpressCheckoutPurchaseTests()
    { }

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ExpressCheckoutPurchaseSucceedSingleTest()
    {
      var addItemRequest = new AddItemRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0);
      addItemRequest.AddItem("710", "1");

      var request = new ExpressCheckoutPurchaseRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , "DEVMYAWEB"
        , "172.16.44.254"
        , false
        , "Customer"
        , "Online"
        , addItemRequest
        , string.Empty
        , string.Empty);

      var response = (ExpressCheckoutPurchaseResponseData)Engine.Engine.ProcessRequest(request, _xcRequestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ExpressCheckoutPurchaseSucceedMultipleTest()
    {
      var addItemRequest = new AddItemRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0);
      addItemRequest.AddItem("5605", "1");
      //addItemRequest.AddItem("4437", "1");
      addItemRequest.AddItem("892", "1");

      var request = new ExpressCheckoutPurchaseRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , "DEVMYAWEB"
        , "172.16.44.254"
        , false
        , "Customer"
        , "Online"
        , addItemRequest
        , string.Empty
        , string.Empty);

      var response = (ExpressCheckoutPurchaseResponseData)Engine.Engine.ProcessRequest(request, _xcRequestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ExpressCheckoutPurchaseFailTest()
    {
      var addItemRequest = new AddItemRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0);

      var request = new ExpressCheckoutPurchaseRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , "DEVMYAWEB"
        , "172.16.44.254"
        , false
        , "Customer"
        , "Online"
        , addItemRequest
        , string.Empty
        , string.Empty);

      var response = (ExpressCheckoutPurchaseResponseData)Engine.Engine.ProcessRequest(request, _xcRequestType);

      Debug.WriteLine(response.ToXML());
      Debug.WriteLine(response.Error.Description);
      Assert.IsFalse(response.IsSuccess);
    }
  }
}
