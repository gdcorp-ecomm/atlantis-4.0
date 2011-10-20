using System;
using Atlantis.Framework.SurveyService.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.SurveyService.Tests
{
  [TestClass]
  public class SurveyServiceTests
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void SendValidSurvey()
    {
      SurveyServiceRequestData requestData = new SurveyServiceRequestData("847235", string.Empty, string.Empty, string.Empty, 0)
      {
        IPAddress = "172.16.44.27",
        AdVersion = 181, // First Impressions Commercial
        AgeGroupID = 6,
        PoliticalID = 3,
        Answers = "10=47,11=53,9=40"
      };

      SurveyServiceResponseData responseData = (SurveyServiceResponseData)Engine.Engine.ProcessRequest(requestData, 36);

      Assert.IsTrue(responseData.IsSuccess);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void SendValidSurveyAsync()
    {
      SurveyServiceRequestData requestData = new SurveyServiceRequestData("847235", string.Empty, string.Empty, string.Empty, 0)
      {
        IPAddress = "172.16.44.27",
        AdVersion = 181, // First Impressions Commercial
        AgeGroupID = 6,
        PoliticalID = 3,
        Answers = "10=47,11=53,9=40"
      };

      Engine.Engine.BeginProcessRequest(requestData, 37, EndSurveyResult, null);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void SendInValidSurvey()
    {
      SurveyServiceRequestData requestData = new SurveyServiceRequestData("847235", string.Empty, string.Empty, string.Empty, 0)
      {
        IPAddress = string.Empty,
        AdVersion = -1,
        AgeGroupID = -1,
        PoliticalID = -1,
        Answers = string.Empty
      };

      SurveyServiceResponseData responseData = (SurveyServiceResponseData)Engine.Engine.ProcessRequest(requestData, 36);

      Assert.IsTrue(!responseData.IsSuccess);
    }

    private void EndSurveyResult(IAsyncResult oAsyncResult)
    {
      
    }
  }
}
