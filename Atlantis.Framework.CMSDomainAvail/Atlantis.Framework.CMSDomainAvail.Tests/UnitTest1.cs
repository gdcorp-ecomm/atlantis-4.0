using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.CMSDomainAvail.Interface;

namespace Atlantis.Framework.CMSDomainAvail.Tests
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void TestMethod1()
    {
      List<string> DomainsToCheck= new List<string>();
      DomainsToCheck.Add("12341234ASDF.COM");
      DomainsToCheck.Add("12341ASFASF12343FFDFDFDFD.BIZ");
      Engine.Engine.ReloadConfig();
      CMSDomainAvailRequestData request = new CMSDomainAvailRequestData("75866", string.Empty, string.Empty, string.Empty,
                                 0, DomainsToCheck);
      CMSDomainAvailResponseData response = (CMSDomainAvailResponseData)Engine.Engine.ProcessRequest(request, 361);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
