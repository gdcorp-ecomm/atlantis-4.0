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
	  [DeploymentItem("atlantis.config")]
    public void PremiumDNSTestStatus()
    {
      TimeSpan requestTimeout = new TimeSpan(0, 0, 0, 2, 500);

     GetPremiumDNSStatusRequestData request = new GetPremiumDNSStatusRequestData(_shopperId
        , string.Empty
        , string.Empty
        , string.Empty
        , 0
        , 1);

      request.RequestTimeout = requestTimeout;

      int _requestType = 288;

      GetPremiumDNSStatusResponseData response = (GetPremiumDNSStatusResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      


	  
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
      Assert.IsTrue(response.HasPremiumDNS);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void PremiumDNSTestNameservers()
    {

      TimeSpan requestTimeout = new TimeSpan(0, 0, 0, 2, 500);

      GetPremiumDNSDefaultNameServersRequestData request = new GetPremiumDNSDefaultNameServersRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 1);

      request.RequestTimeout = requestTimeout;

      int _requestType = 289;

      GetPremiumDNSDefaultNameServersResponseData response = (GetPremiumDNSDefaultNameServersResponseData)Engine.Engine.ProcessRequest(request, _requestType);

      Assert.IsTrue(response.Nameservers.Count > 0);

      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
