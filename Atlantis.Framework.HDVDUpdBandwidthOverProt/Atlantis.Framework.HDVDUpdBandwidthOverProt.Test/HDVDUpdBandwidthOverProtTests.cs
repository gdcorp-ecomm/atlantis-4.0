using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Atlantis.Framework.HDVDUpdBandwidthOverProt.Impl;
using Atlantis.Framework.HDVDUpdBandwidthOverProt.Interface;


namespace Atlantis.Framework.HDVDUpdBandwidthOverProt.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class GetHDVDUpdBandwidthOverProtTests
  {
    string _shopperId = "858421";
    Guid accountUid = new Guid("d11319d0-4d10-11e1-83a0-0050569575d8");
    private const int _requestType = 999;


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void HDVDUpdBandwidthOverProtTest()
    {
      bool suspend = false;
      bool isEnabled = true;

      HDVDUpdBandwidthOverProtRequestData request = new HDVDUpdBandwidthOverProtRequestData(_shopperId
         , string.Empty
         , string.Empty
         , string.Empty
         , 0
         , suspend
         , isEnabled,
         accountUid.ToString()
         );

      HDVDUpdBandwidthOverProtResponseData response = (HDVDUpdBandwidthOverProtResponseData)Engine.Engine.ProcessRequest(request, _requestType);


      Debug.WriteLine(response.ToXML());
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
