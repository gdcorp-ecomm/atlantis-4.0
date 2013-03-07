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
  public class ValidDotTypesTests
  {
    const int _VALIDDOTTYPESREQUEST = 667;

    [TestMethod]
    public void ValidDotTypes()
    {
      var request = new ValidDotTypesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      var response = (ValidDotTypesResponseData)DataCache.DataCache.GetProcessRequest(request, _VALIDDOTTYPESREQUEST);
      Assert.IsTrue(response.IsValidTld("com"));

      int tldId;
      bool success = response.TryGetTldId("com", out tldId);
      Assert.IsTrue(success);
    }
  }
}
