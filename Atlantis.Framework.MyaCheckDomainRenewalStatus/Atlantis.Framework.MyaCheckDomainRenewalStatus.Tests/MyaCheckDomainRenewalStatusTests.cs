using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.MyaCheckDomainRenewalStatus.Interface;
using System.Collections.Generic;
using System;


namespace Atlantis.Framework.MyaCheckDomainRenewalStatus.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class MyaCheckDomainRenewalStatusTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void MyaCheckDomainRenewalStatus()
    {
      MyaCheckDomainRenewalStatusRequestData request = new MyaCheckDomainRenewalStatusRequestData("859148", string.Empty, string.Empty, string.Empty, 0, 1);

      Dictionary<string, DomainRenewalData> domainIds = new Dictionary<string, DomainRenewalData>();
      //disallowed "Domain renew period max exceeded"
      domainIds["ARVINDGURU.BIZ"] = new DomainRenewalData() { DomainId = 1668132, RenewalLength = 2 };
      domainIds["ARVINDGURU.MOBI"] = new DomainRenewalData() { DomainId = 1668133, RenewalLength = 2 };
      domainIds["ARVINDGURU.CC"] = new DomainRenewalData() { DomainId = 1668136, RenewalLength = 2 };

      //allowed
      domainIds["SHANAZ.US"] = new DomainRenewalData() { DomainId = 1671386, RenewalLength = 2 };
      domainIds["ARVINDGURUS.US"] = new DomainRenewalData() { DomainId = 1668162, RenewalLength = 2 };
      domainIds["SHANAZ.BIZ"] = new DomainRenewalData() { DomainId = 1672424, RenewalLength = 2 };


      foreach (DomainRenewalData domain in domainIds.Values)
      {
        request.DomainIdList.Add(domain);
      }

      MyaCheckDomainRenewalStatusResponseData response = (MyaCheckDomainRenewalStatusResponseData)Engine.Engine.ProcessRequest(request, 192);

      if (response.IsSuccess)
      {
        foreach (DomainRenewalData domain in domainIds.Values)
        {
          int actionResult;
          if (response.DomainActionResults.TryGetValue(domain.DomainId, out actionResult))
          {
            switch (domain.DomainId)
            {
              //disallowed (somehow they are all allowed now
              case 166813200:
                Assert.IsTrue(actionResult != 0, "Correctly verified as invalid.");
                break;

              //allowed
              case 1668132:
              case 1668133:
              case 1668136:
              case 1671386:
              case 1668162:
              case 1672424:
                Assert.IsTrue(actionResult == 0, "Correctly verified as valid.");
                break;
            }
          }
          else
          {
            Assert.Fail(string.Format("Domain not verified"));
          }
        }
      }
      else
      {
        Assert.Fail("Failed to verify");
      }
    }

    
  }
}
