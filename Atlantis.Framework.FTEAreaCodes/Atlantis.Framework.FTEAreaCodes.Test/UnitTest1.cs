using System;
using Atlantis.Framework.FTEAreaCodes.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atlantis.Framework.FTEAreaCodes.Test
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestMethod1()
    {
      var request = new FTEAreaCodesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);

      //var response = Engine.Engine.ProcessRequest(request, 609) as FTEAreaCodesResponseData;
      var response = DataCache.DataCache.GetProcessRequest(request, 609) as FTEAreaCodesResponseData;
      
      Console.WriteLine(response.ToString());
    }


    [TestMethod]
    [DeploymentItem("atlantis.config")]
    public void TestMethod2()
    {
      var request = new FTEStateAreaCodesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "US-CO");

      //var response = Engine.Engine.ProcessRequest(request, 610) as FTEStateAreaCodesResponseData;
      var response = DataCache.DataCache.GetProcessRequest(request, 610) as FTEStateAreaCodesResponseData;

      Console.WriteLine(response.ToString());
    }
  }
}
