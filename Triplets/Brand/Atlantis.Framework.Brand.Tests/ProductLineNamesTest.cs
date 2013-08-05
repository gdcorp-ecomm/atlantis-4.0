using Atlantis.Framework.Brand.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.Brand.Impl;

namespace Atlantis.Framework.Brand.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Brand.Impl.dll")]
  public class ProductLineNamesTest
  {
    [TestMethod]
    public void SimpleGetProductLineNames()
    {
      int GODADDY_CONTEXTID = 1;

      ProductLineNameRequestData request = new ProductLineNameRequestData(GODADDY_CONTEXTID);

      ProductLineNameResponseData response = (ProductLineNameResponseData)Engine.Engine.ProcessRequest(request, 727);

      Assert.IsNotNull(response);
    }
  }
}
