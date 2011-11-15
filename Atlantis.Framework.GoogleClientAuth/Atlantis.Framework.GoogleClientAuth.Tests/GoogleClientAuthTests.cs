using System;
using System.Diagnostics;
using Atlantis.Framework.GoogleClientAuth.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.GoogleClientAuth.Tests
{
  [TestClass]
  public class GoogleClientAuthTests
  {
    private static void WriteResults(GoogleClientAuthResponseData responseData)
    {
      Console.WriteLine(string.Format("Auth Token: {0}", responseData.ClientAuthToken));
      Console.WriteLine(string.Format("Service Unavailable: {0}", responseData.ServiceUnavailable ? "true" : "false"));

      Debug.WriteLine(string.Format("Auth Token: {0}", responseData.ClientAuthToken));
      Debug.WriteLine(string.Format("Service Unavailable: {0}", responseData.ServiceUnavailable ? "true" : "false"));
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void ClientAuthValid()
    {
      GoogleClientAuthRequestData requestData = new GoogleClientAuthRequestData(GoogleClientAuthServiceType.AndroidPush,
                                                                                GoogleClientAuthAccountType.Google,
                                                                                "GoogleClientAuthTests",
                                                                                "847235",
                                                                                "http://www.GoogleClientAuthTests.com/",
                                                                                string.Empty,
                                                                                Guid.NewGuid().ToString(),
                                                                                1);

      try
      {
        GoogleClientAuthResponseData responseData = (GoogleClientAuthResponseData)Engine.Engine.ProcessRequest(requestData, 444);

        WriteResults(responseData);
        Assert.IsTrue(responseData.IsSuccess);
      }
      catch (Exception ex)
      {
        Assert.Fail(ex.Message);
      }
    }
  }
}
