using System.Diagnostics;
using Atlantis.Framework.MyaAvailableProductNamespaces.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MyaAvailableProductNamespaces.Test
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("app.config")]
    public void GetNamespaces()
    {
      var request = new MyaAvailableProductNamespacesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (MyaAvailableProductNamespacesResponseData)Engine.Engine.ProcessRequest(request, 494);

      Debug.WriteLine(response.Namespaces.Rows.Count.ToString());
      Assert.IsTrue(response.IsSuccess);

    }
  }
}
