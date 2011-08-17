using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.BonzaiRemoveAddOn.Interface;


namespace Atlantis.Framework.BonzaiRemoveAddOn.Tests
{
  [TestClass]
  public class GetBonzaiRemoveAddOnTests
  {

    private const string _shopperId = "857527";
    private const string _accountUid = "7ca57dd5-2c97-11df-8040-005056956427";
    private const string _attributeUid = "d347bfde-2658-4bfc-97f9-fad3ac985cd3";
    private const string _addOnType = "DedicatedIp";  // "ColdFusion";
    private const int _bonzaiRequestType = 136;

    public GetBonzaiRemoveAddOnTests()
    { }

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void BonzaiRemoveAddOnTest()
    {
      //Note: you must have a hosting acct with DedIp or ColdFusion (using the right attributeUid) for this to work
      var request = new BonzaiRemoveAddOnRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , _accountUid
         , _attributeUid
         , _addOnType);

      var response = (BonzaiRemoveAddOnResponseData)Engine.Engine.ProcessRequest(request, _bonzaiRequestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
