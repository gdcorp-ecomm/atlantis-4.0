using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.CarmaGetDelegatedRoles.Interface;

namespace Atlantis.Framework.CarmaGetDelegatedRoles.Tests
{
  [TestClass]
  public class GetRoles
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.CarmaGetDelegatedRoles.Impl.dll")]
    public void TestMethod1()
    {
      int appId = 2; // - DCC, 1 - INTERNAL
      CarmaGetDelegatedRolesRequestData request = new CarmaGetDelegatedRolesRequestData("856907", string.Empty, string.Empty, string.Empty, 0, appId, 1665619, 1);

      CarmaGetDelegatedRolesResponseData response =
        (CarmaGetDelegatedRolesResponseData) Engine.Engine.ProcessRequest(request, 677);

    }
  }
}
