using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.ECCGetEmailPlanInfo.Tests
{
  [TestClass]
  public class ECCGetPlanInfoTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("bin/netconnect.dll")]
    public void ECCGetPlanInformationTest()
    {
      var request = new ECCGetEmailPlanInfo.Interface.EccGetEmailPlanInfoRequestData("842904", "", "", "", 0, 1, Ecc.Interface.Enums.EmailTypes.Emails, new TimeSpan(0, 0, 10), "7a67b9eb-dee8-11e0-b49b-0050569575d8", "", true);

      var response = (ECCGetEmailPlanInfo.Interface.EccGetEmailPlanInfoResponseData)Engine.Engine.ProcessRequest(request, 226);

      Debug.WriteLine("QuotaAssigned: " + response.EmailPlanDetails.QuotaAssigned.ToString());
      Debug.WriteLine("QuotaRemaining: " + response.EmailPlanDetails.QuotaRemaning.ToString());
      Debug.WriteLine("DiskUsed: " + response.EmailPlanDetails.DiskSpaceMbUsed.ToString());
      Debug.WriteLine("DiskRemaining: " + response.EmailPlanDetails.DiskSpaceMbRemaining.ToString());
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("bin/netconnect.dll")]
    public void ECCGetPlanInformationNoUsageTest()
    {
      var request = new ECCGetEmailPlanInfo.Interface.EccGetEmailPlanInfoRequestData("857527", "", "", "", 0, 1, Ecc.Interface.Enums.EmailTypes.Emails, new TimeSpan(0, 0, 10), "271a0a14-fafd-11de-9d9e-005056956427", "", false);

      var response = (ECCGetEmailPlanInfo.Interface.EccGetEmailPlanInfoResponseData)Engine.Engine.ProcessRequest(request, 226);

      Debug.WriteLine("QuotaAssigned: " + response.EmailPlanDetails.QuotaAssigned.ToString());
      Debug.WriteLine("QuotaRemaining: " + response.EmailPlanDetails.QuotaRemaning.ToString());
      Debug.WriteLine("DiskUsed: " + response.EmailPlanDetails.DiskSpaceMbUsed.ToString());
      Debug.WriteLine("DiskRemaining: " + response.EmailPlanDetails.DiskSpaceMbRemaining.ToString());
    }
  }
}
