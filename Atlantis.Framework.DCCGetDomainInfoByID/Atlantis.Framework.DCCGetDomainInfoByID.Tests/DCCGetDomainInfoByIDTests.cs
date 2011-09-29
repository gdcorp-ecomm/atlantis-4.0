using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DCCGetDomainInfoByID.Interface;

namespace Atlantis.Framework.DCCGetDomainInfoByID.Tests
{
  [TestClass]
  public class DCCGetDomainInfoByIDTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void GetDomainIdForShopper()
    {
      DCCGetDomainInfoByIDRequestData request = new DCCGetDomainInfoByIDRequestData("839627", 
                                                                                    string.Empty, 
                                                                                    string.Empty, 
                                                                                    string.Empty, 
                                                                                    0, 
                                                                                    "MOBILE_CSA_DCC", 
                                                                                    1666019);

      DCCGetDomainInfoByIDResponseData response = (DCCGetDomainInfoByIDResponseData)Engine.Engine.ProcessRequest(request, 119);
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void GetDomainIdNotBelongingToShopper()
    {
      DCCGetDomainInfoByIDRequestData request = new DCCGetDomainInfoByIDRequestData("847235",
                                                                                  string.Empty,
                                                                                  string.Empty,
                                                                                  string.Empty,
                                                                                  0,
                                                                                  "MOBILE_CSA_DCC",
                                                                                  1666019);

      DCCGetDomainInfoByIDResponseData response = (DCCGetDomainInfoByIDResponseData)Engine.Engine.ProcessRequest(request, 119);
      Assert.IsTrue(!response.IsSuccess);
      Assert.IsTrue(!string.IsNullOrEmpty(response.ValidationMessage));
    }
  }
}
