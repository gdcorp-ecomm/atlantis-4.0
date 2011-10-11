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
    public void ECCGetPlanInformationTest()
    {
      var request = new ECCGetEmailPlanInfo.Interface.EccGetEmailPlanInfoRequestData("842904", "", "", "", 0, 1, Ecc.Interface.Enums.EmailTypes.Emails, new TimeSpan(0, 0, 10), "7a67b9eb-dee8-11e0-b49b-0050569575d8", "", true);

      var response = (ECCGetEmailPlanInfo.Interface.EccGetEmailPlanInfoResponseData)Engine.Engine.ProcessRequest(request, 226);

      Debug.WriteLine(response.EmailPlanDetails.QuotaAssigned.ToString());
    }
  }
}
