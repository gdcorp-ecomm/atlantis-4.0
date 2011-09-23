using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.HCCAppCount.Interface;

namespace Atlantis.Framework.HCCAppCount.Tests
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class HCCAppCountTests
  {
    public HCCAppCountTests()
    {
      //
      // TODO: Add constructor logic here
      //
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void HCCAppCountTest()
    {
      HCCAppCountRequestData request = new HCCAppCountRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      request.RequestTimeout = TimeSpan.FromMinutes(1);
      HCCAppCountResponseData response = (HCCAppCountResponseData) Engine.Engine.ProcessRequest(request, 251);

      if (response.AppCount <= 0)
      {
        Assert.Fail("Didn't get a positve number for installed app count.");
      }
    }
  }
}
