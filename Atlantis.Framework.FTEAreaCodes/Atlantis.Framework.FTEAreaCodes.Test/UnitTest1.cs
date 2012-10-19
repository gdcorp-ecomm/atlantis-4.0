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
    [DeploymentItem("app.config")]
    public void TestMethod1()
    {      
      var request = new FTEAreaCodesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0, "US");
      var response = DataCache.DataCache.GetProcessRequest(request, 611) as FTEAreaCodesResponseData;
      
      Console.WriteLine(response.ToString());
    }
  }
}
