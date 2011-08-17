using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.BonzaiApplyAddOn.Interface;


namespace Atlantis.Framework.BonzaiApplyAddOn.Tests
{
  [TestClass]
  public class GetBonzaiApplyAddOnTests
  {

    private const string _shopperId = "857527";
    private const string _accountUID = "7ca57dd5-2c97-11df-8040-005056956427";
    private const string _addOnType = "DedicatedIp";  // "ColdFusion";
    private const int _bonzaiRequestType = 135;

    public GetBonzaiApplyAddOnTests()
    { }

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void BonzaiApplyAddOnTest()
    {
      //Note: you must have a DedicatedIP or ColdFusion addon credit for this to work
      var request = new BonzaiApplyAddOnRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , _accountUID
         , _addOnType);

      var response = (BonzaiApplyAddOnResponseData)Engine.Engine.ProcessRequest(request, _bonzaiRequestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
