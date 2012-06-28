using System;
using System.Diagnostics;
using Atlantis.Framework.MktgSubscribeGet.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.MktgSubscribeGet.Tests
{
  [TestClass]
  public class MktgSubscribeGetTests
  {
    private static void WriteResults(MktgSubscribeGetResponseData responseData)
    {
      if (responseData != null && responseData.OptInDictionary != null)
      {
        foreach (var mktgSubscribeOptIn in responseData.OptInDictionary.Values)
        {
          Debug.WriteLine(string.Format("Id: {0}, Description: {1}", mktgSubscribeOptIn.Id, mktgSubscribeOptIn.Description));
          Console.WriteLine(string.Format("Id: {0}, Description: {1}", mktgSubscribeOptIn.Id, mktgSubscribeOptIn.Description));
        }
      }
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void GetOptInValid()
    {
      MktgSubscribeGetRequestData requestData = new MktgSubscribeGetRequestData("850774",
                                                                                "http://www.MktgSubscribeGetTests.com",
                                                                                string.Empty,
                                                                                Guid.NewGuid().ToString(),
                                                                                1,
                                                                                "sthota@godaddy.com",
                                                                                1);

      MktgSubscribeGetResponseData responseData = (MktgSubscribeGetResponseData)Engine.Engine.ProcessRequest(requestData, 555);

      WriteResults(responseData);
      Assert.IsTrue(responseData.OptInDictionary != null && responseData.OptInDictionary.Count > 0);
    }

    [TestMethod]
    [DeploymentItem("atlantis.config")]
    [DeploymentItem("App.config")]
    public void GetOptInEmptyEmail()
    {
      MktgSubscribeGetRequestData requestData = new MktgSubscribeGetRequestData("850774",
                                                                                "http://www.MktgSubscribeGetTests.com",
                                                                                string.Empty,
                                                                                Guid.NewGuid().ToString(),
                                                                                1,
                                                                                string.Empty,
                                                                                1);

      try
      {
        Engine.Engine.ProcessRequest(requestData, 555);
      }
      catch(Exception ex)
      {
        // Expected exception for empty email
        Debug.WriteLine(ex.Message);
        Console.WriteLine(ex.Message);
      }
    }
  }
}
