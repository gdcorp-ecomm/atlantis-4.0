using System.Data;
using Atlantis.Framework.Manager.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Manager.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Manager.Impl.dll")]
  public class ManagerGetProductDetailTests
  {
    [TestMethod]
    public void VerifyDataSet()
    {
      var request = new ManagerGetProductDetailRequestData(111100, 1, 1, 5032);
      var response = (ManagerGetProductDetailResponseData) Engine.Engine.ProcessRequest(request, 395);
      DataTable dt = response.ProductCatalogDetails;
      Assert.IsTrue(dt != null && dt.Rows.Count == 1, "DataTable is null or does not contain any rows.");
    }
  }
}