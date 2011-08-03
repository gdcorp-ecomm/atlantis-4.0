using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Atlantis.Framework.OrionSecurityAuth.Interface;

namespace Atlantis.Framework.OrionSecurityAuth.Tests
{
  [TestClass]
  public class GetOrionSecurityAuthTests
  {

    private const string _shopperId = "840820";
    const int _requestType = 128;


    public GetOrionSecurityAuthTests()
    {
    }

    public TestContext TestContext { get; set; }

    #region Additional test attributes
    
    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("bin/netconnect.dll")]
    public void OrionSecurityAuthTest()
    {
      var request = new OrionSecurityAuthRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , "orion_security_mya");

      var response = (OrionSecurityAuthResponseData)DataCache.DataCache.GetProcessRequest(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
