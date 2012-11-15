using System;
using System.Diagnostics;
using System.Net;
using System.Linq;
using Atlantis.Framework.WhoIsGetInfo.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.WhoIsGetInfo.Tests
{
  [TestClass]
  public class WhoIsGetInfoTests
  {
    private const int _requestType = 202;
    private const string _shopperId = "858421"; //"840820";  //840820 842749; 
    private const string _domainName = "hatdragon.com";

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("Atlantis.Framework.WhoIsGetInfo.Impl.dll")]
    public void RunWhoIsTest()
    {
      var request = new WhoIsGetInfoRequestData(_shopperId,
                                                string.Empty,
                                                string.Empty,
                                                string.Empty,
                                                0,
                                                _domainName,
                                                privateLabelId: 1,
                                                clientIp: Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(ip => !IPAddress.IsLoopback(ip)).ToString(),
                                wasCaptchaEntered: false) { RequestTimeout = new TimeSpan(0, 0, 0, 10) };

      var response = (WhoIsGetInfoResponseData)Engine.Engine.ProcessRequest(request, _requestType);
      Debug.WriteLine("*************************");
      Debug.WriteLine(string.Format("Requested Domain Name: {0}", response.whoIsInfo.Name));
      Debug.WriteLine("*************************");
      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}