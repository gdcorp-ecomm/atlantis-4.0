using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Atlantis.Framework.DataCache.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.DataCache.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.DataCacheService.dll")]
  [DeploymentItem("Atlantis.Framework.DataCacheGeneric.Impl.dll")]
  public class DataCacheMulticastTests
  {
    const int REQUEST_TYPE = 111972;

    [TestMethod]
    public void ClearInProcessCachedData()
    {
      string data1 = GetTestCachedData("ABCD");
      string data2 = GetTestCachedData("ABCD");
      Assert.IsTrue(object.ReferenceEquals(data1, data2));

      TriggerClearCache();

      string data3 = GetTestCachedData("ABCD");

      //  NO LONGER CACHED
      Assert.IsFalse(object.ReferenceEquals(data2, data3));
    }

    [TestMethod]
    public void ClearInProcessCachedDataWithSlash()
    {
      string data1 = GetTestCachedData("ABCD");
      string data2 = GetTestCachedData("ABCD");
      Assert.IsTrue(object.ReferenceEquals(data1, data2));

      TriggerClearCache("/plid");

      string data3 = GetTestCachedData("ABCD");

      //  NO LONGER CACHED
      Assert.IsFalse(object.ReferenceEquals(data2, data3));
    }

    [TestMethod]
    public void ClearInProcessCachedDataWithSpecificKey()
    {
      string data1 = GetTestCachedData("ABCD111");
      string data2 = GetTestCachedData("ABCD111");
      string data3 = GetTestCachedData("ABCD222");
      string data4 = GetTestCachedData("ABCD222");
      Assert.IsTrue(object.ReferenceEquals(data1, data2));
      Assert.IsTrue(object.ReferenceEquals(data3, data4));
      Assert.AreNotEqual(data1, data3);

      TriggerClearCache("|ABCD111");

      //  NO LONGER CACHED
      string data5 = GetTestCachedData("ABCD111");
      Assert.IsFalse(object.ReferenceEquals(data1, data5));

      //  Still cached
      string data6 = GetTestCachedData("ABCD222");
      Assert.IsTrue(object.ReferenceEquals(data3, data6));
    }

    [TestMethod]
    public void ClearInProcessCachedDataWithInvalidKey()
    {
      string data1 = GetTestCachedData("ABCD111");
      string data2 = GetTestCachedData("ABCD111");
      string data3 = GetTestCachedData("ABCD222");
      string data4 = GetTestCachedData("ABCD222");
      Assert.IsTrue(object.ReferenceEquals(data1, data2));
      Assert.IsTrue(object.ReferenceEquals(data3, data4));
      Assert.AreNotEqual(data1, data3);

      TriggerClearCache("|ABCD333");

      //  Still cached
      string data5 = GetTestCachedData("ABCD111");
      Assert.IsTrue(object.ReferenceEquals(data1, data5));

      //  Still cached
      string data6 = GetTestCachedData("ABCD222");
      Assert.IsTrue(object.ReferenceEquals(data3, data6));
    }

    [TestMethod]
    public void ClearInProcessCachedDataWithKeyDelimiterButNoKey()
    {
      string data1 = GetTestCachedData("ABCD111");
      string data2 = GetTestCachedData("ABCD111");
      string data3 = GetTestCachedData("ABCD222");
      string data4 = GetTestCachedData("ABCD222");
      Assert.IsTrue(object.ReferenceEquals(data1, data2));
      Assert.IsTrue(object.ReferenceEquals(data3, data4));
      Assert.AreNotEqual(data1, data3);

      TriggerClearCache("|");

      //  Still cached
      string data5 = GetTestCachedData("ABCD111");
      Assert.IsTrue(object.ReferenceEquals(data1, data5));

      //  Still cached
      string data6 = GetTestCachedData("ABCD222");
      Assert.IsTrue(object.ReferenceEquals(data3, data6));
    }

    private void TriggerClearCache(string suffix = "")
    {
      UdpClient udpclient = new UdpClient();

      IPAddress multicastaddress = IPAddress.Parse("224.7.8.13");
      udpclient.JoinMulticastGroup(multicastaddress);
      IPEndPoint remoteep = new IPEndPoint(multicastaddress, 7813);

      Byte[] buffer = null;
      buffer = Encoding.ASCII.GetBytes("GetProcessRequest" + REQUEST_TYPE.ToString() + suffix + "*");

      System.Threading.Thread.Sleep(2000);
      udpclient.Send(buffer, buffer.Length, remoteep);
      System.Threading.Thread.Sleep(5000);
    }

    private string GetTestCachedData(string dataValue)
    {
      var request = new MockCachedTripletRequestData();
      request.RequestValue = dataValue;
      var response = (MockCachedTripletResponseData)DataCache.GetProcessRequest(request, REQUEST_TYPE);
      return response.DataValue;
    }
  }
}
