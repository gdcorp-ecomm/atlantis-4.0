using System;
using System.Diagnostics;
using System.Text;
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
    [DeploymentItem("Atlantis.Framework.MyaAvailableProductNamespaces.Impl.dll")]
    public void GetNamespaces()
    {
      var request = new MyaAvailableProductNamespacesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "de");
      var response = (MyaAvailableProductNamespacesResponseData)Engine.Engine.ProcessRequest(request, 494);

      var namespaceList = new StringBuilder();
      foreach(ProductNamespace productNamespace in response.ProductNamespaces)
      {
        namespaceList.AppendFormat("Name: {0} - ProductGroupId: {1}, ", productNamespace.Namespace, productNamespace.ProductGroupId);
      }

      Debug.WriteLine(namespaceList.ToString());
      Console.WriteLine(namespaceList.ToString());
      Assert.IsTrue(response.IsSuccess);

    }
  }
}
