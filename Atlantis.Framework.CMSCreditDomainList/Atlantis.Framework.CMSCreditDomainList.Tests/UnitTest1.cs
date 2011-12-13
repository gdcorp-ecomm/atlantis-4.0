using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.CMSCreditDomainList.Interface;

namespace Atlantis.Framework.CMSCreditDomainList.Tests
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void TestMethod1()
    {
      List<int> productGroups = new List<int>();
      productGroups.Add(4);
      productGroups.Add(15);
      CMSCreditDomainListRequestData request = new CMSCreditDomainListRequestData("853392", string.Empty, string.Empty, string.Empty,
                                 0,productGroups,"eu");
      CMSCreditDomainListResponseData response = (CMSCreditDomainListResponseData)Engine.Engine.ProcessRequest(request, 347);
      Assert.IsTrue(response.IsSuccess);
    }
  }
}
