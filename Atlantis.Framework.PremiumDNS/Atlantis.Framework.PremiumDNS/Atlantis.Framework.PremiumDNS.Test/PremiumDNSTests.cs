using System;
using System.Diagnostics;
using Atlantis.Framework.PremiumDNS.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.PremiumDNS.Test
{
  [TestClass]
  public class GetPremiumDNSTests
  {
    private const string _shopperId = "123415";  //test account provided by DNS group
	
    [TestMethod]
    public void PremiumDNSTestStatus()
    {
     GetPremiumDNSStatusRequestData request = new GetPremiumDNSStatusRequestData(_shopperId, 1);
     request.RequestTimeout = new TimeSpan(0, 0, 0, 2, 500);
      GetPremiumDNSStatusResponseData response
        = (GetPremiumDNSStatusResponseData)Engine.Engine.ProcessRequest(request, 288);
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.HasPremiumDNS);
    }

    [TestMethod]
    public void PremiumDNSTestNameservers()
    {
      GetPremiumDNSDefaultNameServersRequestData request = new GetPremiumDNSDefaultNameServersRequestData(_shopperId, 1);
      request.RequestTimeout = new TimeSpan(0, 0, 0, 2, 500);
      GetPremiumDNSDefaultNameServersResponseData response
        = (GetPremiumDNSDefaultNameServersResponseData)Engine.Engine.ProcessRequest(request, 289);
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.NameserversByTld.Count > 0);
      Assert.IsTrue(response.Nameservers.Count > 0);
    }
  }
}
