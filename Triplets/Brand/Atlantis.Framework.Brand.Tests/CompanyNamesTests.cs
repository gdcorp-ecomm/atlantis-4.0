using Atlantis.Framework.Brand.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.Brand.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Brand.Impl.dll")]
  [DeploymentItem("Atlantis.Framework.PrivateLabel.Impl.dll")]
  public class CompanyNamesTests
  {
    private const int GDContextID = 1;
    private const int PLContextID = 6;
    private const int PrivateLabelID = 1724;

    [TestMethod]
    public void SimpleGetCompanyNames()
    {
      CompanyNameRequestData request = new CompanyNameRequestData(GDContextID);

      CompanyNameResponseData response = (CompanyNameResponseData)Engine.Engine.ProcessRequest(request, 726);

      Assert.IsNotNull(response);
    }

    [TestMethod]
    public void PrivateLabelCompanyNames()
    {
      CompanyNameRequestData request = new CompanyNameRequestData(PLContextID);

      CompanyNameResponseData response = (CompanyNameResponseData)Engine.Engine.ProcessRequest(request, 726);

      Assert.IsNotNull(response);
    }
  }
}
