using System;
using System.Diagnostics;
using System.Net;
using System.Linq;
using Atlantis.Framework.WhoIsGetRegData.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.WhoIsGetRegData.Tests
{
  [TestClass]
  public class WhoIsGetRegDataTests
  {
    private const int _requestType = 319;
    private const string _shopperId = "858421"; //"840820";  //840820 842749; 
    private const string _domainName = "strongbad.com";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.WhoIsGetRegData.Impl.dll")]
    public void RunWhoIsGetRegDataTest()
    {
      var request = new WhoIsGetRegDataRequestData(_shopperId,
                                                  string.Empty,
                                                  string.Empty,
                                                  string.Empty,
                                                  0,
                                                 _domainName,
                                                 Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(ip => !IPAddress.IsLoopback(ip)).ToString(),
                                                  false) {RequestTimeout = new TimeSpan(0, 0, 0, 10)};


      var response = (WhoIsGetRegDataResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      Debug.WriteLine("*************************");
      Debug.WriteLine(string.Format("Requested Reg Data for Domain Name: {0}", _domainName ));
      Debug.WriteLine("*************************");
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
