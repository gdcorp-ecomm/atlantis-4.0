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
    const int _EXTENDEDTLDDATAREQUEST = 668;

    [TestMethod]
    public void ExtendedTLDDataForCom()
    {
      var request = new ExtendedTLDDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "com");
      var response = (ExtendedTLDDataResponseData)DataCache.DataCache.GetProcessRequest(request, _EXTENDEDTLDDATAREQUEST);
      string tldName;
      bool success = response.TryGetValue("tld", out tldName);
      Assert.IsTrue(success);
    }

    [TestMethod]
    public void ExtendedTLDDataForInvalid()
    {
      var request = new ExtendedTLDDataRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "raj");
      var response = (ExtendedTLDDataResponseData)DataCache.DataCache.GetProcessRequest(request, _EXTENDEDTLDDATAREQUEST);
      string tldName;
      bool success = response.TryGetValue("tld", out tldName);
      Assert.IsFalse(success);
    }
  }
}
