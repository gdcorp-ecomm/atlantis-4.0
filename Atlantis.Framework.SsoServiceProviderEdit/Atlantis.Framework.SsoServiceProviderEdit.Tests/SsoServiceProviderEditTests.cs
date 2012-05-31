using System;
using Atlantis.Framework.SsoServiceProviderEdit.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.SsoServiceProviderEdit.Tests
{
  [TestClass]
  public class SsoServiceProviderEditTests
  {
    private const int _requestType = 538;

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("bin/netconnect.dll")]
    public void SsoServiceProviderGetTest()
    {
      string serviceProviderName = "SSO-EDIT-TOOL";
      string identityProviderName = "SPIDP";
      string serviceProviderGroupName = "SPMUI";
      string loginReceive = "172.18.172.28";
      string loginReceiveType = "tcpip";
      string serverName = "C1WSDV-CSHIP";
      bool isRetired = true;
      DateTime? retiredDate = new DateTime(2012, 5, 8);
      string changedBy = "cshipley";
      string approvedBy = null;
      string actionDescription = null;

      var request = new SsoServiceProviderEditRequestData(string.Empty
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , serviceProviderName
        , identityProviderName
        , serviceProviderGroupName
        , loginReceive
        , loginReceiveType
        , serverName
        , isRetired
        , retiredDate
        , changedBy
        , approvedBy
        , actionDescription);

      var response = (SsoServiceProviderEditResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Assert.IsTrue(response.IsSuccess);
    }
  }
}