using System.Diagnostics;
using Atlantis.Framework.CRMLynxPermission.Interface;
using Atlantis.Framework.Testing.MockHttpContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.CRMLynxPermission.Tests
{
  [TestClass]
  public class GetCRMLynxPermissionTests
  {

    private const string _shopperId = "";
    private const int _mgrUserId = 4692;
    private const int _requestType = 353;

    public GetCRMLynxPermissionTests()
    { }

    public TestContext TestContext { get; set; }

    #region Additional test attributes

    #endregion

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CRMLynxPermission.Impl.dll")]
    public void CRMLynxPermissionTest()
    {
      var request = new CRMLynxPermissionRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , _mgrUserId
         , "CancelSetupProducts");

      var response = (CRMLynxPermissionResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CRMLynxPermission.Impl.dll")]
    public void SerializeTest()
    {
      MockHttpContext.SetMockHttpContext(string.Empty, "http://localhost", string.Empty);

      var request = new CRMLynxPermissionRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , _mgrUserId
         , "CancelSetupProducts");

      var response = SessionCache.SessionCache.GetProcessRequest<CRMLynxPermissionResponseData>(request, _requestType);

      string xml = response.SerializeSessionData();
      Debug.WriteLine(xml);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
