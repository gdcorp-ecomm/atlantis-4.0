using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Manager.Interface;

namespace Atlantis.Framework.Manager.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Manager.Impl.dll")]
  public class UnitTest1
  {
    public string shopperId = "859147";
    public string userId = "mmicco";

    [TestMethod]

    public void GetManagerUser()
    {
      var lookupRequest = new ManagerUserLookupRequestData(shopperId, string.Empty, string.Empty, string.Empty, 0, userId);
      var lookupResponse = (ManagerUserLookupResponseData)DataCache.DataCache.GetProcessRequest(lookupRequest, 65);
      Debug.WriteLine(lookupResponse.ManagerUser.UserId);
      Debug.WriteLine(lookupResponse.ManagerUser.LoginName);
      Debug.WriteLine(lookupResponse.ManagerUser.FullName);
      Debug.WriteLine(lookupResponse.ManagerUser.Mstk);
      Assert.IsTrue(lookupResponse.IsSuccess);
    }
  }
}
