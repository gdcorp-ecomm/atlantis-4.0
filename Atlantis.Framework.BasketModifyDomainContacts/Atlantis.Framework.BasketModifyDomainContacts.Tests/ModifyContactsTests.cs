using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.BasketModifyDomainContacts.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.BasketModifyDomainContacts.Tests
{
  [TestClass]
  public class DomainsDbsCheckTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void ModifyDomainContact()
    {
      var c = "<contactInfo link_id=\"b88e970b-d87e-4ddc-88ed-e6489d0fb93f\"><contact fname=\"Test\" lname=\"DomainsDbsCheckTests\" email=\"test@DomainsDbsCheckTests.com\" sa1=\"mexico/usa #20\" sa2=\"300/500\" org=\"\" cc=\"Germany\" city=\"aurora\" pc=\"80017\" phone=\"123-4567\" sp=\"berlin\" fax=\"\" co=\"\" contactType=\"0\" /><contact fname=\"Test\" lname=\"DomainsDbsCheckTests\" email=\"test@DomainsDbsCheckTests.com\" sa1=\"mexico/usa #20\" sa2=\"300/500\" org=\"\" cc=\"Germany\" city=\"aurora\" pc=\"80017\" phone=\"123-4567\" sp=\"berlin\" fax=\"\" co=\"\" contactType=\"1\" /><contact fname=\"Test\" lname=\"DomainsDbsCheckTests\" email=\"test@DomainsDbsCheckTests.com\" sa1=\"mexico/usa #20\" sa2=\"300/500\" org=\"\" cc=\"Germany\" city=\"aurora\" pc=\"80017\" phone=\"123-4567\" sp=\"berlin\" fax=\"\" co=\"\" contactType=\"2\" /><contact fname=\"Test\" lname=\"DomainsDbsCheckTests\" email=\"test@DomainsDbsCheckTests.com\" sa1=\"mexico/usa #20\" sa2=\"300/500\" org=\"\" cc=\"Germany\" city=\"aurora\" pc=\"80017\" phone=\"123-4567\" sp=\"berlin\" fax=\"\" co=\"\" contactType=\"3\" /></contactInfo>";
      var domainNames = new List<string>() { "LKAJSDLKAJSDLKJ32LK2J3LER2.COM", "BFSGBXFXVBXCVB.COM" };
      var rq = new BasketModifyDomainContactsRequestData("850398", string.Empty, string.Empty, string.Empty, 0, c, domainNames);
      rq.RequestTimeout = TimeSpan.FromSeconds(6);
      var rs = (BasketModifyDomainContactsResponseData)Engine.Engine.ProcessRequest(rq, 606);
      Assert.IsTrue(rs.IsSuccess);
    }
  }
}
