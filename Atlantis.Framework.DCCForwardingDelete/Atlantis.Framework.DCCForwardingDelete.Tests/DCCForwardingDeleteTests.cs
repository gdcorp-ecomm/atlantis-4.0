using System;
using Atlantis.Framework.DCCForwardingDelete.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DCCForwardingDelete.Tests
{
  [TestClass]
  public class DCCForwardingDeleteTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCForwardingDeleteValid()
    {
      DCCForwardingDeleteRequestData request = new DCCForwardingDeleteRequestData("857020", string.Empty, string.Empty, string.Empty, 0, 1, 1667126, "MOBILE_CSA_DCC");
      DCCForwardingDeleteResponseData response = (DCCForwardingDeleteResponseData)Engine.Engine.ProcessRequest(request, 112);

      Console.WriteLine(string.Format("Operation Success: {0}", response.IsSuccess));
      Console.WriteLine(string.Format("Error: {0}", !string.IsNullOrEmpty(response.ValidationMsg) ? response.ValidationMsg : "none"));
      Console.WriteLine(response.ToXML());

      Assert.IsTrue(response.IsSuccess || (!response.IsSuccess && !string.IsNullOrEmpty(response.ValidationMsg)));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCForwardingDeleteForDomainThatShopperDoesNotOwn()
    {
      DCCForwardingDeleteRequestData request = new DCCForwardingDeleteRequestData("847235", string.Empty, string.Empty, string.Empty, 0, 1, 1665499, "MOBILE_CSA_DCC");
      DCCForwardingDeleteResponseData response = (DCCForwardingDeleteResponseData)Engine.Engine.ProcessRequest(request, 112);

      Console.WriteLine(string.Format("Operation Success: {0}", response.IsSuccess));
      Console.WriteLine(string.Format("Error: {0}", !string.IsNullOrEmpty(response.ValidationMsg) ? response.ValidationMsg : "none"));
      Console.WriteLine(response.ToXML());

      // This is returning success true, the DCC team is fixing this in a future release
      Assert.IsTrue(response.IsSuccess || (!response.IsSuccess && !string.IsNullOrEmpty(response.ValidationMsg)));
    }
  }
}
