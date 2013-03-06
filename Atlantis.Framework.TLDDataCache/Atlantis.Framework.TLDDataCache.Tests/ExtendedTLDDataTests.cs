using Atlantis.Framework.TLDDataCache.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atlantis.Framework.TLDDataCache.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.TLDDataCache.Impl.dll")]
  [DeploymentItem("Interop.gdDataCacheLib.dll")]
  public class ExtendedTLDDataTests
  {
    const int _EXTENDEDTLDDATAREQUEST = 665;

    [TestMethod]
    public void ExtendedTLDDataForCom()
    {
      var request = new ExtendedTLDDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "com");
      var response = (ExtendedTLDDataResponseData)DataCache.DataCache.GetProcessRequest(request, _EXTENDEDTLDDATAREQUEST);
      Assert.IsTrue(response.TLDData.Count == 1);
    }

    [TestMethod]
    public void ExtendedTLDDataForInvalid()
    {
      var request = new ExtendedTLDDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "raj");
      var response = (ExtendedTLDDataResponseData)DataCache.DataCache.GetProcessRequest(request, _EXTENDEDTLDDATAREQUEST);
      Assert.IsTrue(response.TLDData.Count == 0);
    }
  }
}
