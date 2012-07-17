using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atlantis.Framework.ReceiptSurveyInsert.Interface;

namespace Atlantis.Framework.ReceiptSurveyInsert.Test
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("app.config")]
    public void AddReceiptSurvey()
    {
      var request = new ReceiptSurveyInsertRequestData("99907789", string.Empty, string.Empty, string.Empty, 0, "es", 1, 12);
      var response = Engine.Engine.ProcessRequest(request, 564) as ReceiptSurveyInsertResponseData;
      Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("app.config")]
    public void FailNoShopperId()
    {
      var request = new ReceiptSurveyInsertRequestData("", string.Empty, string.Empty, string.Empty, 0, "us", 1, 4);
      var response = Engine.Engine.ProcessRequest(request, 564) as ReceiptSurveyInsertResponseData;

    }
    
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("app.config")]
    public void FailNoCountry()
    {
      var request = new ReceiptSurveyInsertRequestData("", string.Empty, string.Empty, string.Empty, 0, string.Empty, 1, 4);
      var response = Engine.Engine.ProcessRequest(request, 564) as ReceiptSurveyInsertResponseData;

    }
  }
}
