using Atlantis.Framework.Brand.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Xml.Linq;

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

    [TestMethod]
    public void CompanyNameRequestDataProperties()
    {
        CompanyNameRequestData request = new CompanyNameRequestData(PLContextID);

        Assert.AreEqual(PLContextID, request.ContextId);
        XElement.Parse(request.ToXML());

        Assert.AreEqual("6", request.GetCacheMD5());
    }

    [TestMethod]
    public void CompanyNameRequestDataCacheKey()
    {
        CompanyNameRequestData request1 = new CompanyNameRequestData(GDContextID);
        CompanyNameRequestData request2 = new CompanyNameRequestData(PLContextID);
        CompanyNameRequestData request3 = new CompanyNameRequestData(PLContextID);

        Assert.AreNotEqual(request1.GetCacheMD5(), request2.GetCacheMD5());
        Assert.AreEqual(request2.GetCacheMD5(), request3.GetCacheMD5());
    }

    [TestMethod]
    public void CompanyNameResponseDataProperties()
    {
        CompanyNameRequestData request = new CompanyNameRequestData(GDContextID);

        CompanyNameResponseData response = (CompanyNameResponseData)Engine.Engine.ProcessRequest(request, 726);

        Assert.AreEqual(response.GetName("name"), "GoDaddy");

        XElement.Parse(response.ToXML());
    }
  }
}
