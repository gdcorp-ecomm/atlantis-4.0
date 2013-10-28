using Atlantis.Framework.Shopper.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Shopper.Tests
{
  [TestClass]
  [DeploymentItem("atlantis.config")]
  [DeploymentItem("Atlantis.Framework.Shopper.Impl.dll")]
  public class VerifyCountryAllowedTests
  {
    [TestMethod]
    public void VerifyCountryAllowed()
    {
      VerifiyCountryAllowedRequestData request = new VerifiyCountryAllowedRequestData("co");
      VerifyCountryAllowedResponseData response = (VerifyCountryAllowedResponseData)Engine.Engine.ProcessRequest(request, 758);

    }

  }
}
