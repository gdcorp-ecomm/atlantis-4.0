using System;
using System.Diagnostics;
using Atlantis.Framework.QSCBlockIp.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.QSCBlockIp.Tests
{
  [TestClass]
  public class QSCBlockIpTests
  {
    [TestMethod]
    public void BlockValidIPAddress()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _ipAddress = "192.168.1.1";
      int requestId = 551;

      QSCBlockIpRequestData request = new QSCBlockIpRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _ipAddress);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCBlockIpResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCBlockIpResponseData;

      Assert.IsTrue(response.IsSuccess);

      Console.WriteLine(response.ToXML());
    }

    [TestMethod]
    public void BlockInvalidIPAddress()
    {
      string _shopperId = "837435";
      string _accountUid = "265ddd62-2f88-11de-baa9-005056956427";
      string _ipAddress = "192";
      int requestId = 551;

      QSCBlockIpRequestData request = new QSCBlockIpRequestData(_shopperId, "", string.Empty, string.Empty, 1, _accountUid, _ipAddress);

      request.RequestTimeout = TimeSpan.FromSeconds(30);

      QSCBlockIpResponseData response = Engine.Engine.ProcessRequest(request, requestId) as QSCBlockIpResponseData;

      Assert.IsFalse(response.IsSuccess);

      Console.WriteLine(response.ToXML());
    }
  }
}
