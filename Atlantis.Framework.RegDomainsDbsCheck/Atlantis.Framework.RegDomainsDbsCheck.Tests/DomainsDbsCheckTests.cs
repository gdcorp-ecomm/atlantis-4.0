using System;
using System.Collections.Generic;
using Atlantis.Framework.RegDomainsDbsCheck.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.RegDomainsDbsCheck.Tests
{
  [TestClass]
  public class DomainsDbsCheckTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DomainsDbsCheck()
    {
      /*
       * 
       * request xml
       * <domains><domain>apple.com</domain></domains>
       * 
       * response xml
       * <domains><startTime>01:17:20.6714</startTime><domain isDbsCapable='true' auctionActive='false' auctionLink='none'>apple.com</domain><endTime>01:17:23.5153</endTime></domains>
       * 
       * */
      List<string> domainNames = new List<string>() { "apple.com", "microsoft.com" };
      RegDomainsDbsCheckRequestData rq = new RegDomainsDbsCheckRequestData("77311",
        string.Empty, string.Empty, string.Empty, 0, domainNames);
      rq.RequestTimeout = TimeSpan.FromMilliseconds(6000);
      RegDomainsDbsCheckResponseData rs
        = (RegDomainsDbsCheckResponseData)Engine.Engine.ProcessRequest(rq, 355);
      Assert.IsTrue(rs.IsValid);
    }
  }
}
