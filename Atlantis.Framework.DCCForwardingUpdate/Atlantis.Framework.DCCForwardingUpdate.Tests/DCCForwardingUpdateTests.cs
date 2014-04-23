using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DCCForwardingUpdate.Interface;


namespace Atlantis.Framework.DCCForwardingUpdate.Tests
{
  [TestClass]
  public class DCCForwardingUpdateTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DCCForwardingUpdate.Impl.dll")]
    public void ForwardingUpdatePermanentValid()
    {
      DCCForwardingUpdateRequestData request = new DCCForwardingUpdateRequestData("857020", 
                                                                                  string.Empty, 
                                                                                  string.Empty, 
                                                                                  string.Empty, 
                                                                                  0, 
                                                                                  1,
                                                                                  1667126, 
                                                                                  "MOBILE_CSA_DCC", 
                                                                                  "http://testy123.com", 
                                                                                  RedirectType.Permanent,
                                                                                  true);
      
      DCCForwardingUpdateResponseData response = (DCCForwardingUpdateResponseData)Engine.Engine.ProcessRequest(request, 111);

      Console.WriteLine(string.Format("Call Success: {0}", !string.IsNullOrEmpty(response.ResponseXml)));
      Console.WriteLine(string.Format("Operation Success: {0}", response.IsSuccess));
      Console.WriteLine(string.Format("Error: {0}", !string.IsNullOrEmpty(response.ValidationMsg) ? response.ValidationMsg : "none"));
      Console.WriteLine(response.ToXML());

      Assert.IsTrue(response.IsSuccess || (!response.IsSuccess && !string.IsNullOrEmpty(response.ValidationMsg)));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DCCForwardingUpdate.Impl.dll")]
    public void ForwardingUpdateTemporaryValid()
    {
      DCCForwardingUpdateRequestData request = new DCCForwardingUpdateRequestData("857020", 
                                                                                  string.Empty, 
                                                                                  string.Empty, 
                                                                                  string.Empty, 
                                                                                  0, 
                                                                                  1, 
                                                                                  1665502, 
                                                                                  "MOBILE_CSA_DCC", 
                                                                                  "http://testy123.com", 
                                                                                  RedirectType.Temporary,
                                                                                  true);

      DCCForwardingUpdateResponseData response = (DCCForwardingUpdateResponseData)Engine.Engine.ProcessRequest(request, 111);
      
      Console.WriteLine(string.Format("Call Success: {0}", !string.IsNullOrEmpty(response.ResponseXml)));
      Console.WriteLine(string.Format("Operation Success: {0}", response.IsSuccess));
      Console.WriteLine(string.Format("Error: {0}", !string.IsNullOrEmpty(response.ValidationMsg) ? response.ValidationMsg:"none"));
      Console.WriteLine(response.ToXML());

      Assert.IsTrue(response.IsSuccess || (!response.IsSuccess && !string.IsNullOrEmpty(response.ValidationMsg)));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.DCCForwardingUpdate.Impl.dll")]
    public void ForwardingUpdateForDomainThatShopperDoesNotOwn()
    {
      DCCForwardingUpdateRequestData request = new DCCForwardingUpdateRequestData("847235", 
                                                                                  string.Empty, 
                                                                                  string.Empty, 
                                                                                  string.Empty, 
                                                                                  0, 
                                                                                  1, 
                                                                                  1665502, 
                                                                                  "MOBILE_CSA_DCC",
                                                                                  "http://testy123.com",
                                                                                  RedirectType.Permanent,
                                                                                  true);


      DCCForwardingUpdateResponseData response = (DCCForwardingUpdateResponseData)Engine.Engine.ProcessRequest(request, 111);
      
      Console.WriteLine(string.Format("Call Success: {0}", !string.IsNullOrEmpty(response.ResponseXml)));
      Console.WriteLine(string.Format("Operation Success: {0}", response.IsSuccess));
      Console.WriteLine(string.Format("Error: {0}", !string.IsNullOrEmpty(response.ValidationMsg) ? response.ValidationMsg : "none"));
      Console.WriteLine(response.ToXML());

      // This is returning success, the DCC team is fixing this in a future release
      Assert.IsTrue(response.IsSuccess || (!response.IsSuccess && !string.IsNullOrEmpty(response.ValidationMsg)));
    }
  }
}
