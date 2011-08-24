using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DCCValidateNS.Interface;
using Atlantis.Framework.Engine;

namespace DCCValidateNSTests
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void TestMethod1()
    {


      TimeSpan requestTimeout = new TimeSpan(0, 0, 0, 2, 500);
      List<string> nameServer=new List<string>();
      nameServer.Add("Hello");
      nameServer.Add("ns01.gknschangetest3.us");
      DCCValidateNSRequestData request = new DCCValidateNSRequestData("610"
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , 1,nameServer);

      request.RequestTimeout = requestTimeout;

      int _requestType = 326;

      DCCValidateNSResponseData response = (DCCValidateNSResponseData)Engine.ProcessRequest(request, _requestType);

      //DNSAPI.dnsverifyapi oSvc = new DNSAPI.dnsverifyapi();
      //oSvc.Url = "http://dnsapi.test.secureserver-net.ide/dnsverifyapi.php";
      //oSvc.clientAuth = new DNSAPI.authDataType();
      //oSvc.clientAuth.clientid = "mya";
      //oSvc.custInfo = new DNSAPI.custDataType();
      //oSvc.custInfo.shopperid = "610";
      //oSvc.custInfo.resellerid = 1;
      //vanityNameServerType[] oResult = oSvc.getVanityNSByShopper();
      //string[] nameServer = new string[2];
      //nameServer[0] = "Hello";
      //nameServer[1] = "ns01.gknschangetest3.us";
      //validateNSResponseType oResult2 = oSvc.validateNS(nameServer);
    }
  }
}
