using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.DCCVerifyDomainConsolidate.Interface;
using System.Collections.Generic;
using System;


namespace Atlantis.Framework.DCCVerifyDomainConsolidate.Test
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class DCCVerifyDomainConsolidateTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void DCCVerifyDomainConsolidate()
    {
      DCCVerifyDomainConsolidateRequestData request = new DCCVerifyDomainConsolidateRequestData("859148", string.Empty, string.Empty, string.Empty, 0, 1);

      Dictionary<string, DomainRenewalData> domainIds = new Dictionary<string, DomainRenewalData>();

      DateTime syncExpirationDate = new DateTime(2010, 07, 31);
      //invlaid "Domain extension prohibits update"
      domainIds["ARVINDGURU.BIZ"] = new DomainRenewalData() { DomainId = 1668132, SyncExpirationDate = syncExpirationDate };
      domainIds["ARVINDGURU.MOBI"] = new DomainRenewalData() { DomainId = 1668133, SyncExpirationDate = syncExpirationDate };
      domainIds["ARVINDGURU.CC"] = new DomainRenewalData() { DomainId = 1668136, SyncExpirationDate = syncExpirationDate };

      //invalid "Domain recently consolidated"
      domainIds["ARVIND-GURU.COM"] = new DomainRenewalData() { DomainId = 1668150, SyncExpirationDate = syncExpirationDate };
      domainIds["BESTSHANAZ.COM"] = new DomainRenewalData() { DomainId = 1672438, SyncExpirationDate = syncExpirationDate };
      domainIds["THEARVINDGURU.COM"] = new DomainRenewalData() { DomainId = 1668138, SyncExpirationDate = syncExpirationDate };

      //valid
      domainIds["STRAIGHTLINEPAINTING.COM"] = new DomainRenewalData() { DomainId = 1679916, SyncExpirationDate = syncExpirationDate };
      domainIds["STRAIGHTLINEPAINTING.NET"] = new DomainRenewalData() { DomainId = 1679917, SyncExpirationDate = syncExpirationDate };
      domainIds["STRAIGHTLINEPAINTINGS.COM"] = new DomainRenewalData() { DomainId = 1679918, SyncExpirationDate = syncExpirationDate };

      foreach (DomainRenewalData domain in domainIds.Values)
      {
        request.DomainIDList.Add(domain);
      }


      DCCVerifyDomainConsolidateResponseData response = (DCCVerifyDomainConsolidateResponseData)Engine.Engine.ProcessRequest(request, 199);

      if (response.IsSuccess)
      {
        foreach (DomainRenewalData domain in domainIds.Values)
        {
          int actionResult;
          if (response.DomainActionResults.TryGetValue(domain.DomainId, out actionResult))
          {
            switch (domain.DomainId)
            {
              //disallowed
              case 1668132:
              case 1668133:
              case 1668136:
                Assert.IsTrue(actionResult == 10, "Correctly verified as \"Domain extension prohibits update.\"");
                break;

              //disallowed
              case 1668150:
              case 1672438:
              case 1668138:
                Assert.IsTrue(actionResult == 0, "Correctly verified as \"Domain recently consolidated\".");
                break;

              //allowed
              case 1679916:
              case 1679917:
              case 1679918:
                Assert.IsTrue(actionResult == 0, "Correctly verified as allowed.");
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
