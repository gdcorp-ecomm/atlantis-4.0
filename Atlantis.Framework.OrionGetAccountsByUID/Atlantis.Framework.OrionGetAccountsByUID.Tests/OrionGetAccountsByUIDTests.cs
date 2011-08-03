using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.OrionGetAccountsByUID.Interface;

namespace Atlantis.Framework.OrionGetAccountsByUID.Tests
{
  [TestClass]
  public class GetOrionGetAccountsByUIDTests
  {

    private const string _shopperId = "840820";
    const string _appName = "Atlantis.Framework.OrionGetAccountsByUID.Tests";
    readonly string _messageId = string.Empty;
    readonly string[] _accountUid = { "7b233a4b-979b-497e-80ed-5dde71a31b0d" };
    readonly string[] _returnAttributeList = { "plan_features", "subdomain", "aliasdomain", "ftpuseraccount" };// Used to get orion accounts by attributes.           
    const int _requestType = 129;

    public GetOrionGetAccountsByUIDTests()
    {
    }

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("bin/netconnect.dll")]
    public void OrionGetAccountsByUIDTest()
    {
      var request = new OrionGetAccountsByUIDRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , _appName
         , _messageId
         , _accountUid
         , _returnAttributeList);

      var response = (OrionGetAccountsByUIDResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      //This line is needed for test harness only...
      //invoke datacache to properly load dll so call to datacache within OrionGetUsageRequest works
      DataCache.DataCache.GetPrivateLabelType(1);  

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
